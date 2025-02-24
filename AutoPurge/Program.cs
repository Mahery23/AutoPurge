using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPurge
{
    static class Program
    {
        [STAThread] // Important pour éviter l'erreur

        static void Main(string[] args)
        {
            // Chemin vers le fichier JSON de configuration.
            // Veuillez adapter ce chemin à l'emplacement réel de votre fichier config.json.
            string configFilePath = @"C:\Users\steav\source\repos\AutoPurge\AutoPurge\bin\Debug\config.json";

            // Création d'une instance du service de configuration en passant le chemin du fichier
            ConfigService configService = new ConfigService(configFilePath);

            // Chargement de la configuration depuis le fichier JSON dans un objet ConfigModel
            ConfigModel config = configService.LoadConfig();

            // Instanciation du Logger pour la journalisation (écrit dans un fichier et affiche sur la console)
            ILogger logger = new Logger();

            // Instanciation de FilePurger en lui injectant l'instance du Logger
            FilePurger purger = new FilePurger(logger);

            // Pour chaque configuration de chemin dans le fichier JSON, lancer la purge
            foreach (var pathConfig in config.Paths)
            {
                // La méthode PurgeFiles attend un objet PathConfig qui contient tous les critères
                purger.PurgeFiles(pathConfig);
            }

            Console.WriteLine("Purge terminée. Appuyez sur une touche pour fermer...");
            Console.ReadKey();
        }


        /*
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigurationForm());
        }*/
    }
}
