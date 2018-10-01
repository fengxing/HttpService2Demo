using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SmartHttpService
{
    public static class EncryptExtendHelper
    {
        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="val">Base64源字符串</param>
        /// <returns></returns>
        public static string ToBase64String(this string val)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(val));
        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="s">需要加密的字符串</param>
        /// <returns></returns>
        public static string ToMd532Upper(this string s)
        {
            if (s == null)
            {
                return string.Empty;
            }
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            return string.Join("", bytes.Select(b => b.ToString("X2")));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="err">异常提示</param>
        /// <returns></returns>
        public static string NullOrEmpty(this string s, string err)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new Exception(err);
            }
            return s;
        }


        public static bool IsNullOrEmpty(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #region DES
        /// <summary>
        /// des64加密
        /// </summary>
        /// <param name="message">明文</param>
        /// <param name="key">密钥(8位长度的字符串)</param>
        /// <returns>密文</returns>
        public static string DES64Encrypt(this string message, string key, PaddingMode paddingMode = PaddingMode.Zeros)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                DES des = new DESCryptoServiceProvider();
                des.Key = Encoding.Default.GetBytes(key);
                des.IV = Encoding.Default.GetBytes(key);
                des.Mode = CipherMode.ECB;////兼容其他语言的Des加密算法
                des.Padding = paddingMode;//自动补0 
                byte[] bytes = Encoding.Default.GetBytes(message);
                byte[] resultBytes = des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
                return Convert.ToBase64String(resultBytes);
            }
            return "";
        }

        /// <summary>
        /// des64解密
        /// </summary>
        /// <param name="encrypt">密文</param>
        /// <param name="key">密钥(8位长度的字符串)</param>
        /// <returns>明文</returns>
        public static string DES64Decrypt(this string encrypt, string key, PaddingMode paddingMode = PaddingMode.Zeros)
        {
            if (!string.IsNullOrWhiteSpace(encrypt))
            {
                byte[] data = Convert.FromBase64String(encrypt);
                byte[] bKey = Encoding.Default.GetBytes(key.Substring(0, 8));
                byte[] bIV = bKey;
                DESCryptoServiceProvider desc = new DESCryptoServiceProvider();
                desc.Mode = CipherMode.ECB;//兼容其他语言的Des加密算法,这两行很重要,不加则会解密出错
                desc.Padding = paddingMode;//自动补0  
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, desc.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                byte[] result = mStream.ToArray();
                return Encoding.Default.GetString(result, 0, result.Length);
            }
            return "";
        }
        #endregion

        #region AES
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="key1">密钥向量</param>
        /// <returns>返回密文</returns>
        public static string AESEncrypt(this string plainText, string key, byte[] key1)
        {
            if (!string.IsNullOrEmpty(plainText))
            {
                //分组加密算法
                SymmetricAlgorithm des = Rijndael.Create();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组	
                //设置密钥及密钥向量
                des.Key = Encoding.UTF8.GetBytes(key);
                des.IV = key1;
                des.BlockSize = 128;
                des.Padding = PaddingMode.Zeros;
                des.Mode = CipherMode.ECB;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                    }
                    byte[] cipherBytes = ms.ToArray();
                    return Convert.ToBase64String(cipherBytes);
                }
            }
            return "";
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="key1">密钥向量</param>
        /// <returns>返回明文</returns>
        public static string AESDecrypt(this string cipherText, string key, byte[] key1)
        {
            if (!string.IsNullOrEmpty(cipherText))
            {
                byte[] buffer = Convert.FromBase64String(cipherText);

                SymmetricAlgorithm des = Rijndael.Create();
                des.Key = Encoding.UTF8.GetBytes(key);
                des.IV = key1;
                des.BlockSize = 128;
                des.Padding = PaddingMode.Zeros;
                des.Mode = CipherMode.ECB;
                byte[] decryptBytes = new byte[cipherText.Length];
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cs.Read(decryptBytes, 0, decryptBytes.Length);
                    }
                    return Encoding.UTF8.GetString(decryptBytes).TrimEnd('\0');
                }
            }
            return "";
        }
        #endregion
    }
}
