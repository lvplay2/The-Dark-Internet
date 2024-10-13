using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bake : MonoBehaviour
{
	private List<IT_Puerta> puertas;

	private List<IT_Cajon> cajones;

	private List<Light> luces;

	[ContextMenu("Volver Estaticos")]
	public void Encontrar()
	{
		puertas = Enumerable.ToList(Object.FindObjectsOfType<IT_Puerta>());
		cajones = Enumerable.ToList(Object.FindObjectsOfType<IT_Cajon>());
		foreach (IT_Puerta puerta in puertas)
		{
			puerta.gameObject.isStatic = true;
		}
		foreach (IT_Cajon cajone in cajones)
		{
			cajone.gameObject.isStatic = true;
		}
	}

	[ContextMenu("Volver Dinamicos Puertas y Cajones")]
	public void Aplicar()
	{
		foreach (IT_Puerta puerta in puertas)
		{
			puerta.gameObject.isStatic = false;
		}
		foreach (IT_Cajon cajone in cajones)
		{
			cajone.gameObject.isStatic = false;
		}
		puertas.Clear();
		cajones.Clear();
	}

	[ContextMenu("Encontrar Luces y Agrupar")]
	public void EncontrarLucesyAgrupar()
	{
		luces = Enumerable.ToList(Object.FindObjectsOfType<Light>());
		GameObject gameObject = new GameObject("Luces");
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < luces.Count; i++)
		{
			GameObject gameObject2 = new GameObject("Luz_" + i);
			list.Add(gameObject2);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = luces[i].transform.position;
			gameObject2.Añadir(luces[i]);
		}
	}

	[ContextMenu("Eliminar Luces Activas")]
	public void EliminarLuces()
	{
		List<Light> list = Enumerable.ToList(Object.FindObjectsOfType<Light>());
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].gameObject.activeSelf)
			{
				Object.DestroyImmediate(list[i]);
			}
		}
	}
}
