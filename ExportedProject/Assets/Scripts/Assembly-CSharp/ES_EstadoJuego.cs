using UnityEngine;

public class ES_EstadoJuego : MonoBehaviour
{
	public enum Dificultad
	{
		Extremo = 0,
		Dificil = 1,
		Normal = 2,
		Facil = 3,
		Fantasma = 4
	}

	public enum PreferenciasEnemigo
	{
		Cocina = 0,
		SalaPrincipal = 1,
		Sotano = 2,
		Cine = 3,
		PuertaFinal = 4,
		General = 5
	}

	public enum Idioma
	{
		Español = 0,
		Ingles = 1
	}

	public static ES_EstadoJuego estadoJuego;

	private ES_Datos_Controlador datosControlador = new ES_Datos_Controlador();

	public Dificultad? dificultad { get; private set; } = Dificultad.Extremo;


	public PreferenciasEnemigo? preferenciasEnemigo { get; private set; } = PreferenciasEnemigo.General;


	public Idioma? idioma { get; private set; } = Idioma.Español;


	public string NombreYoutuber { get; private set; }

	public ES_Datos_Controlador DatosControlador
	{
		get
		{
			datosControlador.Inicializar();
			return datosControlador;
		}
	}

	public ES_Anuncios_Controlador AnunciosControlador { get; private set; } = new ES_Anuncios_Controlador();


	public ES_Datos_Contenido DatosContenido { get; private set; }

	private void Awake()
	{
		if (estadoJuego == null)
		{
			estadoJuego = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (estadoJuego != this)
		{
			Object.Destroy(base.gameObject);
		}
		InicializarEstadoJuego();
		AnunciosControlador.Inicializar();
	}

	private void InicializarEstadoJuego()
	{
		DatosContenido = Object.FindObjectOfType<ES_Datos_Contenido>();
	}

	public void AsignarDificultad(Dificultad dificultad)
	{
		this.dificultad = dificultad;
	}

	public void AsignarPreferenciaDelEnemigo(PreferenciasEnemigo preferenciasEnemigo)
	{
		this.preferenciasEnemigo = preferenciasEnemigo;
	}

	public void AsignarIdioma(Idioma idioma)
	{
		this.idioma = idioma;
	}

	public void AsignarNombreYoutuber(string nombre)
	{
		NombreYoutuber = nombre;
	}
}
