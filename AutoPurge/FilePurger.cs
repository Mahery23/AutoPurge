    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoPurge
{
    /// <summary>
    /// Classe chargée de purger les fichiers d'un dossier selon la configuration.
    /// </summary>
    public class FilePurger
    {
        // Instance de ILogger pour enregistrer les actions et erreurs
        private readonly ILogger _logger;

        /// <summary>
        /// Constructeur qui reçoit un logger.
        /// </summary>
        /// <param name="logger">Logger utilisé pour consigner les opérations.</param>
        public FilePurger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parcourt le dossier spécifié et supprime les fichiers répondant aux critères :
        /// - Fichier plus ancien que la date limite calculée à partir de JoursEnArriere.
        /// - Extension autorisée, et respecte les filtres de nom et d'exceptions.
        /// Si aucun fichier n'est supprimé, un message approprié est logué.
        /// </summary>
        /// <param name="pathConfig">Configuration du chemin à traiter.</param>
        public void PurgeFiles(PathConfig pathConfig)
        {
            try
            {
                // Vérifie que le dossier existe
                if (!Directory.Exists(pathConfig.Chemin))
                {
                    throw new DirectoryNotFoundException("Le dossier spécifié n'existe pas : " + pathConfig.Chemin);
                }

                // Récupère tous les fichiers du dossier et de ses sous-dossiers
                string[] files = Directory.GetFiles(pathConfig.Chemin, "*", SearchOption.AllDirectories);

                // Si le dossier est vide, logue un message et quitte la méthode
                if (files.Length == 0)
                {
                    _logger.LogInfo("Le dossier " + pathConfig.Chemin + " est vide, aucun fichier à traiter.");
                    return;
                }

                // Calcule la date limite en soustrayant JoursEnArriere à la date actuelle
                DateTime cutoffDate = DateTime.Now.AddDays(-pathConfig.JoursEnArriere);
                int deletionCount = 0; // Compteur de fichiers supprimés

                // Traite chaque fichier du dossier
                foreach (var file in files)
                {
                    try
                    {
                        // Récupère la date de création du fichier
                        DateTime creationTime = File.GetCreationTime(file);

                        // Vérifie si le fichier est plus ancien que la date limite
                        if (creationTime < cutoffDate)
                        {
                            // Filtre par extension (si défini)
                            string fileExtension = Path.GetExtension(file);
                            if (pathConfig.Extensions != null && pathConfig.Extensions.Any())
                            {
                                if (!pathConfig.Extensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                                {
                                    continue;
                                }
                            }

                            // Filtre par nom (si défini)
                            string fileName = Path.GetFileName(file);
                            if (pathConfig.Nom != null && pathConfig.Nom.Any())
                            {
                                bool nameMatch = pathConfig.Nom.Any(nom => fileName.IndexOf(nom, StringComparison.OrdinalIgnoreCase) >= 0);
                                if (!nameMatch)
                                {
                                    continue;
                                }
                            }

                            // Filtre par exceptions (si défini)
                            if (pathConfig.Exceptions != null && pathConfig.Exceptions.Any())
                            {
                                bool exceptionFound = pathConfig.Exceptions.Any(exc => fileName.IndexOf(exc, StringComparison.OrdinalIgnoreCase) >= 0);
                                if (exceptionFound)
                                {
                                    continue;
                                }
                            }

                            // Si tous les critères sont respectés, supprime le fichier
                            File.Delete(file);
                            deletionCount++;
                            _logger.LogInfo("Fichier supprimé : " + file);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Logue l'erreur sur ce fichier et continue
                        _logger.LogError("Erreur lors de la suppression du fichier : " + file, ex);
                    }
                }

                // Si aucun fichier n'a été supprimé, logue un message indiquant que tous les fichiers sont récents
                if (deletionCount == 0)
                {
                    _logger.LogInfo("Aucun fichier n'a été supprimé dans le dossier " + pathConfig.Chemin + " : tous les fichiers sont récents.");
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur globale, logue et relance l'exception
                _logger.LogError("Erreur lors de la purge dans le dossier : " + pathConfig.Chemin, ex);
                throw;
            }
        }
    }
}
