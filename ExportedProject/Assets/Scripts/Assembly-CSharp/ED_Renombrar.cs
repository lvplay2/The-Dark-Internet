using UnityEngine;

public class ED_Renombrar : MonoBehaviour
{
	[Header("Configuración")]
	public string nombre;

	[ContextMenu("Renombrar")]
	public void Renombrar()
	{
		int num = 1;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).name = nombre + "_" + num++;
		}
	}
}
