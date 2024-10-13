using System;
using System.Collections;
using UnityEngine;

public class IT_Remarcar : MonoBehaviour
{
	private IT_Recogible recogiblePadre;

	private MeshRenderer meshRenderer;

	private bool activado;

	private Transform modeloPadre;

	public Transform modeloDistinto;

	private void Awake()
	{
		recogiblePadre = GetComponentInParent<IT_Recogible>();
		meshRenderer = GetComponent<MeshRenderer>();
	}

	public void Activar()
	{
		if (recogiblePadre != null)
		{
			base.transform.parent = null;
			base.gameObject.SetActive(true);
			StartCoroutine(Seguir());
		}
	}

	public void Desactivar()
	{
		Estado(false);
	}

	private void Recodigo()
	{
		Estado(false);
	}

	private void Soltado()
	{
		Estado(true);
	}

	private void Estado(bool estado)
	{
		meshRenderer.enabled = estado;
	}

	private IEnumerator Seguir()
	{
		IT_Recogible iT_Recogible = recogiblePadre;
		iT_Recogible.lo_Recogio = (IT_Recogible.Lo_Recogio)Delegate.Combine(iT_Recogible.lo_Recogio, new IT_Recogible.Lo_Recogio(Recodigo));
		IT_Recogible iT_Recogible2 = recogiblePadre;
		iT_Recogible2.lo_Solto = (IT_Recogible.Lo_Solto)Delegate.Combine(iT_Recogible2.lo_Solto, new IT_Recogible.Lo_Solto(Soltado));
		Estado(true);
		modeloPadre = ((modeloDistinto != null) ? modeloDistinto : recogiblePadre.transform);
		while (true)
		{
			base.transform.position = modeloPadre.transform.position;
			base.transform.rotation = modeloPadre.transform.rotation;
			base.transform.AsignarEscalaGlobal(modeloPadre.transform.lossyScale);
			yield return null;
		}
	}
}
