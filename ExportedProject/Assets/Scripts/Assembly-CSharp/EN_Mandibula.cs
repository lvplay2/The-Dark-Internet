using System.Collections;
using UnityEngine;

public class EN_Mandibula : MonoBehaviour
{
	public Animator animator;

	public float tiempoDeRotacion;

	public float grados;

	private bool _rotando;

	public void Abrir()
	{
		animator.Rebind();
		animator.Play("Hablar");
	}

	private IEnumerator Rotar()
	{
		_rotando = true;
		float tiempo = 0f;
		Quaternion rotacionActual = base.transform.localRotation;
		Quaternion rotacionFinal = rotacionActual * Quaternion.Euler(new Vector3(grados, 0f, 0f));
		while (tiempo < 1f)
		{
			base.transform.localRotation = Quaternion.Lerp(rotacionActual, rotacionFinal, tiempo);
			tiempo += Time.deltaTime / tiempoDeRotacion;
			yield return null;
		}
		base.transform.localRotation = rotacionFinal;
		yield return new WaitForSeconds(3f);
		base.transform.localRotation = rotacionActual;
		_rotando = false;
	}
}
