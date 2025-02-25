using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPurge
{
    static class Program
    {
        [STAThread] // Important pour éviter l'erreur
        static void Main(string[] args)
        {
            // Vérifier si l'application est lancée par le Task Scheduler (via un argument)
            if (args.Length > 0 && args[0] == "auto")
            {
                LancerPurge(); // Exécute directement la purge
                return; // Quitte après exécution
            }

            // Sinon, afficher l'interface utilisateur
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Fonction qui exécute la purge des fichiers
        /// </summary>
        static void LancerPurge()
        {
            try
            {
                Console.WriteLine("Démarrage du programme AutoPurge...");

                // Charger la configuration
                string configFilePath = @"C:\Users\User\source\repos\AutoPurge\AutoPurge\bin\Debug\config.json";

                ConfigService configService = new ConfigService(configFilePath);
                ConfigModel config = configService.LoadConfig();

                // Initialisation du logger
                ILogger logger = new Logger();
                string logFilePath = ((Logger)logger).LogFilePath;

                logger.LogInfo("Début de la purge des fichiers...");

                // Exécuter la purge
                FilePurger purger = new FilePurger(logger);
                foreach (var pathConfig in config.Paths)
                {
                    purger.PurgeFiles(pathConfig);
                }

                logger.LogInfo("Purge terminée avec succès.");

                // Attendre que le fichier log soit complètement écrit
                WaitForLogFile(logFilePath);

                // Envoyer l’email
                logger.LogInfo("Envoi de l'email récapitulatif...");
                EmailSender emailSender = new EmailSender(config.Email);
                emailSender.SendEmail(logFilePath);
                logger.LogInfo("Email envoyé avec succès.");

                MessageBox.Show("Processus terminé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
