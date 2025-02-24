using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoPurge
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Instanciation du Logger (via l'interface ILogger)
            ILogger logger = new Logger();

            // 2. Création de l'objet FilePurger en injectant le logger
            FilePurger purger = new FilePurger(logger);

            // 3. Définir le chemin du dossier à purger.
            // Remplacez "C:\Temp" par le chemin correspondant à vos besoins
            string folderPath = @"C:\Users\steav\Downloads\Compressed";

            // 4. Définir la date limite pour la suppression.
            // Ici, on supprime les fichiers créés il y a plus d'un an.
            DateTime cutoffDate = DateTime.Now.AddYears(-1);

            try
            {
                // 5. Appel de la méthode PurgeFiles pour purger le dossier
                purger.PurgeFiles(folderPath, cutoffDate);
                Console.WriteLine("Purge terminée avec succès.");
            }
            catch (Exception ex)
            {
                // En cas d'erreur globale, affichage du message d'erreur
                Console.WriteLine("La purge a échoué : " + ex.Message);
            }

            // 6. Attente de l'utilisateur pour visualiser les résultats dans la console
            Console.WriteLine("Appuyez sur une touche pour fermer...");
            Console.ReadKey();
        }
    }
}
