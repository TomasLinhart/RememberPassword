using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace RememberPassword
{
	public static class AesCryptoServiceHelper
	{
		public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] IV)
		{
			string plaintext = null;

			using (Aes aesAlg = new AesManaged())
			{
				Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetString(IV, 0, IV.Length), key);
				aesAlg.Key = deriveBytes.GetBytes(128 / 8);
				aesAlg.IV = aesAlg.Key;

				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream msDecrypt = new MemoryStream(cipherText))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{
							plaintext = srDecrypt.ReadToEnd();
						}
					}
				}

			}

			return plaintext;
		}

		public static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] IV)
		{
			byte[] encrypted;

			using (Aes aesAlg = new AesManaged())
			{
				Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetString(IV, 0, IV.Length), key);
				aesAlg.Key = deriveBytes.GetBytes(128 / 8);
				aesAlg.IV = aesAlg.Key;

				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}

			return encrypted;

		}
	}
}

