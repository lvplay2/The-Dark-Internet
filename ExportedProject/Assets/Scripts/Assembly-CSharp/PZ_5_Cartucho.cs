public class PZ_5_Cartucho : IT_Interactivo
{
	public PZ_5_Arma arma;

	private void Update()
	{
		base.VisibleParaMano = IT_Cartera.cartera.Contiene<PZ_5_Arma>() && !arma.Cargada;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (!arma.Cargada && accion == Acciones.Recoger)
		{
			ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.escopeta_recarga, base.transform.position, 0.3f, ES_EstadoEscena.estadoEscena.audioGlobal);
			arma.Cargada = true;
			base.gameObject.SetActive(false);
		}
	}
}
