using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PixBlocks.Tools.StringCompressor
{
	// Token: 0x02000132 RID: 306
	internal class StringCompressor
	{
		// Token: 0x06000912 RID: 2322 RVA: 0x00031EC4 File Offset: 0x000300C4
		public static string CompressString(string text)
		{
			if (text == null)
			{
				text = "";
			}
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			MemoryStream memoryStream = new MemoryStream();
			using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
			{
				gzipStream.Write(bytes, 0, bytes.Length);
			}
			memoryStream.Position = 0L;
			byte[] array = new byte[memoryStream.Length];
			memoryStream.Read(array, 0, array.Length);
			byte[] array2 = new byte[array.Length + 4];
			Buffer.BlockCopy(array, 0, array2, 4, array.Length);
			Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, array2, 0, 4);
			return Convert.ToBase64String(array2);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00031F70 File Offset: 0x00030170
		public static string DecompressString(string compressedText)
		{
			byte[] array = Convert.FromBase64String(compressedText);
			string @string;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int num = BitConverter.ToInt32(array, 0);
				memoryStream.Write(array, 4, array.Length - 4);
				byte[] array2 = new byte[num];
				memoryStream.Position = 0L;
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					gzipStream.Read(array2, 0, array2.Length);
				}
				@string = Encoding.UTF8.GetString(array2);
			}
			return @string;
		}
	}
}
