using UnityEngine;

public class Ejemplo : MonoBehaviour
{
	public Transform ObjetoPadre;

	private void Start()
	{
		string text = "MiTag";
		ObjetoPadre.ObtenerHijosPorTag(text);
	}
}
