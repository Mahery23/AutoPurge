using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace AutoPurge
{
    public class EmailSender
    {
        private readonly EmailConfig _emailConfig;

        public EmailSender(EmailConfig emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(string logFilePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_emailConfig.From) ||
                    string.IsNullOrWhiteSpace(_emailConfig.To) ||
                    string.IsNullOrWhiteSpace(_emailConfig.Password))
                {
                    Console.WriteLine("⚠️ Configuration email invalide. Vérifiez le fichier JSON.");
                    return;
                }

                if (!File.Exists(logFilePath))
                {
                    Console.WriteLine("❌ Erreur : Le fichier log n'existe pas.");
                    return;
                }

                // Création de l'email
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

                // ✅ Attacher le fichier log
                Console.WriteLine($"📎 Attachement du fichier log : {logFilePath}");
                Attachment attachment = new Attachment(logFilePath);
                mail.Attachments.Add(attachment);

                // Configuration du serveur SMTP (AOL, Gmail, etc.)
                SmtpClient smtpClient = new SmtpClient("smtp.aol.com") // Remplace selon le service utilisé
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password),
                    EnableSsl = true
                };

                // ✅ Envoi de l'email
                smtpClient.Send(mail);
                Console.WriteLine("📩 Email envoyé avec succès avec le fichier log en pièce jointe !");

                // ✅ Fermer et libérer le fichier log après l'envoi
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
