using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPurge
{
    public interface ILogger
    {
        /// <summary>
        /// Log un message d'information.
        /// </summary>
        /// <param name="message">Message à enregistrer.</param>
        void LogInfo(string message);

        /// <summary>
        /// Log un message d'erreur avec l'exception associée.
        /// </summary>
        /// <param name="message">Message décrivant l'erreur.</param>
        /// <param name="ex">Exception associée à l'erreur.</param>
        void LogError(string message, Exception ex);
    }
}
