using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoPurge
{
    /// <summary>
    /// Classe chargée de purger les fichiers d'un dossier selon la configuration.
    /// Elle supprime aussi les sous-dossiers devenus vides, 
    /// mais ne supprime pas le dossier parent s'il contient des fichiers hors extension.
    /// </summary>
    public class FilePurger
    {
        private readonly ILogger _logger;

        public FilePurger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parcourt le dossier spécifié et supprime les fichiers qui répondent aux critères. 
        /// Ensuite, supprime récursivement les sous-dossiers vides. 
        /// Enfin, vérifie si le dossier parent est vide pour éventuellement le supprimer.
        /// </summary>
        /// <param name="pathConfig">Configuration de purge (chemin, ancienneté, extensions, etc.).</param>
        public void PurgeFiles(PathConfig pathConfig)
        {
            try
            {
                // Vérifie que le dossier existe
                if (!Directory.Exists(pathConfig.Chemin))
                {
                    throw new DirectoryNotFoundException("Le dossier spécifié n'existe pas : " + pathConfig.Chemin);
                }

                // Récupère tous les fichiers du dossier et sous-dossiers
                string[] files = Directory.GetFiles(pathConfig.Chemin, "*", SearchOption.AllDirectories);

                // Si le dossier contient des fichiers, on tente de les supprimer selon les critères
                if (files.Length > 0)
                {
                    DateTime cutoffDate = DateTime.Now.AddDays(-pathConfig.JoursEnArriere);
                    int deletionCount = 0;

                    foreach (var file in files)
                    {
                        try
                        {
                            // Récupère la date de création du fichier
                            DateTime creationTime = File.GetCreationTime(file);

                            // Vérifie l'ancienneté
                            if (creationTime < cutoffDate)
                            {
                                // Vérifie l'extension si définie
                                string fileExtension = Path.GetExtension(file);
                                if (pathConfig.Extensions != null && pathConfig.Extensions.Any() &&
                                    !pathConfig.Extensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                                {
                                    continue; // Fichier hors extension ciblée
                                }

                                // Vérifie les mots-clés dans le nom (si définis)
                                string fileName = Path.GetFileName(file);
                                if (pathConfig.Nom != null && pathConfig.Nom.Any())
                                {
                                    bool nameMatch = pathConfig.Nom.Any(nom =>
                                        fileName.IndexOf(nom, StringComparison.OrdinalIgnoreCase) >= 0);
                                    if (!nameMatch)
                                    {
                                        continue; // Nom hors critères
                                    }
                                }

                                // Vérifie les exceptions (si définies)
                                if (pathConfig.Exceptions != null && pathConfig.Exceptions.Any())
                                {
                                    bool exceptionFound = pathConfig.Exceptions.Any(exc =>
                                        fileName.IndexOf(exc, StringComparison.OrdinalIgnoreCase) >= 0);
                                    if (exceptionFound)
                                    {
                                        continue; // Nom contient un terme d'exclusion
                                    }
                                }

                                // Tous les critères sont satisfaits, on supprime le fichier
                                File.Delete(file);
                                deletionCount++;
                                _logger.LogInfo("Fichier supprimé : " + file);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Logue l'erreur pour ce fichier, sans interrompre la boucle
                            _logger.LogError("Erreur lors de la suppression du fichier : " + file, ex);
                        }
                    }

                    // Si aucun fichier n'a été supprimé
                    if (deletionCount == 0)
                    {
                        _logger.LogInfo("Aucun fichier n'a été supprimé dans le dossier " + pathConfig.Chemin
                                        + " : tous les fichiers sont récents ou hors extension.");
                    }
                }
                else
                {
                    // Le dossier est vide de fichiers
                    _logger.LogInfo("Le dossier " + pathConfig.Chemin + " est vide, aucun fichier à traiter.");
                }

                // Suppression récursive des sous-dossiers vides
                // Cette méthode va parcourir tous les sous-dossiers et les supprimer s'ils sont totalement vides
                DeleteEmptySubdirectories(pathConfig.Chemin);

                // Enfin, on vérifie si le dossier parent est devenu vide
                if (IsDirectoryEmpty(pathConfig.Chemin))
                {
                    // Si le dossier est vide, on le supprime
                    Directory.Delete(pathConfig.Chemin, true);
                    _logger.LogInfo("Dossier parent supprimé (il était vide) : " + pathConfig.Chemin);
                }
                else
                {
                    _logger.LogInfo("Dossier parent non supprimé : " + pathConfig.Chemin
                                    + " contient encore des fichiers (peut-être hors extension).");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Erreur lors de la purge dans le dossier : " + pathConfig.Chemin, ex);
                // Ne pas relancer l'exception pour continuer le processus (ex: envoi d'email)
            }
        }

        /// <summary>
        /// Parcourt récursivement les sous-dossiers et supprime ceux qui sont totalement vides.
        /// </summary>
        /// <param name="directory">Le dossier racine à traiter.</param>
        private void DeleteEmptySubdirectories(string directory)
        {
            // Parcours récursif de chaque sous-dossier
            foreach (var subDir in Directory.GetDirectories(directory))
            {
                // On traite d'abord les sous-dossiers de subDir
                DeleteEmptySubdirectories(subDir);

                // Puis on vérifie si subDir est vide (aucun fichier, aucun sous-dossier)
                if (IsDirectoryEmpty(subDir))
                {
                    try
                    {
                        Directory.Delete(subDir, true);
                        _logger.LogInfo("Sous-dossier supprimé (il était vide) : " + subDir);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Erreur lors de la suppression du sous-dossier vide : " + subDir, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Vérifie si un dossier est totalement vide (aucun fichier et aucun sous-dossier).
        /// </summary>
        /// <param name="path">Le chemin du dossier à vérifier.</param>
        /// <returns>True si le dossier ne contient aucun fichier ni sous-dossier, sinon False.</returns>
        private bool IsDirectoryEmpty(string path)
        {
            try
            {
                // S'il y a au moins un fichier ou un sous-dossier, le dossier n'est pas vide
                if (Directory.GetFiles(path).Any() || Directory.GetDirectories(path).Any())
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                // En cas d'accès refusé ou autre erreur, on considère que le dossier n'est pas vide
                // (ou on peut loguer et renvoyer false)
                _logger.LogError("Erreur lors de la vérification du dossier : " + path, ex);
                return false;
            }
        }
    }
}
