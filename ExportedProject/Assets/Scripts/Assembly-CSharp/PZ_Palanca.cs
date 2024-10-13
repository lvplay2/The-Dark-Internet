using UnityEngine;

public class PZ_Palanca : IT_Interactivo
{
	public IT_Puerta puerta;

	public Animator animator;

	public GameObject bateria;

	public AudioSource audioSourcePuerta;

	private bool _activado;

	private bool _preparado;

	private string observacion = "Una palanca sin corriente, me pregunto para que servira";

	private void Update()
	{
		base.VisibleParaMano = !_activado;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger)
		{
			if (!IT_Cartera.cartera.Contiene<PZ_Bateria>() && !_preparado)
			{
				UI_Canvas.canvas.observacion.Observar(observacion);
			}
			else if (!_preparado)
			{
				((IT_Recogible)IT_Cartera.cartera.ElementoEnCartera).Suprimir();
				bateria.SetActive(true);
				AudioSource.PlayClipAtPoint(Sonidos.sonidos.colocarObjeto, base.transform.position, 0.3f);
				_preparado = true;
			}
			else if (!_activado)
			{
				animator.Play("Palanca");
				puerta.Abrir_PuertaConCerrojo();
				Invoke("ReproducirSonido", 1.75f);
				AudioSource.PlayClipAtPoint(Sonidos.sonidos.bajar_palanca, base.transform.position, 0.2f);
				_activado = true;
			}
		}
	}

	private void ReproducirSonido()
	{
		audioSourcePuerta.Play();
	}
}
