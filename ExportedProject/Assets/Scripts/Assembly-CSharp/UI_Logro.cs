using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Logro : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Header("Referencias")]
	private UI_VisualizadorLogros visualizadorLogros;

	public int logroIndex;

	private Image imagen;

	private Color colorActivado = Color.white;

	private Color colorDesactivado = new Color(0.3f, 0.3f, 0.3f, 1f);

	private bool _desbloqueada;

	private void Awake()
	{
		ObtenerReferencias();
		Bloqueada();
		UI_VisualizadorLogros uI_VisualizadorLogros = visualizadorLogros;
		uI_VisualizadorLogros.visualizador_Logros_Aplicar_Cambios = (UI_VisualizadorLogros.Visualizador_Logros_Aplicar_Cambios)Delegate.Combine(uI_VisualizadorLogros.visualizador_Logros_Aplicar_Cambios, new UI_VisualizadorLogros.Visualizador_Logros_Aplicar_Cambios(Comprobar_Desbloqueo));
	}

	private void ObtenerReferencias()
	{
		visualizadorLogros = UnityEngine.Object.FindObjectOfType<UI_VisualizadorLogros>();
		imagen = GetComponent<Image>();
	}

	private void Comprobar_Desbloqueo()
	{
		if (ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Logro_Desbloqueado(logroIndex))
		{
			Desbloqueada();
		}
		else
		{
			Bloqueada();
		}
	}

	private void Boton_VisualizarLogro()
	{
		visualizadorLogros.Configurar(logroIndex, UI_VisualizadorLogros.Configuracion.Solo_Ver);
		visualizadorLogros.Visualizar_Logro(logroIndex);
	}

	public void Boton_VisualizarPoder()
	{
		visualizadorLogros.Configurar(logroIndex, UI_VisualizadorLogros.Configuracion.Poder);
		visualizadorLogros.Visualizar_Logro(logroIndex);
	}

	private void Boton_VisualizarHuevoDeOro()
	{
		visualizadorLogros.Configurar(logroIndex, UI_VisualizadorLogros.Configuracion.HuevoDeOro);
		visualizadorLogros.Visualizar_Logro(logroIndex);
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

	public void OnPointerClick(PointerEventData eventData)
	{
		Boton_VisualizarLogro();
	}
}
