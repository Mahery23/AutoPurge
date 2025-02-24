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
        /// Initialise une nouvelle instance de la classe ConfigService.
        /// </summary>
        /// <param name="configFilePath">Le chemin du fichier de configuration.</param>
        public ConfigService(string configFilePath)
        {
            _configFilePath = configFilePath ?? throw new ArgumentNullException(nameof(configFilePath));
        }

        /// <summary>
        /// Charge la configuration à partir du fichier JSON.
        /// </summary>
        /// <returns>Un objet ConfigModel contenant la configuration chargée.</returns>
        public ConfigModel LoadConfig()
        {
            try
            {
                // Vérifie si le fichier de configuration existe
                if (!File.Exists(_configFilePath))
                {
                    throw new FileNotFoundException($"Le fichier de configuration est introuvable : {_configFilePath}");
                }

                // Lit le contenu JSON du fichier
                string jsonContent = File.ReadAllText(_configFilePath);
                var config = JsonConvert.DeserializeObject<ConfigModel>(jsonContent);

                // Vérifie si la désérialisation a réussi
                if (config == null)
                {
                    throw new JsonException("Le fichier JSON est vide ou mal formaté.");
                }

                // Valide la configuration chargée
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
                // Valide la configuration avant la sauvegarde
                ValidateConfig(config);

                // Sérialise l'objet en JSON et l'écrit dans le fichier
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
        /// Valide la configuration pour s'assurer qu'elle est correcte.
        /// </summary>
        /// <param name="config">L'objet ConfigModel à valider.</param>
        private void ValidateConfig(ConfigModel config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // Vérifie si les informations email sont bien définies
            if (config.Email == null)
            {
                throw new InvalidOperationException("Les informations d'email sont manquantes dans la configuration.");
            }

            // Vérifie que les adresses email 'From' et 'To' ne sont pas vides
            if (string.IsNullOrWhiteSpace(config.Email.From) || string.IsNullOrWhiteSpace(config.Email.To))
            {
                throw new InvalidOperationException("Les adresses email 'From' et 'To' sont obligatoires.");
            }

            // Vérifie si au moins un chemin de purge est défini
            if (config.Paths == null || config.Paths.Count == 0)
            {
                throw new InvalidOperationException("Aucun chemin de purge n'est défini.");
            }

            // Vérifie chaque chemin de purge dans la configuration
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
        /// <returns>True si le fichier existe, sinon False.</returns>
        public bool ConfigFileExists()
        {
            return File.Exists(_configFilePath);
        }

        /// <summary>
        /// Crée un fichier de configuration par défaut si aucun n'existe.
        /// </summary>
        public void CreateDefaultConfigIfNotExists()
        {
            if (!ConfigFileExists())
            {
                // Création d'une configuration par défaut
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

                // Sauvegarde de la configuration par défaut
                SaveConfig(defaultConfig);
            }
        }
    }
}
