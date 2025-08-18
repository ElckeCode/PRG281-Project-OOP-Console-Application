using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SFO_Class_Divided

{
    // Handles AES encryption/decryption
    public class EncryptionManager
    {
        public void EncryptFile(string filePath, string key)
        {
            try
            {
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.GenerateIV();
                    using (FileStream fsOutput = new FileStream(filePath + ".enc", FileMode.Create))
                    {
                        fsOutput.Write(aes.IV, 0, aes.IV.Length);
                        using (FileStream fsInput = new FileStream(filePath, FileMode.Open))
                        using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            fsInput.CopyTo(cs);
                        }
                    }
                }
                Console.WriteLine($"Encrypted {filePath} to {filePath}.enc");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error encrypting file: {ex.Message}");
            }
        }

        public void DecryptFile(string filePath, string key)
        {
            try
            {
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                using (Aes aes = Aes.Create())
                {
                    byte[] iv = new byte[16];
                    using (FileStream fsInput = new FileStream(filePath, FileMode.Open))
                    {
                        fsInput.Read(iv, 0, iv.Length);
                        aes.Key = keyBytes;
                        aes.IV = iv;
                        using (FileStream fsOutput = new FileStream(filePath + ".dec", FileMode.Create))
                        using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            fsInput.CopyTo(cs);
                        }
                    }
                }
                Console.WriteLine($"Decrypted {filePath} to {filePath}.dec");
            }
            catch (Exception ex)
            {
                throw new SensitiveFileException($"Error decrypting file: {ex.Message}");
            }
        }
    }
}
