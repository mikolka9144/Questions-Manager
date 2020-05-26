using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Points_submit
{
	// Token: 0x02000126 RID: 294
	internal class BinarySerializer
	{
		// Token: 0x060008E1 RID: 2273 RVA: 0x000313FC File Offset: 0x0002F5FC
		public static void Serialize(string path, object ob)
		{
			Stream stream = File.Open(path, FileMode.Create);
			new BinaryFormatter().Serialize(stream, ob);
			stream.Close();
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00031424 File Offset: 0x0002F624
		public static object Deserialize(string path)
		{
			object result;
				Stream stream = File.Open(path, FileMode.Open, FileAccess.Read);
				object obj = new BinaryFormatter().Deserialize(stream);
				stream.Close();
				result = obj;
			return result;
		}
	}
}
