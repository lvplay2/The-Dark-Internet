using System;
using UnityEngine;

[Serializable]
public class ES_Logro_Contenedor
{
	public string nombre;

	[TextArea(1, 2)]
	public string descripcion_logro;

	[TextArea(1, 2)]
	public string descripcion_extra;

	public Texture textura;
}
