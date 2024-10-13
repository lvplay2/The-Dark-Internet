using UnityEngine;

public class PZ_3_Peluche : IT_Recogible
{
	private bool _seleccionado;

	[Header("Puerta")]
	public Vector3 posicionEnCaja;

	public bool Seleccionado
	{
		get
		{
			return _seleccionado;
		}
		set
		{
			_seleccionado = value;
		}
	}

	protected override void Start()
	{
		base.Start();
		base.VisibleParaMano = true;
	}
}
