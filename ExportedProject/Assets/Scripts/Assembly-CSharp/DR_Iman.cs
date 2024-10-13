using UnityEngine;

public class DR_Iman : MonoBehaviour
{
	public Vector3 posicionLlave;

	public Vector3 rotacionLlave;

	[HideInInspector]
	public bool llaveEnIman;

	public Transform llave;

	private void Start()
	{
		llaveEnIman = false;
	}

	private void Update()
	{
		if (!(llave == null) && Vector3.Distance(llave.position, base.transform.position) < 0.5f)
		{
			Atraer();
		}
	}

	private void Atraer()
	{
		Rigidbody component = llave.GetComponent<Rigidbody>();
		Collider component2 = llave.GetComponent<Collider>();
		if (component != null)
		{
			component.collisionDetectionMode = CollisionDetectionMode.Discrete;
			component.isKinematic = true;
			component.useGravity = false;
		}
		if (component2 != null)
		{
			component2.isTrigger = true;
		}
		llave.parent = base.transform;
		llave.localPosition = posicionLlave;
		llave.localEulerAngles = rotacionLlave;
		llaveEnIman = true;
	}

	public void SoltarLlave()
	{
		Collider component = llave.GetComponent<Collider>();
		if (component != null)
		{
			component.isTrigger = false;
		}
		llave = null;
		llaveEnIman = false;
	}
}
