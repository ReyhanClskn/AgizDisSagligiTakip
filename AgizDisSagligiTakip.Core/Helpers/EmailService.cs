using System.Net;
using System.Net.Mail;

namespace AgizDisSagligiTakip.Core.Helpers
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _fromPassword;

        public EmailService()
        {
            // Geliştirme aşaması için Gmail SMTP TODO:
            _smtpServer = "smtp.gmail.com";
            _smtpPort = 587;
            _fromEmail = "agizdistakip@gmail.com"; // Test email çalışmıyor
            _fromPassword = "test123"; // App password gerekiyor TODO:
        }

        public async Task<bool> KayitMailiGonderAsync(string toEmail, string kullaniciAdi)
        {
            try
            {
                var subject = "Ağız ve Diş Sağlığı Takip - Kayıt Başarılı";
                var htmlBody = KayitMailiHtmlOlustur(kullaniciAdi);

                using var smtpClient = new SmtpClient(_smtpServer, _smtpPort);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_fromEmail, _fromPassword);

                using var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, "Ağız ve Diş Sağlığı Takip");
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = htmlBody;
                message.IsBodyHtml = true;

                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                //İlerde loglama TODO:
                Console.WriteLine($"Email gönderme hatası: {ex.Message}");
                return false;
            }
        }

        private string KayitMailiHtmlOlustur(string kullaniciAdi)
        {
            //TODO: Örnek HTML 
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Kayıt Başarılı</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f8f9fa; }}
        .footer {{ text-align: center; padding: 10px; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Hoş Geldiniz!</h1>
        </div>
        <div class='content'>
            <h2>Merhaba {kullaniciAdi},</h2>
            <p>Ağız ve Diş Sağlığı Takip uygulamasına başarıyla kayıt oldunuz!</p>
            <p><strong>Kayıt Tarihi:</strong> {DateTime.Now:dd.MM.yyyy HH:mm}</p>
            <p>Artık ağız ve diş sağlığınızı takip etmeye başlayabilirsiniz.</p>
            <p>Uygulamamızda:</p>
            <ul>
                <li>Günlük diş bakım hedeflerinizi belirleyebilirsiniz</li>
                <li>Uyguladığınız bakım rutinlerini kaydedebilirsiniz</li>
                <li>Fotoğraflarla notlar ekleyebilirsiniz</li>
                <li>Kişiselleştirilmiş öneriler alabilirsiniz</li>
            </ul>
            <p>Sağlıklı günler dileriz!</p>
        </div>
        <div class='footer'>
            <p>Bu mail otomatik olarak gönderilmiştir.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}