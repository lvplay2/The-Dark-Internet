using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_VisualizadorLogros : MonoBehaviour
{
	[HideInInspector]
	public enum Configuracion
	{
		Poder = 0,
		HuevoDeOro = 1,
		Solo_Ver = 2
	}

	public delegate void Visualizador_Logros_Aplicar_Cambios();

	[Header("Referencias")]
	public UI_VisualizadorSkins visualizadorSkins;

	public UI_Boton_Jugar botonJugar;

	[Header("Animación")]
	public Animator camara;

	public CanvasGroup canvasGroup;

	[Header("Configuración UI")]
	public Text ui_nombre;

	public Text ui_descripcion;

	[Header("Skins Mesh Renderer")]
	public MeshRenderer meshRenderer_Logro;

	[Header("Activacion")]
	public GameObject jugar_Y_Probar;

	private Configuracion configuracion;

	[HideInInspector]
	public bool _visualizando;

	[HideInInspector]
	public int _index;

	public Visualizador_Logros_Aplicar_Cambios visualizador_Logros_Aplicar_Cambios;

	private void Start()
	{
		Visualizador_Logros_Aplicar_Cambios obj = visualizador_Logros_Aplicar_Cambios;
		if (obj != null)
		{
			obj();
		}
		Cerrar(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cerrar(true);
		}
	}

	public void Configurar(int index, Configuracion configuracion)
	{
		this.configuracion = configuracion;
		switch (configuracion)
		{
		case Configuracion.Solo_Ver:
			jugar_Y_Probar.SetActive(false);
			ui_descripcion.text = ES_EstadoJuego.estadoJuego.DatosContenido.logros[index].descripcion_logro;
			ui_descripcion.fontSize = 55;
			break;
		case Configuracion.Poder:
		case Configuracion.HuevoDeOro:
			jugar_Y_Probar.SetActive(true);
			ui_descripcion.text = ES_EstadoJuego.estadoJuego.DatosContenido.logros[index].descripcion_extra;
			ui_descripcion.fontSize = 48;
			break;
		}
	}

	public void Visualizar_Logro(int index)
	{
		if (!_visualizando && base.gameObject.activeSelf)
		{
			visualizadorSkins.Desactivar_Skins();
			_index = index;
			StartCoroutine(Visualizar(ES_EstadoJuego.estadoJuego.DatosContenido.logros[index]));
		}
	}

	public void Utilizar_Logro()
	{
		ES_EstadoJuego.estadoJuego.DatosControlador.Activar_Extra_Unicamente(_index, (ES_Datos_Controlador.TipoOtro)configuracion);
		Object.FindObjectOfType<UI_Boton_Empezar>().AbrirPanelEmpezar(false);
	}

	public void Cerrar(bool reproducirSonidoCerrar)
	{
		StopAllCoroutines();
		_visualizando = false;
		camara.gameObject.SetActive(false);
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;
		if (reproducirSonidoCerrar)
		{
			SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_2, 0.6f);
		}
	}

	public void Desactivar_Logros()
	{
		meshRenderer_Logro.gameObject.SetActive(false);
	}

	private IEnumerator Visualizar(ES_Logro_Contenedor logroContenedor)
	{
		_visualizando = true;
		ui_nombre.text = logroContenedor.nombre;
		meshRenderer_Logro.gameObject.SetActive(true);
		meshRenderer_Logro.materials[1].mainTexture = logroContenedor.textura;
		camara.gameObject.SetActive(true);
		camara.Rebind();
		camara.Play("Animacion");
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.whoosh, 0.8f);
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_1, 0.8f);
		canvasGroup.blocksRaycasts = true;
		float tiempoTransicion = 1f;
		float tiempo = 0f;
		while (tiempo < 1f)
		{
			canvasGroup.alpha = Mathf.Lerp(0f, 1f, tiempo);
			tiempo += Time.deltaTime / tiempoTransicion;
			yield return null;
		}
		_visualizando = false;
	}
}
