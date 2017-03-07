using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Formatter {

    public static string Encode(string data) {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter(); 
        formatter.Serialize(stream, data); 
        string convertedData = Convert.ToBase64String(stream.GetBuffer()); //Convert the data to a string
        return convertedData;
    }

    public static object Decode(string data) {
        MemoryStream stream = new MemoryStream(Convert.FromBase64String(data)); //Create an input stream from the string
        BinaryFormatter formatter = new BinaryFormatter();
        object deserializedData = formatter.Deserialize(stream);
        return deserializedData;
    }
}
