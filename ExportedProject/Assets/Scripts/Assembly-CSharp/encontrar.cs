using UnityEngine;

public class encontrar : MonoBehaviour
{
	private Collider[] colliders;

	public Collider[] coll;

	[ContextMenu("Encontrar")]
	public void Encontrar()
	{
		colliders = new Collider[0];
		colliders = Object.FindObjectsOfType<Collider>();
		coll = new Collider[colliders.Length];
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject.activeSelf)
			{
				coll[i] = colliders[i];
			}
		}
	}

	[ContextMenu("Limpiar")]
	public void Limpiar()
	{
		Collider[] array = coll;
		for (int i = 0; i < array.Length; i++)
		{
			Object.DestroyImmediate(array[i]);
		}
	}
}
