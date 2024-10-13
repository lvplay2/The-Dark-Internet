using System.Collections;
using UnityEngine;

public class PZ_2_Llave : IT_Recogible
{
	private const float velocidadRotacion = 80f;

	private new bool _recodigo;

	protected override void Start()
	{
		base.Start();
		StartCoroutine(Rotar());
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger)
		{
			_recodigo = true;
		}
	}

	private IEnumerator Rotar()
	{
		while (!_recodigo)
		{
			base.transform.eulerAngles += new Vector3(0f, 80f, 0f) * Time.deltaTime;
			yield return null;
		}
	}
}
