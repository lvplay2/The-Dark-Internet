using UnityEngine;

public class UI_Entrar : UI_Boton
{
	private JG_Jugador jugador;

	private JG_Vision vision;

	protected override void Awake()
	{
		base.Awake();
		jugador = Object.FindObjectOfType<JG_Jugador>();
		vision = Object.FindObjectOfType<JG_Vision>();
	}

	private void Update()
	{
		imagen.AsignarEstado(vision.ElementoEnVista is EC_Collider);
	}
}
