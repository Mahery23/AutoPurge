using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutoPurge
{
    /// <summary>
    /// Classe chargée de purger les fichiers d'un dossier
    /// en fonction d'une date limite (cutoffDate).
    /// </summary>
    public class FilePurger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Injecte une instance d'ILogger dans FilePurger pour la journalisation.
        /// </summary>
        /// <param name="logger">Instance de ILogger à utiliser pour les logs.</param>
        public FilePurger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parcourt le dossier spécifié et supprime les fichiers dont la date de création
        /// est antérieure à la date limite fournie.
        /// </summary>
        /// <param name="directoryPath">Chemin du dossier à purger.</param>
        /// <param name="cutoffDate">Date limite pour la suppression des fichiers.</param>
        public void PurgeFiles(string directoryPath, DateTime cutoffDate)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    throw new DirectoryNotFoundException("Le dossier spécifié n'existe pas : " + directoryPath);
                }

                string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);


                foreach (var file in files)
                {
                    try
                    {
                        DateTime creationTime = File.GetCreationTime(file);

                        if (creationTime < cutoffDate)
                        {
                            File.Delete(file);
                            _logger.LogInfo("Fichier supprimé : " + file);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Erreur lors de la suppression du fichier : " + file, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erreur lors de la purge dans le dossier : " + directoryPath, ex);
                throw;
            }
        }
    }
}
