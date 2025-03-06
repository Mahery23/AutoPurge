using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AutoPurge
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application AutoPurge.
        /// L'application peut être lancée en mode automatique (pour le Task Scheduler) ou en mode interactif via l'interface.
        /// </summary>
        [STAThread] // Requis pour certaines fonctionnalités Windows Forms
        static void Main(string[] args)
        {
            // Vérifie si l'application est lancée par le Task Scheduler (argument "auto")
            if (args.Length > 0 && args[0] == "auto")
            {
                LancerPurge(); // Exécute directement la purge
                return;        // Quitte l'application après exécution
            }

            // Sinon, lance l'interface utilisateur (ici, MainForm)
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Méthode qui exécute la purge des fichiers.
        /// Charge la configuration, lance la purge, attend que le log soit prêt et envoie un email récapitulatif.
        /// </summary>
        static void LancerPurge()
        {
            try
            {
                Console.WriteLine("Démarrage du programme AutoPurge...");

                // Charger la configuration en utilisant un chemin relatif
                // Application.StartupPath correspond au dossier contenant l'exécutable (que ce soit en Debug ou en Release)
                string configFilePath = Path.Combine(Application.StartupPath, "config.json");


                // Initialiser le service de configuration avec le chemin relatif
                ConfigService configService = new ConfigService(configFilePath);
                // Crée le fichier de configuration par défaut s'il n'existe pas
                configService.CreateDefaultConfigIfNotExists();
                // Charge la configuration depuis le fichier JSON
                ConfigModel config = configService.LoadConfig();

                // Initialisation du logger qui crée le fichier de log dans le dossier "Logs"
                ILogger logger = new Logger();
                string logFilePath = ((Logger)logger).LogFilePath; // Récupère le chemin du fichier de log

                logger.LogInfo("Début de la purge des fichiers...");

                // Exécuter la purge pour chaque chemin configuré
                FilePurger purger = new FilePurger(logger);
                foreach (var pathConfig in config.Paths)
                {
                    purger.PurgeFiles(pathConfig);
                }
                logger.LogInfo("Purge terminée avec succès.");

                // Attendre que le fichier de log soit complètement écrit et libéré
                WaitForLogFile(logFilePath);

                // Envoyer l'email récapitulatif avec le fichier de log en pièce jointe
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
        /// Attend que le fichier de log soit totalement écrit et accessible avant de poursuivre.
        /// </summary>
        /// <param name="filePath">Chemin du fichier log.</param>
        private static void WaitForLogFile(string filePath)
        {
            int attempts = 0;
            while (attempts < 10) // Essaye jusqu'à 10 fois (environ 5 secondes maximum)
            {
                try
                {
                    // Tente d'ouvrir le fichier en mode lecture exclusive pour vérifier s'il est libre
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        Console.WriteLine("Fichier log prêt pour l'envoi.");
                        return;
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("Fichier log encore en cours d'écriture... Attente...");
                    Thread.Sleep(500); // Attendre 500 ms avant de réessayer
                    attempts++;
                }
            }
            Console.WriteLine("Attention : Le fichier log est peut-être encore en cours d'utilisation.");
        }
    }
}
