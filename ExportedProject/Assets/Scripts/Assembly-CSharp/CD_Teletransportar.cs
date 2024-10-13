using System;
using UnityEngine;

public class CD_Teletransportar : IT_Interactivo
{
	public JG_Jugador jugador;

	public Transform puntoFinal;

	public UI_Conducto conducto;

	private void Start()
	{
		UI_Conducto uI_Conducto = conducto;
		uI_Conducto.completado = (UI_Conducto.Completado)Delegate.Combine(uI_Conducto.completado, new UI_Conducto.Completado(Completado));
	}

	private void Completado()
	{
		jugador.Recolocar_Conducto(base.transform, puntoFinal);
	}
}
