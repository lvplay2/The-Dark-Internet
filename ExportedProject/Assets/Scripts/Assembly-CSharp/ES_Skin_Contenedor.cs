using System;
using UnityEngine;

[Serializable]
public class ES_Skin_Contenedor
{
	[Header("Información")]
	[TextArea(1, 2)]
	public string nombre;

	[TextArea(1, 2)]
	public string descripcion;

	[Range(1f, 5f)]
	public int rareza;

	public Texture textura;

	[Range(0f, 100f)]
	public float probabilidad;
}
