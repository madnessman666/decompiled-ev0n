// Decompiled with JetBrains decompiler
// Type: Evon.Encryption
// Assembly: Evon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DB5636C-8858-4769-AD95-C6115039B8B8
// Assembly location: C:\Users\miham\Desktop\sw\Evon\Evon.exe

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Evon
{
  public class Encryption
  {
    public static byte[] EncryptBytes(byte[] inputBytes)
    {
      PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("sakpotisgay")), Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("sakpotisgay"))), "SHA-256", 2);
      RijndaelManaged rijndaelManaged1 = new RijndaelManaged();
      rijndaelManaged1.Mode = CipherMode.CBC;
      using (RijndaelManaged rijndaelManaged2 = rijndaelManaged1)
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndaelManaged2.CreateEncryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16)), CryptoStreamMode.Write))
          {
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
          }
        }
      }
    }

    public static byte[] DecryptBytes(byte[] inputBytes)
    {
      PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("sakpotisgay")), Encoding.ASCII.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes("sakpotisgay"))), "SHA-256", 2);
      RijndaelManaged rijndaelManaged1 = new RijndaelManaged();
      rijndaelManaged1.Mode = CipherMode.CBC;
      using (RijndaelManaged rijndaelManaged2 = rijndaelManaged1)
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndaelManaged2.CreateDecryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16)), CryptoStreamMode.Write))
          {
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
          }
        }
      }
    }
  }
}
