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
        // Configuration des emails
        public EmailConfig Email { get; set; }
        // Liste des chemins à surveiller pour la suppression automatique
        public List<PathConfig> Paths { get; set; }
    }

    // Classe représentant la configuration de l'envoi des emails
    public class EmailConfig
    {
        public string From { get; set; }
        public string Password { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
    }

    // Classe représentant la configuration des chemins à surveiller
    public class PathConfig
    {
        public string Chemin { get; set; }
        public int JoursEnArriere { get; set; }
        public string FormatDate { get; set; }
        public List<string> Extensions { get; set; }
        public List<string> Nom { get; set; }
        public List<string> Exceptions { get; set; }
    }
}

