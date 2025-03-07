using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPurge
{
    public partial class MainForm: Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Lancer la purge lorsqu'on clique sur le bouton
        private void btnStartPurge_Click(object sender, EventArgs e)
        {
            // Exécuter la purge dans un thread de fond
            Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("Démarrage du programme AutoPurge...");

                    // Charger la configuration depuis le dossier de l'application
                    string configFilePath = Path.Combine(Application.StartupPath, "config.json");
                    ConfigService configService = new ConfigService(configFilePath);
                    ConfigModel config = configService.LoadConfig();

                    // Initialisation du logger et récupération du chemin du fichier log
                    ILogger logger = new Logger();
                    string logFilePath = ((Logger)logger).LogFilePath;
                    logger.LogInfo("Début de la purge des fichiers...");

                    // Exécuter la purge pour chaque configuration de chemin
                    FilePurger purger = new FilePurger(logger);
                    foreach (var pathConfig in config.Paths)
                    {
                        purger.PurgeFiles(pathConfig);
                    }

                    logger.LogInfo("Purge terminée avec succès.");

                    // Attendre que le fichier log soit complètement écrit
                    WaitForLogFile(logFilePath);

                    // Envoyer l’email récapitulatif
                    logger.LogInfo("Envoi de l'email récapitulatif...");
                    EmailSender emailSender = new EmailSender(config.Email);
                    emailSender.SendEmail(logFilePath);
                    logger.LogInfo("Email envoyé avec succès.");

                    // Mise à jour de l'interface utilisateur une fois le traitement terminé
                    // (Utilisation de Invoke pour accéder au thread UI)
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show("Processus terminé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                catch (Exception ex)
                {
                    // En cas d'erreur, on affiche le message sur le thread UI
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }


        // Ouvrir la configuration du fichier JSON lorsqu'on clique sur le bouton
        private void btnConfigurePurge_Click(object sender, EventArgs e)
        {
            ConfigurationForm configForm = new ConfigurationForm();
            configForm.ShowDialog(); // Ouvre la fenêtre en mode modal
        }

        /// <summary>
        /// Attend que le fichier log soit totalement écrit et ne soit plus verrouillé.
        /// </summary>
        /// <param name="filePath">Chemin du fichier log</param>
        private static void WaitForLogFile(string filePath)
        {
            int attempts = 0;
            while (attempts < 10) // Essaye jusqu'à 10 fois (5 secondes max)
            {
                try
                {
                    // Vérifie si le fichier est accessible en lecture
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        Console.WriteLine("Fichier log prêt pour l'envoi.");
                        return;
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("Fichier log encore en cours d'écriture... Attente...");
                    Thread.Sleep(500); // Attend 500 ms avant de réessayer
                    attempts++;
                }
            }
            Console.WriteLine("Attention : Le fichier log est peut-être encore en cours d'utilisation.");
        }
    }
}
