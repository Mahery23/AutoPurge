using System;
using System.IO;
using System.Text;

namespace AutoPurge
{
    /// <summary>
    /// Logger qui enregistre les messages dans un fichier de log et les affiche dans la console.
    /// </summary>
    public class Logger : ILogger
    {
        // Chemin complet du fichier de log
        private readonly string _logFilePath;

        /// <summary>
        /// Constructeur qui crée le dossier "Logs" (s'il n'existe pas) et génère un fichier de log unique.
        /// </summary>
        public Logger()
        {
            try
            {
                // Construit le chemin du dossier Logs dans le répertoire de l'application
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                // Génère un nom unique pour le fichier de log avec la date et l'heure
                _logFilePath = Path.Combine(logDirectory, "app_log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création du dossier de log : " + ex.Message);
                throw;
            }
        }

        // Propriété publique pour accéder au chemin du fichier log
        public string LogFilePath => _logFilePath;

        /// <summary>
        /// Enregistre un message d'information dans le fichier de log et l'affiche dans la console.
        /// </summary>
        public void LogInfo(string message)
        {
            try
            {
                // Prépare le message avec l'heure
                string logEntry = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} : {message}";
                // Utilise StreamWriter pour ajouter le message au fichier de log
                using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    writer.WriteLine(logEntry);
                }
                // Affiche le message dans la console
                Console.WriteLine("[INFO] " + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'écriture du log info : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Enregistre un message d'erreur dans le fichier de log et l'affiche dans la console.
        /// </summary>
        public void LogError(string message, Exception ex)
        {
            try
            {
                string logEntry = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} : {message} Exception: {ex.Message}";
                using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    writer.WriteLine(logEntry);
                }
                Console.WriteLine("[ERROR] " + message + " Exception: " + ex.Message);
            }
            catch (Exception loggingEx)
            {
                Console.WriteLine("Erreur lors de l'écriture du log erreur : " + loggingEx.Message);
                throw;
            }
        }
    }
}
