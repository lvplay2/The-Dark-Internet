using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IluminacionTemporal : MonoBehaviour
{
	private IT_Interactivo[] objetos;

	public List<IT_Interactivo> lista = new List<IT_Interactivo>();

	public List<Camera> lista2 = new List<Camera>();

	public List<OJ_Posicion> lista3 = new List<OJ_Posicion>();

	public List<Light> lista4 = new List<Light>();

	public AudioSource[] audioSources;

	[ContextMenu("Encontrar")]
	public void Encontrar()
	{
		objetos = Object.FindObjectsOfType<IT_Interactivo>();
		lista2 = Enumerable.ToList(Object.FindObjectsOfType<Camera>());
		lista3 = Enumerable.ToList(Object.FindObjectsOfType<OJ_Posicion>());
		audioSources = Object.FindObjectsOfType<AudioSource>();
		lista4 = Enumerable.ToList(Object.FindObjectsOfType<Light>());
		IT_Interactivo[] array = objetos;
		foreach (IT_Interactivo iT_Interactivo in array)
		{
			if (!(iT_Interactivo is IT_Recogible))
			{
				lista.Add(iT_Interactivo);
			}
		}
	}

	[ContextMenu("Estatico")]
	public void Estatico()
	{
		foreach (IT_Interactivo listum in lista)
		{
			listum.gameObject.isStatic = true;
		}
	}

	[ContextMenu("Dinamico")]
	public void Dinamico()
	{
		foreach (IT_Interactivo listum in lista)
		{
			listum.gameObject.isStatic = false;
		}
	}

	[ContextMenu("ForzarDinamico")]
	public void ForzarDinamico()
	{
		IT_Interactivo[] array = objetos;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.isStatic = false;
		}
	}
}
