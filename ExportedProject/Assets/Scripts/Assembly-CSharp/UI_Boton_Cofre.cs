using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Boton_Cofre : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public SC_Cofre cofre;

	public UI_VisualizadorSkins visualizadorSkins;

	public Image boton;

	private bool _todasLasSkins;

	private void Update()
	{
	}

	private void Start()
	{
		Consultar_TodasLasSkins();
		UI_VisualizadorSkins uI_VisualizadorSkins = visualizadorSkins;
		uI_VisualizadorSkins.visualizador_Skins_Aplicar_Cambios = (UI_VisualizadorSkins.Visualizador_Skins_Aplicar_Cambios)Delegate.Combine(uI_VisualizadorSkins.visualizador_Skins_Aplicar_Cambios, new UI_VisualizadorSkins.Visualizador_Skins_Aplicar_Cambios(Consultar_TodasLasSkins));
	}

	private void Consultar_TodasLasSkins()
	{
		if (ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Todas_Las_Skins_Desbloqueadas())
		{
			_todasLasSkins = true;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		cofre.Activar();
	}
}
