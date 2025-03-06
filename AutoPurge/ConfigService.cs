using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Linq;

namespace AutoPurge
{
    /// <summary>
    /// Service de gestion de la configuration pour le programme de purge automatique.
    /// </summary>
    public class ConfigService
    {
        private readonly string _configFilePath;
        private readonly Regex _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled); // Regex pour validation email
        private readonly Regex _pathRegex = new Regex(@"^([a-zA-Z]:\\|\/).*", RegexOptions.Compiled);  // Regex pour chemin
        private readonly Regex _dateFormatRegex = new Regex(@"^[yMd\-\/]+$", RegexOptions.Compiled);  // Regex pour format de date
        private readonly Regex _extensionRegex = new Regex(@"^\.\w+$", RegexOptions.Compiled);  // Regex pour extension

        /// <summary>
        /// Constructeur qui initialise le service avec le chemin du fichier de configuration.
        /// </summary>
        /// <param name="configFilePath">Chemin vers le fichier JSON de configuration.</param>
        public ConfigService(string configFilePath)
        {
            // Si le chemin est null, on lance une exception.
            _configFilePath = configFilePath ?? throw new ArgumentNullException(nameof(configFilePath));
        }

        /// <summary>
        /// Charge la configuration à partir du fichier JSON.
        /// </summary>
        /// <returns>Un objet ConfigModel contenant la configuration.</returns>
        public ConfigModel LoadConfig()
        {
            try
            {
                // Vérifie si le fichier existe
                if (!File.Exists(_configFilePath))
                {
                    throw new FileNotFoundException($"Le fichier de configuration est introuvable : {_configFilePath}");
                }

                // Lit tout le contenu du fichier JSON
                string jsonContent = File.ReadAllText(_configFilePath);
                // Désérialise le contenu en objet ConfigModel
                var config = JsonConvert.DeserializeObject<ConfigModel>(jsonContent);

                if (config == null)
                {
                    throw new JsonException("Le fichier JSON est vide ou mal formaté.");
                }

                // Valide que la configuration est correcte
                ValidateConfig(config);
                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement de la configuration : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sauvegarde la configuration dans le fichier JSON.
        /// </summary>
        /// <param name="config">L'objet ConfigModel à sauvegarder.</param>
        public void SaveConfig(ConfigModel config)
        {
            try
            {
                // Vérifie que la configuration est correcte avant de sauvegarder
                ValidateConfig(config);

                // Sérialise l'objet en JSON avec une mise en forme indentée
                string jsonContent = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(_configFilePath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde de la configuration : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Valide la configuration pour s'assurer que toutes les données obligatoires sont présentes.
        /// </summary>
        /// <param name="config">L'objet ConfigModel à valider.</param>
        private void ValidateConfig(ConfigModel config)
        {
            // Vérifie si l'objet 'config' est null
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // Vérifie si la section Email est présente
            if (config.Email == null)
            {
                throw new InvalidOperationException("Les informations d'email sont manquantes.");
            }

            // Vérifie si les adresses email 'From' et 'To' sont valides (obligatoires)
            if (!_emailRegex.IsMatch(config.Email.From) || !_emailRegex.IsMatch(config.Email.To))
            {
                throw new InvalidOperationException("Les adresses email 'From' et 'To' doivent être valides.");
            }

            // Vérifie si l'adresse 'Cc' est valide (seulement si elle est renseignée)
            if (!string.IsNullOrWhiteSpace(config.Email.Cc) && !_emailRegex.IsMatch(config.Email.Cc))
            {
                throw new InvalidOperationException("L'adresse email 'Cc' est invalide.");
            }

            // Vérifie si l'adresse 'Bcc' est valide (seulement si elle est renseignée)
            if (!string.IsNullOrWhiteSpace(config.Email.Bcc) && !_emailRegex.IsMatch(config.Email.Bcc))
            {
                throw new InvalidOperationException("L'adresse email 'Bcc' est invalide.");
            }

            // Vérifie si la section 'Paths' existe et qu'elle contient au moins un élément
            if (config.Paths == null || config.Paths.Count == 0)
            {
                throw new InvalidOperationException("Aucun chemin de purge n'est défini.");
            }

            // Vérification de chaque chemin dans la liste 'Paths'
            foreach (var path in config.Paths)
            {
                // Vérifie si le chemin est vide ou ne correspond pas au format d'un chemin valide (obligatoire)
                if (string.IsNullOrWhiteSpace(path.Chemin))
                {
                    throw new InvalidOperationException("Veuillez sélectionner un chemin.");
                }
                else if (!_pathRegex.IsMatch(path.Chemin))
                {
                    throw new InvalidOperationException($"Le chemin '{path.Chemin}' est invalide.");
                }

                // Vérifie si la valeur 'JoursEnArriere' est absente ou invalide (optionnel)
                if (path.JoursEnArriere < 0 || path.JoursEnArriere > 600)
                {
                    throw new InvalidOperationException("Veuillez entrer une valeur valide pour 'JoursEnArriere' (entre 0 et 600).");
                }

                // Vérifie si le format de date est valide (obligatoire)
                if (!string.IsNullOrWhiteSpace(path.FormatDate) && path.FormatDate.Length > 20)
                {
                    throw new InvalidOperationException($"Le format de date pour le chemin {path.Chemin} dépasse la longueur maximale de 20 caractères.");
                }

                // Vérifie que chaque extension est valide et ne dépasse pas 10 caractères (optionnel)
                if (path.Extensions != null)
                {
                    foreach (var ext in path.Extensions)
                    {
                        if (!_extensionRegex.IsMatch(ext))
                        {
                            throw new InvalidOperationException($"L'extension '{ext}' est invalide.");
                        }
                        if (ext.Length > 10)
                        {
                            throw new InvalidOperationException($"L'extension '{ext}' est trop longue (max 10 caractères).");
                        }
                    }
                }

                // Vérifie que le nom ne dépasse pas 100 caractères (optionnel)
                if (path.Nom != null && path.Nom.Any(nom => nom.Length > 100))
                {
                    throw new InvalidOperationException($"Un nom dans la liste dépasse la longueur maximale de 100 caractères.");
                }

                // Vérifie que chaque exception ne dépasse pas 200 caractères (optionnel)
                if (path.Exceptions != null)
                {
                    foreach (var exception in path.Exceptions)
                    {
                        if (exception.Length > 200)
                        {
                            throw new InvalidOperationException($"L'exception '{exception}' est trop longue (max 200 caractères).");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Vérifie si le fichier de configuration existe.
        /// </summary>
        /// <returns>Vrai si le fichier existe, sinon Faux.</returns>
        public bool ConfigFileExists()
        {
            return File.Exists(_configFilePath);
        }

        /// <summary>
        /// Crée une configuration par défaut si aucun fichier n'existe.
        /// </summary>
        public void CreateDefaultConfigIfNotExists()
        {
            if (!ConfigFileExists())
            {
                // Création d'une configuration par défaut avec des valeurs minimales
                var defaultConfig = new ConfigModel
                {
                    Email = new EmailConfig
                    {
                        From = "sender@example.com",
                        To = "receiver@example.com",
                        Cc = "",
                        Bcc = "",
                        Password = ""
                    },
                    Paths = new List<PathConfig>
                    {
                        new PathConfig
                        {
                            Chemin = @"C:\Example\Path",
                            JoursEnArriere = 0,
                            FormatDate = "yyyyMMdd",
                            Extensions = new List<string> { ".txt", ".log" },
                            Nom = new List<string>(),
                            Exceptions = new List<string>()
                        }
                    }
                };

                // Sauvegarde de la configuration par défaut dans le fichier JSON
                SaveConfig(defaultConfig);
            }
        }
    }
}
