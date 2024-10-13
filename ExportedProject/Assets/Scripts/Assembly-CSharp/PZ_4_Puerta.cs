using System.Collections;
using UnityEngine;

public class PZ_4_Puerta : MonoBehaviour
{
	[HideInInspector]
	public string codigo = "";

	public PZ_4_Marcador marcador1;

	public PZ_4_Marcador marcador2;

	public PZ_4_Marcador marcador3;

	[Header("Animación")]
	public Transform marcador;

	public Vector3 posicionFinalMarcador;

	private bool _completado;

	public void ComprobarCodigo()
	{
		if (marcador1.ObtenerDigito() == codigo[0] && marcador2.ObtenerDigito() == codigo[1] && marcador3.ObtenerDigito() == codigo[2] && !_completado)
		{
			StartCoroutine(PuzleCompletado());
		}
	}

	private IEnumerator PuzleCompletado()
	{
		_completado = true;
		PZ_Puerta_Final.puerta_Final.Nuevo_Puzle_Desbloqueado();
		marcador1.completado = true;
		marcador2.completado = true;
		marcador3.completado = true;
		float tiempo = 0f;
		Vector3 posicionActual = marcador.transform.localPosition;
		while (tiempo < 1f)
		{
			marcador.transform.localPosition = Vector3.Lerp(posicionActual, posicionFinalMarcador, tiempo);
			tiempo += Time.deltaTime / 0.7f;
			yield return null;
		}
		yield return new WaitForSeconds(0.35f);
		while (true)
		{
			marcador1.GirarConstantemente(1);
			marcador2.GirarConstantemente(-1);
			marcador3.GirarConstantemente(1);
			yield return null;
		}
	}
}
