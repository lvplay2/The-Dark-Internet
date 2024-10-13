using UnityEngine;

public class sadasada : MonoBehaviour
{
	public IT_Interactivo[] g1;

	public IT_Interactivo[] g2;

	[ContextMenu("SADA")]
	private void xfh()
	{
		IT_Interactivo[] array = Object.FindObjectsOfType<IT_Puerta>();
		g1 = array;
		array = Object.FindObjectsOfType<IT_Cajon>();
		g2 = array;
		array = g1;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.isStatic = false;
		}
		array = g2;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.isStatic = false;
		}
	}
}
