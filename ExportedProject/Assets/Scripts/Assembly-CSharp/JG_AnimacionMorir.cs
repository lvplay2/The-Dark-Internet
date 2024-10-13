using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JG_AnimacionMorir : MonoBehaviour
{
	[Header("Referencias")]
	public JG_Jugador jugador;

	public EN_Enemigo enemigo;

	public Camera camaraJugador;

	public Camera camaraJugadorDepth;

	public FP_Joystick joystick;

	[Header("Camara Referencias")]
	public Camera camaraAnimacion;

	public EX_CopiarTransform_Eje_Y copiarEjeY;

	public Animator animator;

	[Header("Texto Hora")]
	public Text texto;

	[Header("Sonidos")]
	public AudioSource vocesAmbiente;

	private bool _mirando;

	private float _fov_de_inicio;

	private void Start()
	{
		_fov_de_inicio = camaraAnimacion.fieldOfView;
	}

	public void Reproducir()
	{
		if (!_mirando)
		{
			UI_Canvas.canvas.DesactivarBotones();
			joystick.ReseteaarJoystick();
			StartCoroutine(Mirar_Corrutina());
			StartCoroutine(Aumentar_Vision());
		}
	}

	public void Reiniciar()
	{
		_mirando = false;
		camaraAnimacion.fieldOfView = _fov_de_inicio;
	}

	private IEnumerator Mostrar_Hora()
	{
		texto.text = "0" + ES_EstadoEscena.estadoEscena.Vidas() + ":00";
		texto.color = new Color(0.925f, 0.925f, 0.925f, 0.925f);
		texto.enabled = true;
		yield return new WaitForSeconds(2.5f);
		float tiempo = 0f;
		Color colorInicial = Color.white;
		Color colorFinal = new Color(1f, 1f, 1f, 0f);
		while (tiempo < 1f)
		{
			texto.color = Color.Lerp(colorInicial, colorFinal, tiempo);
			tiempo += Time.deltaTime / 1.25f;
			yield return null;
		}
		texto.enabled = false;
	}

	private IEnumerator Aumentar_Vision()
	{
		float tiempoTransicion2 = 0.7f;
		float tiempo3 = 0f;
		float fov_inicial = 90f;
		float fov_final = 45f;
		Sonidos.sonidos.musicaAmbiente.Stop();
		Sonidos.sonidos.musicaPersecucion.Detener();
		Sonidos.sonidos.sonidoFinal.Play();
		camaraAnimacion.enabled = true;
		camaraJugador.enabled = false;
		camaraJugadorDepth.enabled = false;
		while (tiempo3 < 1f)
		{
			camaraAnimacion.fieldOfView = Mathf.Lerp(fov_inicial, fov_final, tiempo3);
			tiempo3 += Time.deltaTime / tiempoTransicion2;
			yield return null;
		}
		camaraAnimacion.fieldOfView = fov_final;
		_mirando = false;
		copiarEjeY.copiando = false;
		AudioSource.PlayClipAtPoint(Sonidos.sonidos.acuchillar, base.transform.position, 1f);
		animator.Play("MorirParado");
		tiempoTransicion2 = 0.8f;
		tiempo3 = 0f;
		while (tiempo3 < 1f)
		{
			camaraAnimacion.fieldOfView = Mathf.Lerp(fov_final, _fov_de_inicio, tiempo3);
			tiempo3 += Time.deltaTime / tiempoTransicion2;
			yield return null;
		}
		AudioSource.PlayClipAtPoint(Sonidos.sonidos.cuerpo_golpeando_suelo, base.transform.position, 0.9f);
		UI_Canvas_Dinamico.canvas_dinamico.Aparecer(UI_Canvas_Dinamico.canvas_dinamico.fondoNegro, 1.75f, true, 1.5f);
		yield return new WaitForSeconds(1.75f);
		if (ES_EstadoEscena.estadoEscena.SinVidas())
		{
			Object.FindObjectOfType<ES_EscenaCargando>().CargarEscenaAsyncronica("EscenaMorir");
			enemigo.gameObject.SetActive(false);
			yield break;
		}
		StartCoroutine(Mostrar_Hora());
		AudioSource.PlayClipAtPoint(Sonidos.sonidos.suspenso_1, jugador.transform.position, 1f);
		Sonidos.sonidos.vocesAmbiente.volume = 0.12f;
		Sonidos.sonidos.musicaAmbiente.volume = 0.1f;
		Sonidos.sonidos.musicaAmbiente.Play();
		Sonidos.sonidos.musicaPersecucion.volumen = 0f;
		Sonidos.sonidos.musicaPersecucion.CambiarDeAudio();
		Sonidos.sonidos.musicaPersecucion.Reproducir();
		enemigo.voces._musicaActivada = false;
		jugador.Recolocar_Y_Despertar();
		copiarEjeY.AsignarTransform(jugador.puntoResucitacion);
		jugador.transform.GetChild(0).rotation = Quaternion.identity;
		while (!jugador.Reactivado)
		{
			yield return null;
		}
		ADS_Anuncios.anuncios.MostrarIntersticial();
		jugador.DesbloquearMovimiento();
		yield return new WaitForSeconds(0.1f);
		camaraAnimacion.enabled = false;
		camaraJugador.enabled = true;
		camaraJugadorDepth.enabled = true;
		copiarEjeY.copiando = true;
		UI_Canvas.canvas.ActivarBotones(IT_Interactivo.AccionesPredeterminadas, true);
		tiempo3 = 0f;
		while (tiempo3 < 1f)
		{
			Sonidos.sonidos.vocesAmbiente.volume = Mathf.Lerp(0.12f, 0f, tiempo3);
			tiempo3 += Time.deltaTime / 0.7f;
			yield return null;
		}
	}

	private IEnumerator Mirar_Corrutina()
	{
		_mirando = true;
		float velocidadRotacion = 25f;
		while (_mirando)
		{
			Quaternion b = Quaternion.LookRotation(enemigo.transform.position + new Vector3(0f, -0.1f, 0f) - base.transform.position, Vector3.up);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, velocidadRotacion * Time.deltaTime);
			yield return null;
		}
	}
}
