using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sector_dll.util
{

	public class ReversibleRenamer
	{
		RijndaelManaged cipher;
		byte[] key;

		public static ReversibleRenamer inst = new ReversibleRenamer(null);

		public ReversibleRenamer(string password)
		{
			if(password == null)
            {
				cipher = null;
				return;
            }
			cipher = new RijndaelManaged();
			using (var sha = SHA256.Create())
				cipher.Key = key = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
		}

		static string Base64Encode(byte[] buf)
		{
			return Convert.ToBase64String(buf).Trim('=').Replace('+', '$').Replace('/', '_');
		}

		static byte[] Base64Decode(string str)
		{
			str = str.Replace('$', '+').Replace('_', '/').PadRight((str.Length + 3) & ~3, '=');
			return Convert.FromBase64String(str);
		}

		byte[] GetIV(byte ivId)
		{
			byte[] iv = new byte[cipher.BlockSize / 8];
			for (int i = 0; i < iv.Length; i++)
				iv[i] = (byte)(ivId ^ key[i]);
			return iv;
		}

		byte GetIVId(string str)
		{
			byte x = (byte)str[0];
			for (int i = 1; i < str.Length; i++)
				x = (byte)(x * 3 + (byte)str[i]);
			return x;
		}

		public string Encrypt(string name, object _ = null)
		{
			if (cipher == null) return name;
			byte ivId = GetIVId(name);
			cipher.IV = GetIV(ivId);
			var buf = Encoding.UTF8.GetBytes(name);

			using (var ms = new MemoryStream())
			{
				ms.WriteByte(ivId);
				using (var stream = new CryptoStream(ms, cipher.CreateEncryptor(), CryptoStreamMode.Write))
					stream.Write(buf, 0, buf.Length);

				buf = ms.ToArray();
				return Base64Encode(buf);
			}
		}

		public string Decrypt(string name, object _ = null)
		{
			if (cipher == null) return name;
			using (var ms = new MemoryStream(Base64Decode(name)))
			{
				byte ivId = (byte)ms.ReadByte();
				cipher.IV = GetIV(ivId);

				var result = new MemoryStream();
				using (var stream = new CryptoStream(ms, cipher.CreateDecryptor(), CryptoStreamMode.Read))
					stream.CopyTo(result);

				return Encoding.UTF8.GetString(result.ToArray());
			}
		}

		public static string Encrypt(string name) => inst.Encrypt(name);
		public static string Decrypt(string name) => inst.Decrypt(name);

	}
}
