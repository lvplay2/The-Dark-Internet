using UnityEngine;

public class Creador : MonoBehaviour
{
	public GameObject[] objetos;

	private GameObject[] entidades;

	[ContextMenu("LimpiarCuenta")]
	public void LimpiarCuenta()
	{
		PlayerPrefs.DeleteKey("Cuenta");
	}

	[ContextMenu("Adaptar")]
	public void CrearObjetosObservables()
	{
		entidades = new GameObject[objetos.Length];
		string text = "Elemento_Observable_";
		int num = PlayerPrefs.GetInt("Cuenta");
		if (num == 0)
		{
			num++;
		}
		for (int i = 0; i < objetos.Length; i++)
		{
			entidades[i] = new GameObject(text + num);
			num++;
			entidades[i].transform.position = objetos[i].transform.position;
			entidades[i].transform.localScale = objetos[i].transform.localScale;
			entidades[i].transform.localRotation = objetos[i].transform.localRotation;
			objetos[i].isStatic = false;
			objetos[i].transform.parent = entidades[i].transform;
			objetos[i].name = "Modelo_1";
			objetos[i].layer = LayerMask.NameToLayer("Observar");
			entidades[i].layer = LayerMask.NameToLayer("ElementoDeJuego");
			BoxCollider boxCollider = objetos[i].GetComponent<BoxCollider>();
			if (boxCollider == null)
			{
				boxCollider = objetos[i].AddComponent<BoxCollider>();
			}
			Rigidbody rigidbody = entidades[i].AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
			BoxCollider boxCollider2 = entidades[i].AddComponent<BoxCollider>();
			boxCollider2.isTrigger = true;
			boxCollider2.size = boxCollider.size;
			boxCollider2.center = boxCollider.center;
			Object.DestroyImmediate(boxCollider);
			entidades[i].AddComponent<EJ_Observar>().distancia = 0.3f;
		}
		PlayerPrefs.SetInt("Cuenta", num);
	}
}
