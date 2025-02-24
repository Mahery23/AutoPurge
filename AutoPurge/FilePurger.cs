using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoPurge
{
    /// <summary>
    /// Classe chargée de purger les fichiers d'un dossier selon la configuration dynamique.
    /// La configuration est passée via un objet PathConfig qui contient :
    /// - Le chemin du dossier à surveiller.
    /// - Le nombre de jours à considérer pour l'ancienneté (JoursEnArriere).
    /// - Le format de date (pour d'éventuelles comparaisons ou affichages).
    /// - Une liste d'extensions à traiter.
    /// - Des filtres sur le nom et les exceptions pour affiner la sélection.
    /// </summary>
    public class FilePurger
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Constructeur qui injecte une instance d'ILogger pour la journalisation.
        /// </summary>
        /// <param name="logger">Instance de ILogger utilisée pour loguer les actions et erreurs.</param>
        public FilePurger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parcourt le dossier spécifié dans la configuration et supprime les fichiers qui répondent aux critères :
        /// - Le fichier doit être plus ancien que la date limite calculée à partir de JoursEnArriere.
        /// - Son extension doit figurer dans la liste des extensions autorisées (si définie).
        /// - Si une liste de noms est fournie, le nom du fichier doit contenir au moins l'un de ces mots.
        /// - Si une liste d'exceptions est définie, le nom du fichier ne doit pas contenir l'un de ces éléments.
        /// Si aucun fichier n'est supprimé, un message approprié est logué : soit le dossier est vide, soit tous les fichiers sont récents.
        /// </summary>
        /// <param name="pathConfig">Configuration spécifique au chemin de purge.</param>
        public void PurgeFiles(PathConfig pathConfig)
        {
            try
            {
                // Vérifie que le dossier spécifié existe
                if (!Directory.Exists(pathConfig.Chemin))
                {
                    throw new DirectoryNotFoundException("Le dossier spécifié n'existe pas : " + pathConfig.Chemin);
                }

                // Récupère tous les fichiers du dossier et de ses sous-dossiers
                string[] files = Directory.GetFiles(pathConfig.Chemin, "*", SearchOption.AllDirectories);

                // Cas 1 : Le dossier est vide
                if (files.Length == 0)
                {
                    _logger.LogInfo("Le dossier " + pathConfig.Chemin + " est vide, aucun fichier à traiter.");
                    return;
                }

                // Calcule la date limite (cutoffDate) en soustrayant le nombre de jours définis (JoursEnArriere) à la date actuelle
                DateTime cutoffDate = DateTime.Now.AddDays(-pathConfig.JoursEnArriere);

                // Compteur pour suivre le nombre de fichiers supprimés
                int deletionCount = 0;

                // Parcourt chaque fichier du dossier
                foreach (var file in files)
                {
                    try
                    {
                        // Récupère la date de création du fichier
                        DateTime creationTime = File.GetCreationTime(file);

                        // Vérifie si le fichier est plus ancien que la date limite
                        if (creationTime < cutoffDate)
                        {
                            // Vérification du filtre par extension (si la liste des extensions est renseignée)
                            string fileExtension = Path.GetExtension(file);
                            if (pathConfig.Extensions != null && pathConfig.Extensions.Any())
                            {
                                if (!pathConfig.Extensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                                {
                                    continue; // L'extension ne correspond pas, on ignore ce fichier
                                }
                            }

                            // Vérification du filtre sur le nom (si la liste "Nom" est renseignée)
                            string fileName = Path.GetFileName(file);
                            if (pathConfig.Nom != null && pathConfig.Nom.Any())
                            {
                                // Vérifie que le nom du fichier contient au moins un des mots clés
                                bool nameMatch = pathConfig.Nom.Any(nom => fileName.IndexOf(nom, StringComparison.OrdinalIgnoreCase) >= 0);
                                if (!nameMatch)
                                {
                                    continue; // Aucun mot clé trouvé, on ignore ce fichier
                                }
                            }

                            // Vérification des exceptions (si la liste "Exceptions" est renseignée)
                            if (pathConfig.Exceptions != null && pathConfig.Exceptions.Any())
                            {
                                // Si le nom du fichier contient un des termes à exclure, on ignore ce fichier
                                bool exceptionFound = pathConfig.Exceptions.Any(exc => fileName.IndexOf(exc, StringComparison.OrdinalIgnoreCase) >= 0);
                                if (exceptionFound)
                                {
                                    continue;
                                }
                            }

                            // Tous les critères sont satisfaits : on tente de supprimer le fichier
                            File.Delete(file);
                            deletionCount++; // Incrémente le compteur de suppression
                            _logger.LogInfo("Fichier supprimé : " + file);
                        }
                    }
                    catch (Exception ex)
                    {
                        // En cas d'erreur sur un fichier spécifique, logue l'erreur et continue avec le fichier suivant
                        _logger.LogError("Erreur lors de la suppression du fichier : " + file, ex);
                    }
                }

                // Si aucun fichier n'a été supprimé, logue un message approprié
                if (deletionCount == 0)
                {
                    _logger.LogInfo("Aucun fichier n'a été supprimé dans le dossier " + pathConfig.Chemin + " : tous les fichiers sont récents.");
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur globale, logue l'erreur et relance l'exception
                _logger.LogError("Erreur lors de la purge dans le dossier : " + pathConfig.Chemin, ex);
                throw;
            }
        }
    }
}
