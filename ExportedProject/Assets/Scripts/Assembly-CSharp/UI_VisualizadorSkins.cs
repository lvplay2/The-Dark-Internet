using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_VisualizadorSkins : MonoBehaviour
{
	public enum Configuracion
	{
		Con_Interaccion = 0,
		Sin_Interaccion = 1
	}

	public delegate void Visualizador_Skins_Aplicar_Cambios();

	[Header("Referencias")]
	public UI_VisualizadorLogros visualizadorLogros;

	[Header("Animación")]
	public Animator camara;

	public CanvasGroup canvasGroup;

	[Header("Configuración UI")]
	public Text ui_nombre;

	public Text ui_descripcion;

	public UI_Rareza_Skin ui_rareza;

	[Header("Skins Mesh Renderer")]
	public MeshRenderer meshRenderer_Gorro;

	public MeshRenderer meshRenderer_Arma;

	public MeshRenderer meshRenderer_Puerta;

	public SkinnedMeshRenderer[] meshRenderer_Muñeco;

	public MeshRenderer meshRenderer_PantallaCine;

	public MeshRenderer meshRenderer_Dron;

	[Header("Activacion")]
	public Image fondoPanel;

	public UI_Usar botonUsar;

	[Header("Detalles Menu")]
	public ES_ExtrasSkin skinGorroMenu;

	[HideInInspector]
	public bool _visualizando;

	[HideInInspector]
	public int _index;

	[HideInInspector]
	public ES_Datos_Controlador.TipoSkin _tipoSkin;

	public Visualizador_Skins_Aplicar_Cambios visualizador_Skins_Aplicar_Cambios;

	private void Start()
	{
		Visualizador_Skins_Aplicar_Cambios obj = visualizador_Skins_Aplicar_Cambios;
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

	public void Configurar(Configuracion configuracion)
	{
		switch (configuracion)
		{
		case Configuracion.Con_Interaccion:
			fondoPanel.enabled = true;
			break;
		case Configuracion.Sin_Interaccion:
			fondoPanel.enabled = false;
			break;
		}
	}

	public void Visualizar_Skin(int index, ES_Datos_Controlador.TipoSkin tipoSkin)
	{
		if (!_visualizando && base.gameObject.activeSelf)
		{
			visualizadorLogros.Desactivar_Logros();
			_index = index;
			_tipoSkin = tipoSkin;
			StartCoroutine(Visualizar(ES_EstadoJuego.estadoJuego.DatosContenido.ObtenerSkin_Contenedor(index, tipoSkin)));
		}
	}

	public void Seleccionar_Skin_Globalmente()
	{
		if (ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Skin_Desbloqueada(_index, _tipoSkin))
		{
			ES_EstadoJuego.estadoJuego.DatosControlador.Registrar_Skin(_index, _tipoSkin, ES_Datos_Controlador.Accion.Seleccionar);
			Visualizador_Skins_Aplicar_Cambios obj = visualizador_Skins_Aplicar_Cambios;
			if (obj != null)
			{
				obj();
			}
			skinGorroMenu.Cargar_Skin();
		}
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

	public void Desactivar_Skins()
	{
		meshRenderer_Gorro.gameObject.SetActive(false);
		meshRenderer_Arma.gameObject.SetActive(false);
		meshRenderer_Puerta.gameObject.SetActive(false);
		for (int i = 0; i < meshRenderer_Muñeco.Length; i++)
		{
			meshRenderer_Muñeco[i].gameObject.SetActive(false);
		}
		meshRenderer_PantallaCine.gameObject.SetActive(false);
		meshRenderer_Dron.gameObject.SetActive(false);
	}

	private IEnumerator Visualizar(ES_Skin_Contenedor skinContenedor)
	{
		_visualizando = true;
		ui_nombre.text = skinContenedor.nombre;
		ui_descripcion.text = skinContenedor.descripcion;
		ui_rareza.AsignarRareza(skinContenedor.rareza);
		Desactivar_Skins();
		switch (_tipoSkin)
		{
		case ES_Datos_Controlador.TipoSkin.Gorro:
			meshRenderer_Gorro.gameObject.SetActive(true);
			meshRenderer_Gorro.material.mainTexture = skinContenedor.textura;
			break;
		case ES_Datos_Controlador.TipoSkin.Arma:
			meshRenderer_Arma.gameObject.SetActive(true);
			meshRenderer_Arma.material.mainTexture = skinContenedor.textura;
			break;
		case ES_Datos_Controlador.TipoSkin.Puerta:
			meshRenderer_Puerta.gameObject.SetActive(true);
			meshRenderer_Puerta.material.mainTexture = skinContenedor.textura;
			break;
		case ES_Datos_Controlador.TipoSkin.Muñeco:
		{
			for (int i = 0; i < meshRenderer_Muñeco.Length; i++)
			{
				meshRenderer_Muñeco[i].gameObject.SetActive(true);
				meshRenderer_Muñeco[i].material.mainTexture = skinContenedor.textura;
			}
			break;
		}
		case ES_Datos_Controlador.TipoSkin.PantallaCine:
			meshRenderer_PantallaCine.gameObject.SetActive(true);
			meshRenderer_PantallaCine.material.mainTexture = skinContenedor.textura;
			break;
		case ES_Datos_Controlador.TipoSkin.Dron:
			meshRenderer_Dron.gameObject.SetActive(true);
			meshRenderer_Dron.material.mainTexture = skinContenedor.textura;
			break;
		}
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
