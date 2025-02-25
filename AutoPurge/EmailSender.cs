using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace AutoPurge
{
    /// <summary>
    /// Classe qui gère l'envoi d'un email avec le fichier de log en pièce jointe.
    /// </summary>
    public class EmailSender
    {
        // Contient la configuration email (expéditeur, mot de passe, destinataire, etc.)
        private readonly EmailConfig _emailConfig;

        /// <summary>
        /// Constructeur qui reçoit la configuration email.
        /// </summary>
        /// <param name="emailConfig">L'objet EmailConfig avec les paramètres d'envoi.</param>
        public EmailSender(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        /// <summary>
        /// Envoie un email avec le fichier de log en pièce jointe.
        /// </summary>
        /// <param name="logFilePath">Chemin du fichier de log à joindre.</param>
        public void SendEmail(string logFilePath)
        {
            try
            {
                // Vérifie que les informations d'email essentielles sont présentes
                if (string.IsNullOrWhiteSpace(_emailConfig.From) ||
                    string.IsNullOrWhiteSpace(_emailConfig.To) ||
                    string.IsNullOrWhiteSpace(_emailConfig.Password))
                {
                    Console.WriteLine("⚠️ Configuration email invalide. Vérifiez le fichier JSON.");
                    return;
                }

                // Vérifie que le fichier de log existe
                if (!File.Exists(logFilePath))
                {
                    Console.WriteLine("❌ Erreur : Le fichier log n'existe pas.");
                    return;
                }

                // Création du message email
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_emailConfig.From),
                    Subject = "📌 Rapport de purge des fichiers",
                    Body = $"Bonjour,\n\nCi-joint le rapport de purge des fichiers.\n\nCordialement,\nAutoPurge",
                    IsBodyHtml = false
                };

                mail.To.Add(_emailConfig.To);
                if (!string.IsNullOrWhiteSpace(_emailConfig.Cc)) mail.CC.Add(_emailConfig.Cc);
                if (!string.IsNullOrWhiteSpace(_emailConfig.Bcc)) mail.Bcc.Add(_emailConfig.Bcc);

                // Ajoute le fichier de log en pièce jointe
                Console.WriteLine($"📎 Attachement du fichier log : {logFilePath}");
                Attachment attachment = new Attachment(logFilePath);
                mail.Attachments.Add(attachment);

                // Configuration du serveur SMTP (ici configuré pour AOL, à adapter selon votre service)
                SmtpClient smtpClient = new SmtpClient("smtp.aol.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password),
                    EnableSsl = true
                };

                // Envoi de l'email
                smtpClient.Send(mail);
                Console.WriteLine("📩 Email envoyé avec succès avec le fichier log en pièce jointe !");

                // Libère les ressources (attachement, message, client SMTP)
                attachment.Dispose();
                mail.Dispose();
                smtpClient.Dispose();

                Console.WriteLine("🔓 Fichier log libéré après l'envoi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur lors de l'envoi de l'email : {ex.Message}");
            }
        }
    }
}
