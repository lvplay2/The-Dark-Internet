using UnityEngine;

public class EN_Voces : MonoBehaviour
{
	public EN_Enemigo enemigo;

	public AudioSource audioEnemigo;

	[Header("Configuración")]
	public float tiempoMinimo;

	public float tiempoMaximo;

	[Header("Voces")]
	public EN_Voces_Contenido vocesEspañol;

	public EN_Voces_Contenido vocesIngles;

	[Header("Musica")]
	public MS_MusicaPersecucion musicaPersecucion;

	[HideInInspector]
	public bool _musicaActivada;

	private EN_Enemigo.Accion _accion;

	private EN_Enemigo.EventoEspecial _eventoEspecial;

	public void ReproducirVoz(bool forzarVoz, float probabilidad)
	{
		if (probabilidad > 100f)
		{
			Debug.LogError("¡ERROR!");
		}
		if ((!forzarVoz && audioEnemigo.isPlaying) || enemigo._neutralizado || (float)Random.Range(1, 101) > probabilidad)
		{
			return;
		}
		CancelInvoke("ReproducirVoz");
		AudioClip clip = null;
		if (_eventoEspecial == EN_Enemigo.EventoEspecial.Nulo)
		{
			switch (_accion)
			{
			case EN_Enemigo.Accion.Patrullar:
				clip = ObtenerVoces().patrullar.Random().audios.Random();
				break;
			case EN_Enemigo.Accion.Perseguir:
				clip = ObtenerVoces().perseguir.Random().audios.Random();
				break;
			case EN_Enemigo.Accion.Observar:
				clip = ObtenerVoces().observar.Random().audios.Random();
				break;
			case EN_Enemigo.Accion.Atacar:
				clip = ObtenerVoces().atacar.Random().audios.Random();
				break;
			}
		}
		else
		{
			switch (_eventoEspecial)
			{
			case EN_Enemigo.EventoEspecial.VerDron:
				clip = ObtenerVoces().verDron.Random().audios.Random();
				break;
			case EN_Enemigo.EventoEspecial.EscucharMuñeco:
				clip = ObtenerVoces().escucharMuñeco.Random().audios.Random();
				break;
			case EN_Enemigo.EventoEspecial.EscucharTelefono:
				clip = ObtenerVoces().escucharTelefono.Random().audios.Random();
				break;
			case EN_Enemigo.EventoEspecial.RomperTelefono:
				clip = ObtenerVoces().romperTelefono.Random().audios.Random();
				break;
			case EN_Enemigo.EventoEspecial.EscucharCrujido:
				clip = ObtenerVoces().escucharCrujido.Random().audios.Random();
				break;
			}
		}
		audioEnemigo.clip = clip;
		audioEnemigo.Play();
		_eventoEspecial = EN_Enemigo.EventoEspecial.Nulo;
		Invokar_ReproducirVoz();
	}

	public void MusicaPersecucion(bool activado)
	{
		if (activado && !_musicaActivada)
		{
			Sonidos.sonidos.musicaPersecucion.CambiarDeAudio();
		}
		_musicaActivada = activado;
	}

	private void Invokar_ReproducirVoz()
	{
		Invoke("ReproducirVoz", Random.Range(tiempoMinimo, tiempoMaximo));
	}

	private EN_Voces_Contenido ObtenerVoces()
	{
		switch (ES_EstadoJuego.estadoJuego.idioma)
		{
		case ES_EstadoJuego.Idioma.Español:
			return vocesEspañol;
		case ES_EstadoJuego.Idioma.Ingles:
			return vocesIngles;
		default:
			return null;
		}
	}

	public void Actualizar_Accion(EN_Enemigo.Accion accion)
	{
		_accion = accion;
	}

	public void Actualizar_EventoEspecial(EN_Enemigo.EventoEspecial eventoEspecial)
	{
		_eventoEspecial = eventoEspecial;
	}

	private void Update()
	{
		musicaPersecucion.volumen = Mathf.Lerp(musicaPersecucion.volumen, _musicaActivada ? 0.2f : 0f, _musicaActivada ? (3f * Time.deltaTime) : (0.5f * Time.deltaTime));
	}
}
