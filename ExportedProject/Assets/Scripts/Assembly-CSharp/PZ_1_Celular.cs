using UnityEngine;

public class PZ_1_Celular : IT_Recogible
{
	public PZ_1_Telefono telefono;

	[HideInInspector]
	public bool Llamo;

	protected override void Start()
	{
		base.Start();
		lo_Solto = CancelarLlamada;
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Llamar)
		{
			Llamar();
		}
	}

	public override void Recoger(string ObjetoBrazo)
	{
		base.Recoger(ObjetoBrazo);
		UI_Canvas.canvas.ActivarBotones(new Acciones[1] { Acciones.Llamar }, false);
	}

	public override void Soltar(Caida caida)
	{
		base.Soltar(caida);
	}

	private void Llamar()
	{
		telefono.Llamar();
	}

	private void CancelarLlamada()
	{
		telefono.CancelarLlamada();
	}
}
