using System.Security.Cryptography;
using System.Text;

namespace AgizDisSagligiTakip.Core.Helpers
{
    public class SifreleyiciService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public SifreleyiciService()
        {
            // 32 byte 256 bit anahtar
            _key = Encoding.UTF8.GetBytes("AgizDisSagligiTakip2024!12345678");
            // 16 byte 128 bit IV
            _iv = Encoding.UTF8.GetBytes("1234567890123456");
        }

        public string Sifrele(string metin)
        {
            if (string.IsNullOrEmpty(metin))
                return string.Empty;

            try
            {
                //AES notlarına bak TODO:
                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    var metinBytes = Encoding.UTF8.GetBytes(metin);

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        var sifreliBytes = encryptor.TransformFinalBlock(metinBytes, 0, metinBytes.Length);
                        return Convert.ToBase64String(sifreliBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Şifreleme hatası: {ex.Message}");
                return string.Empty;
            }
        }

        public string SifreCoz(string sifreliMetin)
        {
            if (string.IsNullOrEmpty(sifreliMetin))
                return string.Empty;

            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    var sifreliBytes = Convert.FromBase64String(sifreliMetin);
                    
                    using (var decryptor = aes.CreateDecryptor())
                    {
                        var metinBytes = decryptor.TransformFinalBlock(sifreliBytes, 0, sifreliBytes.Length);
                        return Encoding.UTF8.GetString(metinBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Şifre çözme hatası: {ex.Message}");
                return string.Empty;
            }
        }

        public bool SifreKontrolEt(string girilenSifre, string veritabanindakiSifre)
        {
            if (string.IsNullOrEmpty(girilenSifre) || string.IsNullOrEmpty(veritabanindakiSifre))
                return false;

            var sifreliGirilenSifre = Sifrele(girilenSifre);
            return sifreliGirilenSifre == veritabanindakiSifre;
        }
    }
}