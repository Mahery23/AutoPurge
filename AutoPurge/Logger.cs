using System;
using System.IO;

namespace AutoPurge
{
    /// <summary>
    /// Logger qui enregistre les messages dans un fichier de log.
    /// </summary>
    public class Logger : ILogger  // Si tu as créé l'interface ILogger
    {
        private readonly string _logFilePath;

        /// <summary>
        /// Constructeur qui initialise le fichier de log.
        /// </summary>
        public Logger()
        {
            try
            {
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Le nom du fichier inclut la date et l'heure pour qu'il soit unique
                _logFilePath = Path.Combine(logDirectory, "app_log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création du dossier de log : " + ex.Message);
                throw;
            }
        }

        public void LogInfo(string message)
        {
            try
            {
                string logEntry = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} : {message}";
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                Console.WriteLine("[INFO] " + message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'écriture du log info : " + ex.Message);
                throw;
            }
        }

        public void LogError(string message, Exception ex)
        {
            try
            {
                string logEntry = $"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} : {message} Exception: {ex.Message}";
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
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
