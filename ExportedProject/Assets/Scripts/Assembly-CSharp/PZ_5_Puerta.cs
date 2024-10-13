using UnityEngine;

public class PZ_5_Puerta : MonoBehaviour
{
	public Collider collider;

	public GameObject candado;

	public GameObject cierre;

	private bool _destruido;

	public void Destruir()
	{
		if (!_destruido)
		{
			PZ_Puerta_Final.puerta_Final.Nuevo_Puzle_Desbloqueado();
			collider.enabled = false;
			Rigidbody component = cierre.GetComponent<Rigidbody>();
			Rigidbody component2 = candado.GetComponent<Rigidbody>();
			component.collisionDetectionMode = CollisionDetectionMode.Continuous;
			component.isKinematic = false;
			component.useGravity = true;
			component2.collisionDetectionMode = CollisionDetectionMode.Continuous;
			component2.isKinematic = false;
			component2.useGravity = true;
			component.velocity = default(Vector3).VectorRandom(-3f, 3f);
			component2.velocity = default(Vector3).VectorRandom(-3f, 3f);
			component.angularVelocity = default(Vector3).VectorRandom(-3f, 3f);
			component2.angularVelocity = default(Vector3).VectorRandom(-3f, 3f);
			cierre.GetComponent<Collider>().enabled = true;
			candado.GetComponent<Collider>().enabled = true;
			_destruido = true;
		}
	}
}
