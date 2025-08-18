using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

public class EncryptionManager
{
    private byte[] GetKeyBytes(string key) => System.Text.Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));

    private string[] GetFiles(string directory, string searchPattern, params string[] keywords) =>
        Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories)
                 .Where(f => keywords.Any(k => Path.GetFileName(f).Contains(k)))
                 .ToArray();

    public void EncryptSensitiveFiles(string directory, string key)
    {
        if (!Directory.Exists(directory)) throw new DirectoryNotFoundException("Directory not found.");

        byte[] keyBytes = GetKeyBytes(key);
        var files = GetFiles(directory, "*.*", "Confidential", "Secret");

        foreach (var file in files)
        {
            string tempFile = file + ".tmp";
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.GenerateIV();

                using (var fsOutput = new FileStream(tempFile, FileMode.Create))
                {
                    fsOutput.Write(aes.IV, 0, aes.IV.Length);
                    using (var fsInput = new FileStream(file, FileMode.Open))
                    using (var cs = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        fsInput.CopyTo(cs);
                }
            }
            File.Delete(file);
            File.Move(tempFile, file);
            Console.WriteLine($"Encrypted: {file}");
        }
    }

    public void DecryptSensitiveFiles(string directory, string key)
    {
        if (!Directory.Exists(directory)) throw new DirectoryNotFoundException("Directory not found.");

        byte[] keyBytes = GetKeyBytes(key);
        var files = GetFiles(directory, "*.*", "Confidential", "Secret");

        foreach (var file in files)
        {
            string tempFile = file + ".tmp";
            using (Aes aes = Aes.Create())
            {
                byte[] iv = new byte[16];
                using (var fsInput = new FileStream(file, FileMode.Open))
                {
                    fsInput.Read(iv, 0, iv.Length);
                    aes.Key = keyBytes;
                    aes.IV = iv;

                    using (var fsOutput = new FileStream(tempFile, FileMode.Create))
                    using (var cs = new CryptoStream(fsOutput, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        fsInput.CopyTo(cs);
                }
            }
            File.Delete(file);
            File.Move(tempFile, file);
            Console.WriteLine($"Decrypted: {file}");
        }
    }
}
