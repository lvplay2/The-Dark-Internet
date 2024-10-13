using UnityEngine;

public class JG_Controlador : MonoBehaviour
{
	public JG_Jugador jugador;

	public JG_Vision vision;

	private void Start()
	{
		ES_Controles.controles.interaccionar_Click = Interaccionar;
	}

	public void Interaccionar(IT_Interactivo.Acciones accion, bool seSolto)
	{
		if (vision.ElementoEnVista != null)
		{
			vision.ElementoEnVista.Interaccionar(accion, seSolto);
		}
	}
}
