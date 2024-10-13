using UnityEngine;

public class Nombrar2 : MonoBehaviour
{
	public string nombre;

	public GameObject[] objetos;

	[ContextMenu("Nombrar")]
	public void Nomrar_()
	{
		int num = 1;
		GameObject[] array = objetos;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].name = nombre + num;
			num++;
		}
	}
}
