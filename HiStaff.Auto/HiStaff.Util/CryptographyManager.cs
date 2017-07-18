using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace HiStaff.Util
{
    public class CryptographyManager
    {
        #region Fields
        private static byte[] key = { };
        private static byte[] IV = { 38, 55, 206, 48, 28, 64, 20, 16 };
        private static string stringKey = "!56sdweiwincnxc103238932asjkx@#@*&%^*@udfudl;aslxmwdxdghytunedsxvctr63a#KN";
        #endregion

        #region Public Methods

        public static string Encrypt(string text)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] byteArray = Encoding.UTF8.GetBytes(text);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             des.CreateEncryptor(key, IV),
                                                             CryptoStreamMode.Write);

                cryptoStream.Write(byteArray, 0, byteArray.Length);
                cryptoStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception) { return string.Empty; }
        }

        public static string Decrypt(string text)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] byteArray = Convert.FromBase64String(text);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                             des.CreateDecryptor(key, IV),
                                                             CryptoStreamMode.Write);
                cryptoStream.Write(byteArray, 0, byteArray.Length);
                cryptoStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception) { return string.Empty; }
        }

        #endregion
        
        #region crypt old
        //        private static byte[] lbtVector = {
//    240,
//    3,
//    45,
//    29,
//    0,
//    76,
//    173,
//    59
//};

//        private static string lscryptoKey = "TVC_HCM_DBA_1234567890!";
//        public static string Decrypt(string text)
//        {
//            byte[] buffer = null;
//            TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
//            MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();

//            buffer = Convert.FromBase64String(text);
//            loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey));
//            loCryptoClass.IV = lbtVector;
//            return Encoding.ASCII.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
//        }

//        public static string Encrypt(string text)
//        {
//            string functionReturnValue = null;

//            TripleDESCryptoServiceProvider loCryptoClass = new TripleDESCryptoServiceProvider();
//            MD5CryptoServiceProvider loCryptoProvider = new MD5CryptoServiceProvider();
//            byte[] lbtBuffer = null;

//            lbtBuffer = System.Text.Encoding.ASCII.GetBytes(text);
//            loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey));
//            loCryptoClass.IV = lbtVector;
//            text = Convert.ToBase64String(loCryptoClass.CreateEncryptor().TransformFinalBlock(lbtBuffer, 0, lbtBuffer.Length));
//            functionReturnValue = text;

//            return functionReturnValue;
        //        }
        #endregion
    }
}