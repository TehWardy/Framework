using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class NetworkUtils
{

	static BinaryFormatter formatter = new BinaryFormatter();

	public static byte[] SerializeObject(System.Object obj) {
		MemoryStream ms = new MemoryStream();
		formatter.Serialize(ms, obj);
		return ms.ToArray();
	}
	
	public static System.Object DeserializeObject(byte[] objData) {
		MemoryStream ms = new MemoryStream();
		ms.Write(objData, 0, objData.Length);
		ms.Seek(0, SeekOrigin.Begin);
		return formatter.Deserialize(ms);
	}
}


