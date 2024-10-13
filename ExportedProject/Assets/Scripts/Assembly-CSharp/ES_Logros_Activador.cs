using System;
using UnityEngine;

public class ES_Logros_Activador : MonoBehaviour
{
	public static ES_Logros_Activador logrosActivador;

	public UI_VisualizadorSkins visualizadorSkins;

	public UI_VisualizadorLogros visualizadorLogros;

	public AudioSource audioSource;

	private bool gatoAtaco;

	private bool[] cintasEscuchadas = new bool[7];

	private void Awake()
	{
		if (logrosActivador == null)
		{
			logrosActivador = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (logrosActivador != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		audioSource = GetComponent<AudioSource>();
		UI_VisualizadorSkins uI_VisualizadorSkins = visualizadorSkins;
		uI_VisualizadorSkins.visualizador_Skins_Aplicar_Cambios = (UI_VisualizadorSkins.Visualizador_Skins_Aplicar_Cambios)Delegate.Combine(uI_VisualizadorSkins.visualizador_Skins_Aplicar_Cambios, new UI_VisualizadorSkins.Visualizador_Skins_Aplicar_Cambios(ConsultarSkins));
	}

	private void ConsultarSkins()
	{
		for (int i = 0; i < 6; i++)
		{
			if (ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Seccion_Desbloqueada(i))
			{
				GanarLogro(i);
			}
		}
		if (ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Todas_Las_Skins_Desbloqueadas())
		{
			GanarLogro(6);
		}
		UI_VisualizadorLogros.Visualizador_Logros_Aplicar_Cambios visualizador_Logros_Aplicar_Cambios = visualizadorLogros.visualizador_Logros_Aplicar_Cambios;
		if (visualizador_Logros_Aplicar_Cambios != null)
		{
			visualizador_Logros_Aplicar_Cambios();
		}
	}

	public void Comprobar_Logro_11_Tiempo_Limite(float tiempoDeJuego)
	{
		if (ES_EstadoJuego.estadoJuego.dificultad == ES_EstadoJuego.Dificultad.Extremo && tiempoDeJuego <= 600f)
		{
			GanarLogro(10);
		}
	}

	public void Comprobar_Logro_12_Experto()
	{
		if (ES_EstadoJuego.estadoJuego.dificultad == ES_EstadoJuego.Dificultad.Extremo)
		{
			GanarLogro(11);
		}
	}

	public void Comprobar_Logro_13_La_Escopeta()
	{
		if (ES_EstadoJuego.estadoJuego.dificultad != ES_EstadoJuego.Dificultad.Fantasma)
		{
			GanarLogro(12);
		}
	}

	public void Gato_Ataco()
	{
		gatoAtaco = true;
	}

	public void Comprobar_Logro_14_Gatos_Fantasmas()
	{
		ES_EstadoJuego.Dificultad value = ES_EstadoJuego.estadoJuego.dificultad.Value;
		if ((value == ES_EstadoJuego.Dificultad.Dificil || value == ES_EstadoJuego.Dificultad.Extremo) && !gatoAtaco)
		{
			GanarLogro(13);
		}
	}

	public void Cinta_Escuchada(int cintaIndex)
	{
		cintasEscuchadas[cintaIndex] = true;
	}

	public void Comprobar_Logro_15_Todas_Las_Cintas()
	{
		for (int i = 0; i < cintasEscuchadas.Length; i++)
		{
			if (!cintasEscuchadas[i])
			{
				return;
			}
		}
		GanarLogro(14);
	}

	private void OnLevelWasLoaded(int level)
	{
		gatoAtaco = false;
		for (int i = 0; i < cintasEscuchadas.Length; i++)
		{
			cintasEscuchadas[i] = false;
		}
	}

	public void GanarLogro(int logroIndex)
	{
		if (!ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Logro_Desbloqueado(logroIndex))
		{
			ES_EstadoJuego.estadoJuego.DatosControlador.Registrar_Logro_Desbloqueado(logroIndex);
			UI_GanarLogros.ganarLogros.GanarLogro(logroIndex);
			audioSource.Play();
		}
	}
}
