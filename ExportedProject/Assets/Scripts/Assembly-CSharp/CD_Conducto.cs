using UnityEngine;

public class CD_Conducto : IT_Interactivo
{
	private const int toques = 5;

	private int _toques;

	private bool _quitado;

	public GameObject colliderFisico;

	public Rigidbody rigibody;

	public Vector3 cambioPosicion;

	public BoxCollider boxCollider;

	private void Update()
	{
		base.VisibleParaMano = !_quitado;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger && !_quitado)
		{
			if (_toques != 5)
			{
				base.transform.position += cambioPosicion;
				_toques++;
				return;
			}
			colliderFisico.layer = LayerMask.NameToLayer(ES_Tags.Fantasma);
			rigibody.collisionDetectionMode = CollisionDetectionMode.Continuous;
			rigibody.isKinematic = false;
			rigibody.useGravity = true;
			Vector3 velocity = cambioPosicion * 100f;
			velocity.y = 2f;
			rigibody.velocity = velocity;
			Vector3 angularVelocity = default(Vector3).VectorRandom(0.25f, 0.75f);
			rigibody.angularVelocity = angularVelocity;
			_quitado = true;
			Invoke("AlColisionar", 0.3f);
		}
	}

	private void AlColisionar()
	{
		boxCollider.isTrigger = false;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.metal_1, base.transform.position, 0.225f, ES_EstadoEscena.estadoEscena.audioGlobal);
	}
}
