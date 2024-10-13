using UnityEngine;

public class TESTTTT : MonoBehaviour
{
	[ContextMenu("estatico")]
	public void Estaticos()
	{
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject in array)
		{
			if (!gameObject.isStatic && gameObject.activeSelf)
			{
				gameObject.isStatic = true;
			}
		}
	}

	[ContextMenu("res")]
	public void Rstaticos()
	{
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject in array)
		{
			if (gameObject.isStatic)
			{
				gameObject.isStatic = false;
			}
		}
	}
}
