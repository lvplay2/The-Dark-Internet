using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Skin : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Header("Referencias")]
	private UI_VisualizadorSkins visualizadorSkins;

	[Header("Configuracion")]
	public ES_Datos_Controlador.TipoSkin tipoSkin;

	public int skinIndex;

	private UI_Usar usar;

	private Image tick;

	private Image imagen;

	private Color colorActivado = Color.white;

	private Color colorDesactivado = new Color(0.3f, 0.3f, 0.3f, 1f);

	private bool _desbloqueada;

	private bool _seleccionada;

	private void Awake()
	{
		ObtenerReferencias();
		Bloqueada();
		Deseleccionar();
		visualizadorSkins.Configurar(UI_VisualizadorSkins.Configuracion.Con_Interaccion);
		UI_VisualizadorSkins uI_VisualizadorSkins = visualizadorSkins;
		uI_VisualizadorSkins.visualizador_Skins_Aplicar_Cambios = (UI_VisualizadorSkins.Visualizador_Skins_Aplicar_Cambios)Delegate.Combine(uI_VisualizadorSkins.visualizador_Skins_Aplicar_Cambios, new UI_VisualizadorSkins.Visualizador_Skins_Aplicar_Cambios(Comprobar_Desbloqueo));
	}

	private void ObtenerReferencias()
	{
		visualizadorSkins = UnityEngine.Object.FindObjectOfType<UI_VisualizadorSkins>();
		usar = UnityEngine.Object.FindObjectOfType<UI_Usar>();
		imagen = GetComponent<Image>();
		tick = base.transform.GetChild(0).gameObject.GetComponent<Image>();
	}

	public void Comprobar_Desbloqueo()
	{
		if (ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Skin_Desbloqueada(skinIndex, tipoSkin))
		{
			Desbloqueada();
			if (skinIndex == ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Skin_Seleccionada(tipoSkin))
			{
				Seleccionar();
			}
			else
			{
				Deseleccionar();
			}
		}
		else
		{
			Bloqueada();
		}
	}

	public void Boton_VisualizarSkin()
	{
		visualizadorSkins.Configurar(UI_VisualizadorSkins.Configuracion.Con_Interaccion);
		visualizadorSkins.Visualizar_Skin(skinIndex, tipoSkin);
		if (_desbloqueada)
		{
			if (_seleccionada)
			{
				usar.Asigar_Estado(UI_Usar.EstadoBoton.Usando);
			}
			else
			{
				usar.Asigar_Estado(UI_Usar.EstadoBoton.Usar);
			}
		}
		else
		{
			usar.Asigar_Estado(UI_Usar.EstadoBoton.Comprar);
		}
	}

	private void Desbloqueada()
	{
		imagen.color = colorActivado;
		_desbloqueada = true;
	}

	private void Bloqueada()
	{
		imagen.color = colorDesactivado;
		_desbloqueada = false;
	}

	public void Seleccionar()
	{
		tick.enabled = true;
		_seleccionada = true;
	}

	public void Deseleccionar()
	{
		tick.enabled = false;
		_seleccionada = false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Boton_VisualizarSkin();
	}
}
