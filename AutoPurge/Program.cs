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
            try
            {
                Console.WriteLine("🚀 Démarrage du programme AutoPurge...");

                // 1️⃣ Charger la configuration
                string configFilePath = @"C:\Users\saidm\source\repos\AutoPurge\AutoPurge\bin\Debug\config.json";
                ConfigService configService = new ConfigService(configFilePath);
                ConfigModel config = configService.LoadConfig();

                // 2️⃣ Initialisation du logger et création du fichier log
                ILogger logger = new Logger();
                string logFilePath = ((Logger)logger).LogFilePath; // ✅ Récupération du chemin du fichier log

                logger.LogInfo("📌 Début de la purge des fichiers...");

                // 3️⃣ Exécuter la purge des fichiers
                FilePurger purger = new FilePurger(logger);
                foreach (var pathConfig in config.Paths)
                {
                    purger.PurgeFiles(pathConfig);
                }

                logger.LogInfo("✅ Purge terminée avec succès.");

                // 4️⃣ Attendre que le fichier log soit complètement écrit avant l’envoi
                WaitForLogFile(logFilePath);

                // 5️⃣ Envoyer l’email seulement si le fichier log est bien disponible
                logger.LogInfo("📩 Envoi de l'email récapitulatif...");
                EmailSender emailSender = new EmailSender(config.Email);
                emailSender.SendEmail(logFilePath);
                logger.LogInfo("📨 Email envoyé avec succès.");

                Console.WriteLine("✅ Processus terminé avec succès.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Une erreur est survenue : {ex.Message}");
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
                    // Vérifie si le fichier est accessible en lecture
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        Console.WriteLine("✅ Fichier log prêt pour l'envoi.");
                        return;
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("⏳ Fichier log encore en cours d'écriture... Attente...");
                    Thread.Sleep(500); // Attend 500 ms avant de réessayer
                    attempts++;
                }
            }
            Console.WriteLine("⚠️ Attention : Le fichier log est peut-être encore en cours d'utilisation.");
        }

        /*
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigurationForm());
        }
        */
    }
}
