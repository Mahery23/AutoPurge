using System;

namespace AutoPurge
{
    /// <summary>
    /// Interface définissant les méthodes de journalisation.
    /// Toute classe qui implémente ILogger doit fournir LogInfo et LogError.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Enregistre un message d'information.
        /// </summary>
        /// <param name="message">Message à loguer.</param>
        void LogInfo(string message);

        /// <summary>
        /// Enregistre un message d'erreur avec l'exception associée.
        /// </summary>
        /// <param name="message">Description de l'erreur.</param>
        /// <param name="ex">Exception associée à l'erreur.</param>
        void LogError(string message, Exception ex);
    }
}
