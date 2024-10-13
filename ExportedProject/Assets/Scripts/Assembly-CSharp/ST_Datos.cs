using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ST_Datos
{
	private enum ERROR
	{
		FicheroInexistente = 0
	}

	private const string Extension = ".dat";

	public static string Ruta
	{
		get
		{
			return Application.persistentDataPath;
		}
	}

	public static void Guardar<T>(string llave, T valor)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(Ruta + "/" + llave + ".dat");
		binaryFormatter.Serialize(fileStream, new Archivo
		{
			dato = valor
		});
		fileStream.Close();
	}

	public static T Cargar<T>(string llave)
	{
		if (File.Exists(Ruta + "/" + llave + ".dat"))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Open(Ruta + "/" + llave + ".dat", FileMode.Open);
			Archivo obj = (Archivo)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			return (T)obj.dato;
		}
		Error(ERROR.FicheroInexistente, llave);
		return default(T);
	}

	public static bool Existe(string nombreFichero, string extension = ".dat")
	{
		return File.Exists(Ruta + "/" + nombreFichero + ".dat");
	}

	private static void Error(ERROR error, string llave)
	{
		if (error == ERROR.FicheroInexistente)
		{
			Debug.LogError("¡El archivo " + llave + ".dat no existe!");
		}
	}
}
