using UnityEngine;

public class PZ_5_Puerta : IT_Interactivo
{
	public Collider collider;

	public GameObject candado;

	public GameObject cierre;

	private bool _destruido;

	private string observacion = "Este candado parece estar muy oxidado, quizas pueda destruirlo";

	private void Start()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (!_destruido)
		{
			UI_Canvas.canvas.observacion.Observar(observacion);
		}
	}

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
			base.VisibleParaMano = false;
			_destruido = true;
		}
	}
}
