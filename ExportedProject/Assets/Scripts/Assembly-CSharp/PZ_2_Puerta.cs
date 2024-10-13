using UnityEngine;

public class PZ_2_Puerta : IT_Interactivo
{
	public GameObject cabra;

	public Animator animator;

	private bool _abierto;

	private void Update()
	{
		if (!_abierto)
		{
			base.VisibleParaMano = IT_Cartera.cartera.Contiene<PZ_2_Cabra>();
		}
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger && IT_Cartera.cartera.Contiene<PZ_2_Cabra>())
		{
			((IT_Recogible)IT_Cartera.cartera.ElementoEnCartera).Suprimir();
			Abrir();
		}
	}

	private void Abrir()
	{
		if (!_abierto)
		{
			cabra.SetActive(true);
			animator.Play("Animacion");
			ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.colocarObjeto, base.transform.position, 0.3f, ES_EstadoEscena.estadoEscena.audioGlobal);
			_abierto = true;
			PZ_Puerta_Final.puerta_Final.Nuevo_Puzle_Desbloqueado();
		}
	}
}
