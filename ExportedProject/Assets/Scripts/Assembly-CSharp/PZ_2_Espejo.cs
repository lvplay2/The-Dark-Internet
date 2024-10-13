using System.Collections;
using UnityEngine;

public class PZ_2_Espejo : IT_Interactivo
{
	private const int toques = 2;

	private int _toques;

	[HideInInspector]
	public bool _lanzado;

	private bool vibrando;

	private Coroutine vibrar;

	public JG_Jugador jugador;

	public BoxCollider boxCollider;

	public Rigidbody rigidbody;

	public Vector3 lanzar;

	public Vector3 rotar;

	public AudioSource audioSource;

	public AudioClip[] scaryJane;

	public static bool _primerEspejoTirado;

	public PZ_2_Cruz cruz;

	public GameObject llave;

	private string observacion = "Este espejo tiene un simbolo muy curioso en su centro";

	private string observacion_2 = "Este objeto no parece servir aqui";

	private string observacion_3 = "Ya no necesito hacer este conjuro";

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger)
		{
			Tocar();
		}
	}

	private void Update()
	{
		base.VisibleParaMano = IT_Cartera.cartera.ElementoEnCartera != cruz && !_lanzado;
	}

	private void Tocar()
	{
		if (_primerEspejoTirado)
		{
			UI_Canvas.canvas.observacion.Observar(observacion_3);
		}
		else if (!IT_Cartera.cartera.Contiene<PZ_2_Cruz>())
		{
			UI_Canvas.canvas.observacion.Observar(observacion);
		}
		else if (IT_Cartera.cartera.ElementoEnCartera != cruz)
		{
			UI_Canvas.canvas.observacion.Observar(observacion_2);
		}
		else if (!vibrando && !_lanzado && !vibrando)
		{
			if (vibrar != null)
			{
				StopCoroutine(vibrar);
			}
			vibrar = StartCoroutine(Vibrar());
		}
	}

	private void Lanzar()
	{
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;
		rigidbody.velocity = lanzar;
		rigidbody.angularVelocity = rotar;
		llave.SetActive(true);
		_lanzado = true;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.metal_2, base.transform.position, 0.7f, ES_EstadoEscena.estadoEscena.audioGlobal);
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.suspenso_1, base.transform.position, 0.7f, ES_EstadoEscena.estadoEscena.audioGlobal);
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.voces_1, base.transform.position, 0.2f, ES_EstadoEscena.estadoEscena.audioGlobal);
		_primerEspejoTirado = true;
		Invoke("Desactivar_Collider", 3f);
	}

	private IEnumerator Vibrar()
	{
		vibrando = true;
		float tiempoSonido = 1.25f;
		float tiempoVibracion = 0.07f;
		float tiempoTotal = 0.75f;
		Vector3 posicionInical = base.transform.position;
		jugador.brazo.animatorBrazoMovimiento.Rebind();
		jugador.brazo.ReproducirAnimacion("Cruz");
		yield return new WaitForSeconds(0.35f);
		audioSource.clip = scaryJane[_toques];
		audioSource.Play();
		yield return new WaitForSeconds(tiempoSonido);
		do
		{
			Vector3 vector = default(Vector3).VectorRandom(-0.007f, 0.007f, 1);
			base.transform.position = posicionInical + vector;
			yield return new WaitForSeconds(tiempoVibracion);
			tiempoTotal -= tiempoVibracion;
		}
		while (!(tiempoTotal < 0f));
		base.transform.position = posicionInical;
		vibrando = false;
		if (_toques == 2)
		{
			Lanzar();
		}
		_toques++;
	}

	private void Desactivar_Collider()
	{
		boxCollider.enabled = false;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		rigidbody.isKinematic = true;
	}
}
