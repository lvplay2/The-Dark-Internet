using System;
using UnityEngine;

public class UI_Usar : MonoBehaviour
{
	public enum EstadoBoton
	{
		Usar = 0,
		Comprar = 1,
		Usando = 2,
		Desactivar = 3
	}

	[Header("Referencias")]
	public UI_VisualizadorSkins visualizadorSkins;

	[Header("Botones")]
	public UI_Usar botonUsar;

	public GameObject boton_Usar;

	public GameObject boton_Comprar;

	public GameObject boton_Usando;

	private void Start()
	{
	}

	public void Click(int estadoBoton)
	{
		Reproducir_Sonido();
		visualizadorSkins.Seleccionar_Skin_Globalmente();
		switch ((EstadoBoton)estadoBoton)
		{
		case EstadoBoton.Usar:
			Boton_Usando();
			break;
		}
	}

	private void CompraSkinCompletada(string skinId)
	{
		ES_EstadoJuego.estadoJuego.DatosControlador.Registrar_Skin(visualizadorSkins._index, visualizadorSkins._tipoSkin, ES_Datos_Controlador.Accion.Desbloquear);
		UI_VisualizadorSkins.Visualizador_Skins_Aplicar_Cambios visualizador_Skins_Aplicar_Cambios = visualizadorSkins.visualizador_Skins_Aplicar_Cambios;
		if (visualizador_Skins_Aplicar_Cambios != null)
		{
			visualizador_Skins_Aplicar_Cambios();
		}
		Asigar_Estado(EstadoBoton.Usar);
	}

	public void Asigar_Estado(EstadoBoton estadoBoton)
	{
		switch (estadoBoton)
		{
		case EstadoBoton.Usar:
			Boton_Usar();
			break;
		case EstadoBoton.Comprar:
			Boton_Comprar();
			break;
		case EstadoBoton.Usando:
			Boton_Usando();
			break;
		case EstadoBoton.Desactivar:
			Boton_Desactivar();
			break;
		}
	}

	private void Boton_Usar()
	{
		Desactivar_Botones();
		boton_Usar.SetActive(true);
	}

	private void Boton_Comprar()
	{
		Desactivar_Botones();
		boton_Comprar.SetActive(true);
	}

	private void Boton_Usando()
	{
		Desactivar_Botones();
		boton_Usando.SetActive(true);
	}

	private void Boton_Desactivar()
	{
		Desactivar_Botones();
	}

	private void Desactivar_Botones()
	{
		boton_Usar.SetActive(false);
		boton_Comprar.SetActive(false);
		boton_Usando.SetActive(false);
	}

	private void Reproducir_Sonido()
	{
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_2, 0.7f);
	}
}
