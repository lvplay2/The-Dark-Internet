using UnityEngine;

public class UI_Boton_Ajustes : MonoBehaviour
{
	public GameObject ajustes;

	public void Abrir_Ajustes()
	{
		ajustes.SetActive(true);
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_1, 0.6f);
	}

	public void Cerrar_Ajustes()
	{
		ajustes.SetActive(false);
		SD_SonidosMenu.sonidosMenu.ReproducirSonido(SD_SonidosMenu.sonidosMenu.seleccionar_2, 0.8f);
	}
}
