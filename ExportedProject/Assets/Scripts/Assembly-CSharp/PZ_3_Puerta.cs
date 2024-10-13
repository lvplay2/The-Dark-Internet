using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZ_3_Puerta : IT_Interactivo
{
	[HideInInspector]
	public int pelucheSeleccionado;

	[HideInInspector]
	public List<Material> materialesDesordenados;

	public Transform jugador;

	public EN_Enemigo enemigo;

	public MeshRenderer luz;

	public Animator animator;

	public PZ_3_Peluches peluches;

	private bool _escaneando;

	private bool _tapaAbierta;

	private bool _desbloqueado;

	private Coroutine escanear;

	private PZ_3_Peluche pelucheActual;

	public Transform posicionPeluche;

	private string observacion = "Una caja sellada, los colores no parecen alternarse por igual";

	private void Start()
	{
		StartCoroutine(Parpadear());
	}

	private void Update()
	{
		if (_desbloqueado)
		{
			base.VisibleParaMano = false;
			return;
		}
		base.VisibleParaMano = true;
		if (Vector3.Distance(jugador.position, base.transform.position) < 2f)
		{
			if (IT_Cartera.cartera.Contiene<PZ_3_Peluche>() && !_tapaAbierta)
			{
				AbrirTapa();
			}
			return;
		}
		if (_tapaAbierta && !_escaneando)
		{
			CerrarTapa();
		}
		base.VisibleParaMano = false;
	}

	private void AbrirTapa()
	{
		animator.SetInteger("Estado", 1);
		_tapaAbierta = true;
	}

	private void CerrarTapa()
	{
		animator.SetInteger("Estado", 0);
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.abrirCajaFuerte, base.transform.position, 0.6f, ES_EstadoEscena.estadoEscena.audioGlobal);
		_tapaAbierta = false;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion != 0 || _desbloqueado)
		{
			return;
		}
		if (IT_Cartera.cartera.Contiene<PZ_3_Peluche>())
		{
			if (!_escaneando)
			{
				if (escanear != null)
				{
					StopCoroutine(escanear);
				}
				escanear = StartCoroutine(Escanear());
			}
		}
		else if (!_escaneando && !_desbloqueado)
		{
			UI_Canvas.canvas.observacion.Observar(observacion);
		}
	}

	private IEnumerator Escanear()
	{
		_escaneando = true;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.colocarObjeto, base.transform.position, 0.3f, ES_EstadoEscena.estadoEscena.audioGlobal);
		pelucheActual = (PZ_3_Peluche)IT_Cartera.cartera.ElementoEnCartera;
		((IT_Recogible)IT_Cartera.cartera.ElementoEnCartera).Soltar(IT_Recogible.Caida.EjercerFuerza);
		pelucheActual.gameObject.layer = 0;
		Rigidbody rigidbody = pelucheActual.GetComponent<Rigidbody>();
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
		pelucheActual.transform.parent = posicionPeluche;
		pelucheActual.transform.localPosition = pelucheActual.posicionEnCaja;
		pelucheActual.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		float tiempo = 0f;
		while (tiempo < 3f)
		{
			posicionPeluche.transform.Rotate(0f, 30f * Time.deltaTime, 0f, Space.World);
			tiempo += Time.deltaTime;
			yield return null;
		}
		if (pelucheActual.Seleccionado)
		{
			_desbloqueado = true;
			CerrarTapa();
			animator.SetInteger("Estado", 2);
			base.VisibleParaMano = false;
			PZ_Puerta_Final.puerta_Final.Nuevo_Puzle_Desbloqueado();
		}
		else
		{
			ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.expulsar_muñeco, base.transform.position, 0.4f, ES_EstadoEscena.estadoEscena.audioGlobal);
			pelucheActual.transform.parent = null;
			pelucheActual.gameObject.layer = LayerMask.NameToLayer(ES_Tags.Interactivo);
			rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
			rigidbody.velocity = new Vector3(-3f, 0f, 0f);
			CerrarTapa();
			enemigo.Ruido(base.transform.position, EN_Enemigo.EventoEspecial.Nulo);
		}
		_escaneando = false;
	}

	private IEnumerator Parpadear()
	{
		while (true)
		{
			if (!_desbloqueado)
			{
				for (int i = 0; i < materialesDesordenados.Count; i++)
				{
					float seconds = ((i != pelucheSeleccionado) ? 0.35f : 0.5f);
					luz.material = materialesDesordenados[i];
					yield return new WaitForSeconds(seconds);
				}
				yield return null;
			}
			else
			{
				luz.material = peluches.materiales[2];
				luz.transform.Rotate(0f, -110f * Time.deltaTime, 0f);
				yield return null;
			}
		}
	}
}
