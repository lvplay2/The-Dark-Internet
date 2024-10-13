using System.Collections.Generic;
using UnityEngine;

public static class MetodosDeExtension
{
	public static List<Transform> ObtenerHijosPorTag(this Transform transform, string tag)
	{
		List<Transform> list = new List<Transform>();
		AlgoritmoRecursivo(list, transform, tag);
		return list;
	}

	private static void AlgoritmoRecursivo(List<Transform> contenedor, Transform transform, string tag)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			AlgoritmoRecursivo(contenedor, transform.GetChild(i), tag);
		}
		if (transform.CompareTag(tag))
		{
			contenedor.Add(transform);
		}
	}
}
