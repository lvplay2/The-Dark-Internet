using UnityEngine;

public class UI_OpcionesDePrivacidad : MonoBehaviour
{
	public GameObject panelOpcionesDePrivacidad;

	public void Activar_OpcionesDePrivacidad()
	{
		panelOpcionesDePrivacidad.SetActive(true);
	}

	public void Desactivar_OpcionesDePrivacidad()
	{
		panelOpcionesDePrivacidad.SetActive(false);
	}

	public void Boton_Aceptar()
	{
		ES_EstadoJuego.estadoJuego.AnunciosControlador.Establecer_GDPR(true);
		Desactivar_OpcionesDePrivacidad();
	}

	public void Boton_Rechazar()
	{
		ES_EstadoJuego.estadoJuego.AnunciosControlador.Establecer_GDPR(false);
		Desactivar_OpcionesDePrivacidad();
	}
}
