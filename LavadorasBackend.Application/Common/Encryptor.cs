using System.Security.Cryptography;
using System.Text;

namespace Lavadoras.Application.Common;

public static class Encryptor
{
    private static readonly Encoding encoding = Encoding.UTF8;
    public static string Sha256Hash(string text)
    {
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    private static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string Aes256Encrypt(string message)
    {
        var key = "6Le0DgMTAAAAANokdEEial";
        var iv = "mHGFxENnZLbienLyANoi.e";
        var hashedKey = Sha256Hash(key);
        var keyBytes = Encoding.ASCII.GetBytes(hashedKey);

        var ivSha256 = Sha256Hash(iv);
        var ivBytes = Encoding.ASCII.GetBytes(ivSha256.Substring(0, 16));

        var encrypted = EncryptString(message, keyBytes, ivBytes);

        return iv + Base64Encode(encrypted);
    }

    public static string Aes256Decrypt(string cipherText)
    {
        var key = "6Le0DgMTAAAAANokdEEial";
        var iv = "mHGFxENnZLbienLyANoi.e";
        var hashedKey = Sha256Hash(key);
        var keyBytes = Encoding.ASCII.GetBytes(hashedKey);

        var ivSha256 = Sha256Hash(iv);
        var ivBytes = Encoding.ASCII.GetBytes(ivSha256.Substring(0, 16));

        var text = cipherText.Substring(16, cipherText.Length - 16);

        var decodedText = Base64Decode(text);

        return DecryptString(decodedText, keyBytes, ivBytes);
    }
    static byte[] HmacSHA256(String data, String key)
    {
        using (HMACSHA256 hmac = new HMACSHA256(encoding.GetBytes(key)))
        {
            return hmac.ComputeHash(encoding.GetBytes(data));
        }
    }
    public static string EncryptAES(string plainText)
    {
        try
        {
            byte[] encrypted;
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = Encoding.UTF8.GetBytes("8080808080808080");
                rijAlg.IV = Encoding.UTF8.GetBytes("8080808080808080");

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }
        catch (Exception e)
        {
            throw new Exception("Error encrypting: " + e.Message);
        }
    }

    public static string DecryptAES(string cipherText)
    {
        string plaintext = null;

        using (var rijAlg = new RijndaelManaged())
        {
            //Settings  
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Padding = PaddingMode.PKCS7;
            rijAlg.FeedbackSize = 128;

            rijAlg.Key = Encoding.UTF8.GetBytes("8080808080808080");
            rijAlg.IV = Encoding.UTF8.GetBytes("8080808080808080");

            // Create a decrytor to perform the stream transform.  
            var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            try
            {
                // Create the streams used for decryption.  
                using (var msDecrypt = new MemoryStream(Encoding.UTF8.GetBytes(cipherText)))
                {
                    using(var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {

                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream  
                            // and place them in a string.  
                            plaintext = srDecrypt.ReadToEnd();
                            return plaintext;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error decrypting: " + e.Message);
            }
        }
    }

    private static string EncryptString(string plainText, byte[] key, byte[] iv)
    {
        // Instantiate a new Aes object to perform string symmetric encryption
        Aes encryptor = Aes.Create();

        encryptor.Mode = CipherMode.CBC;
        //encryptor.KeySize = 256;
        //encryptor.BlockSize = 128;
        encryptor.Padding = PaddingMode.PKCS7;

        // Set key and IV
        encryptor.Key = key.Take(32).ToArray();
        encryptor.IV = iv;

        // Instantiate a new MemoryStream object to contain the encrypted bytes
        MemoryStream memoryStream = new MemoryStream();

        // Instantiate a new encryptor from our Aes object
        ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

        // Instantiate a new CryptoStream object to process the data and write it to the 
        // memory stream
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

        // Convert the plainText string into a byte array
        byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

        // Encrypt the input plaintext string
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);

        // Complete the encryption process
        cryptoStream.FlushFinalBlock();

        // Convert the encrypted data from a MemoryStream to a byte array
        byte[] cipherBytes = memoryStream.ToArray();

        // Close both the MemoryStream and the CryptoStream
        memoryStream.Close();
        cryptoStream.Close();

        // Convert the encrypted byte array to a base64 encoded string
        string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

        // Return the encrypted data as a string
        return cipherText;
    }

    private static string DecryptString(string cipherText, byte[] key, byte[] iv)
    {
        // Instantiate a new Aes object to perform string symmetric encryption
        Aes encryptor = Aes.Create();

        encryptor.Mode = CipherMode.CBC;
        //encryptor.KeySize = 256;
        //encryptor.BlockSize = 128;
        //encryptor.Padding = PaddingMode.Zeros;

        // Set key and IV
        encryptor.Key = key.Take(32).ToArray();
        encryptor.IV = iv;

        // Instantiate a new MemoryStream object to contain the encrypted bytes
        MemoryStream memoryStream = new MemoryStream();

        // Instantiate a new encryptor from our Aes object
        ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

        // Instantiate a new CryptoStream object to process the data and write it to the 
        // memory stream
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

        // Will contain decrypted plaintext
        string plainText = String.Empty;

        try
        {
            // Convert the message string into a byte array
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Decrypt the input message string
            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

            // Complete the decryption process
            cryptoStream.FlushFinalBlock();

            // Convert the decrypted data from a MemoryStream to a byte array
            byte[] plainBytes = memoryStream.ToArray();

            // Convert the decrypted byte array to string
            plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
        }
        finally
        {
            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();
        }

        // Return the decrypted data as a string
        return plainText;
    }
}
