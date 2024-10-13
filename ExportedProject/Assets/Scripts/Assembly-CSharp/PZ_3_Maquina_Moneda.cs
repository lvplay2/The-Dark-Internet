using System.Collections;
using UnityEngine;

public class PZ_3_Maquina_Moneda : IT_Interactivo
{
	private Vector3 posicionMoneda = new Vector3(0f, -0.16f, -0.75f);

	private float fuerza = 2f;

	private float movimiento = 0.75f;

	private bool _enProceso;

	private Coroutine lanzarMoneda;

	public GameObject monedaPrefab;

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	private void Start()
	{
		Nombre = "Maquina Arcade";
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger && !_enProceso)
		{
			lanzarMoneda = StartCoroutine(LanzarMoneda());
		}
	}

	private IEnumerator LanzarMoneda()
	{
		_enProceso = true;
		float tiempoVibracion = 0.07f;
		float tiempoTotal = 1.5f;
		Vector3 rotacionInicial = base.transform.localEulerAngles;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.maquina_ganaste, base.transform.position, 0.25f, ES_EstadoEscena.estadoEscena.audioGlobal);
		do
		{
			base.transform.localRotation = Quaternion.Euler(rotacionInicial + default(Vector3).VectorRandom(0f, movimiento));
			yield return new WaitForSeconds(tiempoVibracion);
			tiempoTotal -= tiempoVibracion;
		}
		while (!(tiempoTotal < 0f));
		base.transform.localRotation = Quaternion.Euler(rotacionInicial);
		Rigidbody component = Object.Instantiate(monedaPrefab, base.transform.TransformPoint(posicionMoneda), Quaternion.identity).GetComponent<Rigidbody>();
		component.collisionDetectionMode = CollisionDetectionMode.Continuous;
		component.isKinematic = false;
		component.useGravity = true;
		component.velocity = -base.transform.up * fuerza;
		component.angularVelocity = default(Vector3).VectorRandom(1f, 7.5f);
		_enProceso = false;
		lanzarMoneda = null;
	}
}
