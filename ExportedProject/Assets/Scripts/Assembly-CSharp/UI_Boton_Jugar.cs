using System.Collections;
using UnityEngine;

public class UI_Boton_Jugar : MonoBehaviour
{
	[Header("Referencias")]
	public ES_EscenaCargando cargadorEscena;

	public UI_Seleccion_Dificultad seleccionDificultad;

	public Canvas canvasMenu;

	public Canvas canvasComic;

	private bool _cargando;

	public void Boton_Jugar()
	{
		if (!_cargando)
		{
			StartCoroutine(CargarEscenaJuego());
			_cargando = true;
		}
	}

	private IEnumerator CargarEscenaJuego()
	{
		ES_EstadoJuego.estadoJuego.AsignarDificultad((ES_EstadoJuego.Dificultad)seleccionDificultad.dificultad);
		canvasComic.enabled = false;
		canvasMenu.enabled = false;
		cargadorEscena.CargarEscenaAsyncronica("EscenaComic");
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_2, 0.8f);
		yield return null;
	}
}
