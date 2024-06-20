using System.Security.Cryptography;
using System.Text;

namespace Shared;

public class EncryptionHelper
{
    private readonly byte[] key;
    private readonly byte[] iv;

    public EncryptionHelper(string key, string iv)
    {
        if (key == null || iv == null)
            throw new ArgumentNullException(nameof(key), "Key and IV must not be null");
        if (key.Length != 32 || iv.Length != 16)
            throw new ArgumentException("Invalid key or IV length. Key must be 32 bytes and IV must be 16 bytes.");

        this.key = Encoding.UTF8.GetBytes(key);
        this.iv = Encoding.UTF8.GetBytes(iv);
    }

    public string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}