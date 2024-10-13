using System.Collections;
using UnityEngine;

public class DR_CongelarFotograma : MonoBehaviour
{
	private Camera camara;

	private VHS_Effect efecto;

	public float tiempoMaximoVariacion;

	private CameraClearFlags clearFlags;

	private int cullingMask;

	private Coroutine congelarFotograma;

	private void Awake()
	{
		ObtenerReferencias();
	}

	private void ObtenerReferencias()
	{
		camara = GetComponent<Camera>();
		efecto = GetComponent<VHS_Effect>();
	}

	private void OnEnable()
	{
		clearFlags = camara.clearFlags;
		cullingMask = camara.cullingMask;
		if (congelarFotograma != null)
		{
			StopCoroutine(congelarFotograma);
		}
		congelarFotograma = StartCoroutine(CongelarFotograma());
	}

	private IEnumerator CongelarFotograma()
	{
		while (true)
		{
			camara.clearFlags = CameraClearFlags.Nothing;
			camara.cullingMask = 0;
			yield return new WaitForSeconds(Random.Range(0f, tiempoMaximoVariacion));
			camara.clearFlags = clearFlags;
			camara.cullingMask = cullingMask;
			yield return new WaitForSeconds(tiempoMaximoVariacion);
		}
	}
}
