using UnityEngine;

public class PZ_5_LanzarHueso : MonoBehaviour
{
	private EN_Enemigo enemigo;

	private Rigidbody rigidbody;

	private SpringJoint joint;

	private void Awake()
	{
		enemigo = Object.FindObjectOfType<EN_Enemigo>();
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Disparar(Vector3 origen, Vector3 direccion, float fuerza, Vector3 posicion_hueso)
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(origen, direccion, out hitInfo, 100f, LayerMask.GetMask(ES_Tags.Estatico, ES_Tags.Estatico_F, ES_Tags.Interactivo_C)))
		{
			if (!joint)
			{
				GameObject gameObject = new GameObject("Joint-Arrastrar");
				Rigidbody obj = gameObject.AddComponent<Rigidbody>();
				joint = gameObject.AddComponent<SpringJoint>();
				joint.spring = 750f;
				joint.autoConfigureConnectedAnchor = false;
				obj.collisionDetectionMode = CollisionDetectionMode.Discrete;
				obj.isKinematic = true;
			}
			Ray ray = new Ray(origen, direccion);
			float num = Vector3.Distance(origen, posicion_hueso);
			float num2 = Vector3.Distance(origen, hitInfo.point);
			float num3 = Vector3.Distance(posicion_hueso, hitInfo.point);
			float distance = ((num + fuerza > num2) ? (num + num3) : (num + fuerza));
			Vector3 point = ray.GetPoint(distance);
			joint.transform.position = point;
			joint.connectedBody = rigidbody;
			Invoke("Desenganchar", 0.1f);
		}
	}

	private void Desenganchar()
	{
		joint.connectedBody = null;
	}
}
