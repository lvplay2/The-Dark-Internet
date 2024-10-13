using System;
using UnityEngine;

[Serializable]
public class ES_Poder_Contenedor
{
	public string nombre;

	[Range(1f, 5f)]
	public int rareza;
}
