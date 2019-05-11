using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

/// <summary>
/// Security purposes classes. Use AES-CBC Encryption with 32bit key and 16bit IV, PKCS7 padding mode
/// </summary>
public class AES
{
	private  static string chars="0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	static byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	static string GetString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}
	public static string AES_Key 	= "Q1nblvc3KyB1s7jQ";
	//private static string AES_Key	= "ZjkzODRjOGVkNzA2OTIyODYwZmU1N2M4MWQyOTY2Yzg=";
	public static string AES_IV 	= "c0017b4dfda445c8";
	//private static string AES_IV 	= "YzAwMTdiNGRmZGE0NDVjOA==";

	/// <summary>
	/// Encrypt a plain text with AES-CBC encryption. The result is base64 string
	/// </summary>
	/// <param name="plainText">Plain text.</param>
	public static string Encrypt(string plainText, bool isEncrypt = true)
	{
		if(isEncrypt)
		{	//Q1nblvc3KyB1s7jQ
			string IVTemp = generateIV();	
			string IVcode = IVTemp;

			var aes = new RijndaelManaged();
			aes.Mode = CipherMode.CBC;
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Padding = PaddingMode.PKCS7;
			aes.Key = System.Text.Encoding.UTF8.GetBytes (AES_Key);
			aes.IV = System.Text.Encoding.UTF8.GetBytes (IVcode);

			var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
			byte[] xBuff = null;
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
				{
					byte[] xXml = Encoding.UTF8.GetBytes(plainText);
					cs.Write(xXml, 0, xXml.Length);
				}

				xBuff = ms.ToArray();
			}

			String Output = Convert.ToBase64String(xBuff).ToString();

			string result = Output.Substring(0,Mathf.FloorToInt(Output.Length/2))+
				IVTemp.Substring(IVTemp.Length/2,IVTemp.Length/2) +
				IVTemp.Substring(0,IVTemp.Length/2)+
				Output.Substring(Mathf.FloorToInt(Output.Length/2),Output.Length - Mathf.FloorToInt(Output.Length/2));

			return result;
		}
		else
		{
			return plainText;
		}
	}

	/// <summary>
	/// Decrypt an ecnrypted text with AES-CBC decryption method	
	/// </summary>
	/// <param name="encryptedText">Encrypted text.</param>
	public static string Decrypt(string encryptedText, bool isEncrypt = true)
	{
		if(isEncrypt)
		{
			int encryptedTextCount = encryptedText.Length - 16;
			string IVTemp = encryptedText.Substring(Mathf.FloorToInt(encryptedTextCount/2)+8,8) + encryptedText.Substring(Mathf.FloorToInt(encryptedTextCount/2),8);

			string IVcode = IVTemp;

			string onlyEncryptedText = encryptedText.Substring(0,Mathf.FloorToInt(encryptedTextCount/2))+encryptedText.Substring(Mathf.FloorToInt(encryptedTextCount/2)+16,encryptedTextCount-Mathf.FloorToInt(encryptedTextCount/2));

			RijndaelManaged aes = new RijndaelManaged();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			aes.Key = System.Text.Encoding.UTF8.GetBytes (AES_Key);
			aes.IV = System.Text.Encoding.UTF8.GetBytes (IVcode);

			var decrypt = aes.CreateDecryptor();
			byte[] xBuff = null;
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
				{
					byte[] xXml = Convert.FromBase64String(onlyEncryptedText);
					cs.Write(xXml, 0, xXml.Length);
				}

				xBuff = ms.ToArray();
			}

			String Output = Encoding.UTF8.GetString(xBuff).ToString();

			string result = Output;
			return result;
		}
		else
		{
			return encryptedText;
		}
	}

	public static string generateIV(){
		String resultIV = "";
		for(int i=0;i<16;i++){
			resultIV += chars[UnityEngine.Random.Range (0,chars.Length)];
		}
		return resultIV;
	}
}
