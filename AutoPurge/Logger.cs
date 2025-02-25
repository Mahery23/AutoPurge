using System;
using System.IO;
using System.Text;

namespace AutoPurge
{
    public class Logger : ILogger
    {
        private readonly string _logFilePath;

        public Logger()
        {
            try
            {
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                _logFilePath = Path.Combine(logDirectory, "app_log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création du dossier de log : " + ex.Message);
                throw;
            }
        }

        // ✅ Propriété publique pour récupérer le chemin du fichier log
        public string LogFilePath => _logFilePath;

        // 🔹 Utilisation de StreamWriter avec `using` pour libérer le fichier après écriture
public void LogInfo(string message)
{
    try
    {
        string logEntry = $"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} : {message}";
        
        using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
        {
            writer.WriteLine(logEntry);
        }

        Console.WriteLine("[INFO] " + message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erreur lors de l'écriture du log info : " + ex.Message);
    }
}

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
    }
}

    }
}
