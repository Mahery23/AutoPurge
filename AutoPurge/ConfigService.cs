using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AutoPurge
{
    /// <summary>
    /// Service de gestion de la configuration pour le programme de purge automatique.
    /// </summary>
    public class ConfigService
    {
        private readonly string _configFilePath;

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
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.Email == null)
            {
                throw new InvalidOperationException("Les informations d'email sont manquantes dans la configuration.");
            }

            if (string.IsNullOrWhiteSpace(config.Email.From) || string.IsNullOrWhiteSpace(config.Email.To))
            {
                throw new InvalidOperationException("Les adresses email 'From' et 'To' sont obligatoires.");
            }

            if (config.Paths == null || config.Paths.Count == 0)
            {
                throw new InvalidOperationException("Aucun chemin de purge n'est défini.");
            }

            foreach (var path in config.Paths)
            {
                if (string.IsNullOrWhiteSpace(path.Chemin))
                {
                    throw new InvalidOperationException("Un chemin de purge est vide ou non défini.");
                }
                if (path.JoursEnArriere < 0)
                {
                    throw new InvalidOperationException($"La valeur 'JoursEnArriere' pour le chemin {path.Chemin} doit être supérieure ou égale à 0.");
                }
                if (string.IsNullOrWhiteSpace(path.FormatDate))
                {
                    throw new InvalidOperationException($"Le champ 'FormatDate' est vide pour le chemin {path.Chemin}.");
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
