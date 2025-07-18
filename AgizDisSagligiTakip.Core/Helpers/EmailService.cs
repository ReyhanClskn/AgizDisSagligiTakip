using System.Net;
using System.Net.Mail;

namespace AgizDisSagligiTakip.Core.Helpers
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _fromEmail = "*************@gmail.com"; 
        private readonly string _fromPassword = "*********"; //app password TODO: delete before commit

        public async Task<bool> KayitMailiGonderAsync(string toEmail, string kullaniciAdi)
        {
            try
            {
                var subject = "🦷 Ağız ve Diş Sağlığı Takip - Kayıt Başarılı";
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
                Console.WriteLine($"Email gönderme hatası: {ex.Message}");
                return false;
            }
        }

        private string KayitMailiHtmlOlustur(string kullaniciAdi)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Kayıt Başarılı</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; background-color: #f5f5f5; }}
        .container {{ max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #00244A 0%, #8A9DA6 100%); color: white; padding: 30px 20px; text-align: center; }}
        .content {{ padding: 30px 20px; text-align: center; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; background-color: #f8f9fa; }}
        .highlight {{ background-color: #e3f2fd; padding: 15px; border-radius: 5px; margin: 15px 0; display: inline-block; }}
        ul {{ background-color: #f8f9fa; padding: 20px; border-radius: 5px; text-align: center; display: inline-block; list-style-position: inside; }}
        li {{ margin-bottom: 8px; }}
        .emoji {{ font-size: 24px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='emoji'>🦷</div>
            <h1>Hoş Geldiniz!</h1>
            <p>Ağız ve Diş Sağlığı Takip Ailesine Katıldınız</p>
        </div>
        <div class='content'>
            <h2>Merhaba {kullaniciAdi},</h2>
            <p>Ağız ve Diş Sağlığı Takip uygulamasına başarıyla kayıt oldunuz! 🎉</p>
            
            <div class='highlight'>
                <strong>📅 Kayıt Tarihi:</strong> {DateTime.Now:dd.MM.yyyy HH:mm}
            </div>
            
            <p>Artık sağlıklı diş bakım alışkanlıklarınızı takip etmeye başlayabilirsiniz.</p>
            
            <h3>Neler Yapabilirsiniz:</h3>
            <ul>
                <li>🎯 Günlük diş bakım hedeflerinizi belirleyebilirsiniz.</li>
                <li>✅ Uyguladığınız bakım rutinlerini kaydedebilirsiniz.</li>
                <li>📸 Fotoğraflarla notlar ekleyebilirsiniz.</li>
                <li>💡 Kişiselleştirilmiş öneriler alabilirsiniz.</li>
                <li>📊 İlerlemenizi takip edebilirsiniz.</li>
            </ul>
            
            <p style='margin-top: 30px;'>
                <strong>Sağlıklı gülüşler dileriz! 😊</strong>
            </p>
        </div>
        <div class='footer'>
            <p>Bu mail otomatik olarak gönderilmiştir.</p>
            <p>© 2025 - Ağız ve Diş Sağlığı Takip</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}