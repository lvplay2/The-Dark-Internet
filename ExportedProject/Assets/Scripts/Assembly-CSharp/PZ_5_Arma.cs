using UnityEngine;

public class PZ_5_Arma : IT_Recogible
{
	[HideInInspector]
	public bool Cargada;

	public Transform camaraJugador;

	public EN_Ragdoll enemigoRagdoll;

	public AudioSource audioSource;

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Disparar && Cargada)
		{
			Disparar();
		}
	}

	public override void Recoger(string ObjetoBrazo)
	{
		base.Recoger(ObjetoBrazo);
		UI_Canvas.canvas.ActivarBotones(new Acciones[1] { Acciones.Disparar }, false);
	}

	private void Disparar()
	{
		Cargada = false;
		RaycastHit hitInfo;
		Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out hitInfo, 100f, LayerMask.GetMask(ES_Tags.Estatico, ES_Tags.Estatico_F, ES_Tags.Interactivo, ES_Tags.Interactivo_C, ES_Tags.Hueso, ES_Tags.Animal));
		if ((bool)hitInfo.collider && !jugador.Muerto)
		{
			if (hitInfo.transform.gameObject.CompareTag(ES_Tags.Hueso))
			{
				enemigoRagdoll.Activar_Ragdoll();
				hitInfo.transform.GetComponent<PZ_5_LanzarHueso>().Disparar(camaraJugador.transform.position, camaraJugador.transform.forward, 3f, hitInfo.point);
				ES_Logros_Activador.logrosActivador.Comprobar_Logro_13_La_Escopeta();
			}
			else if (hitInfo.transform.gameObject.CompareTag(ES_Tags.Candado))
			{
				hitInfo.transform.GetComponent<PZ_5_Puerta>().Destruir();
			}
			else if (hitInfo.transform.gameObject.CompareTag(ES_Tags.Gato))
			{
				hitInfo.transform.GetComponent<GT_Gato>().Morir();
			}
		}
		jugador.brazo.animatorBrazoMovimiento.Rebind();
		jugador.brazo.ReproducirAnimacion("Disparar");
		audioSource.Reproducir(Sonidos.sonidos.escopeta_disparo, 0.34f, false);
	}
}
