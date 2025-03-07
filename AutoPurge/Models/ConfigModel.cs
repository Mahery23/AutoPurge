using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPurge
{
    // Modèle principal de configuration
    public class ConfigModel
    {
        // Contient les informations d'email
        public EmailConfig Email { get; set; }
        // Liste des chemins à surveiller pour la suppression automatique des fichiers
        public List<PathConfig> Paths { get; set; }
    }

    // Classe représentant la configuration pour l'envoi des emails
    public class EmailConfig
    {
        public string From { get; set; }      // Adresse de l'expéditeur
        public string Password { get; set; }  // Mot de passe de l'expéditeur
        public string To { get; set; }        // Adresse du destinataire
        public string Cc { get; set; }        // Adresse(s) en copie (facultatif)
        public string Bcc { get; set; }       // Adresse(s) en copie cachée (facultatif)
    }

    // Classe représentant la configuration d'un chemin à surveiller pour la purge
    public class PathConfig
    {
        public string Chemin { get; set; }         // Le dossier à surveiller
        public int JoursEnArriere { get; set; }     // Nombre de jours pour déterminer l'ancienneté d'un fichier
        public string FormatDate { get; set; }       // Format de la date (pour comparaison ou affichage)
        public List<string> Extensions { get; set; } // Liste des extensions à traiter (ex: ".txt", ".log")
        public List<string> Nom { get; set; }        // Mots-clés à rechercher dans le nom du fichier
        public List<string> Exceptions { get; set; } // Termes d'exclusion dans le nom du fichier                                                    
        public bool SupprimerDossier { get; set; }  // Nouvelle propriété : si vrai, supprime le dossier (et sous-dossiers vides) après la purge
    }
}
