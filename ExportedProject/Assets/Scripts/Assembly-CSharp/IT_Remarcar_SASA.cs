using System;
using System.Collections;
using UnityEngine;

public class IT_Remarcar_SASA : MonoBehaviour
{
	private IT_Recogible recogiblePadre;

	private MeshRenderer meshRenderer;

	private bool activado;

	private void Awake()
	{
		recogiblePadre = GetComponentInParent<IT_Recogible>();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	public void Activar()
	{
		if (recogiblePadre != null)
		{
			IT_Recogible iT_Recogible = recogiblePadre;
			iT_Recogible.lo_Recogio = (IT_Recogible.Lo_Recogio)Delegate.Combine(iT_Recogible.lo_Recogio, new IT_Recogible.Lo_Recogio(Recodigo));
			IT_Recogible iT_Recogible2 = recogiblePadre;
			iT_Recogible2.lo_Solto = (IT_Recogible.Lo_Solto)Delegate.Combine(iT_Recogible2.lo_Solto, new IT_Recogible.Lo_Solto(Soltado));
			StartCoroutine(Posicionar());
		}
	}

	private IEnumerator Posicionar()
	{
		while (true)
		{
			if (meshRenderer.enabled != activado)
			{
				meshRenderer.enabled = activado;
			}
			yield return null;
		}
	}

	private void Recodigo()
	{
		activado = false;
	}

	private void Soltado()
	{
		activado = true;
	}
}
