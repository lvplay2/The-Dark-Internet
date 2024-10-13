using System.Collections;
using UnityEngine;

public class EN_Ragdoll : MonoBehaviour
{
	[Header("Huesos")]
	public Transform mano_I;

	public Transform mano_D;

	public Transform codo_I;

	public Transform codo_D;

	private Transform padreAnterior;

	[Header("Referencias")]
	public JG_Jugador jugador;

	public EN_Enemigo enemigo;

	public Animator animator;

	public Rigidbody[] huesos;

	[Header("Materiales")]
	public SkinnedMeshRenderer cara;

	public SkinnedMeshRenderer cuerpo_y_ropa;

	public SkinnedMeshRenderer gorro;

	public Material _vestimenta;

	public Material _cuerpo;

	public Material _gorro;

	public Material vestimenta_T;

	public Material cuerpo_T;

	public Material gorro_T;

	private bool _activado;

	private Coroutine desactivar;

	public void Activar_Ragdoll()
	{
		if (!_activado)
		{
			padreAnterior = mano_I.parent;
			mano_I.parent = codo_I;
			mano_D.parent = codo_D;
			enemigo.DesactivarAgente();
			animator.enabled = false;
			for (int i = 0; i < huesos.Length; i++)
			{
				huesos[i].collisionDetectionMode = CollisionDetectionMode.Continuous;
				huesos[i].isKinematic = false;
				huesos[i].useGravity = true;
			}
			_activado = true;
			Invoke("Desactivar_Ragdoll", enemigo.tiempo_Neutralizacion);
		}
	}

	public void Desactivar_Ragdoll()
	{
		if (desactivar == null)
		{
			desactivar = StartCoroutine(Desactivar());
		}
	}

	private IEnumerator Desactivar()
	{
		float tiempoTransicion = 2.5f;
		float tiempo = 0f;
		cara.material = cuerpo_T;
		cuerpo_y_ropa.materials = new Material[2] { vestimenta_T, cuerpo_T };
		gorro.material = gorro_T;
		vestimenta_T.color = Color.white;
		cuerpo_T.color = Color.white;
		gorro_T.color = Color.white;
		Color colorInicial = Color.white;
		Color colorFinal = new Color(1f, 1f, 1f, 0f);
		Color white = Color.white;
		while (tiempo < 1f)
		{
			Color color = Color.Lerp(colorInicial, colorFinal, tiempo);
			vestimenta_T.color = color;
			cuerpo_T.color = color;
			gorro_T.color = color;
			tiempo += Time.deltaTime / tiempoTransicion;
			yield return null;
		}
		enemigo.agente.enabled = false;
		base.transform.position = enemigo.recorrido.ObtenerPuntoMasLejano(jugador.transform.position);
		enemigo.agente.enabled = true;
		cara.material = _cuerpo;
		cuerpo_y_ropa.materials = new Material[2] { _vestimenta, _cuerpo };
		gorro.material = _gorro;
		mano_I.parent = padreAnterior;
		mano_D.parent = padreAnterior;
		enemigo.ActivarAgente();
		animator.enabled = true;
		for (int i = 0; i < huesos.Length; i++)
		{
			huesos[i].collisionDetectionMode = CollisionDetectionMode.Discrete;
			huesos[i].isKinematic = true;
			huesos[i].useGravity = false;
		}
		yield return new WaitForSeconds(1f);
		enemigo.vision.ObjetivoAlcanzado();
		enemigo.ContinuarPatrullando_SinVoz();
		_activado = false;
		desactivar = null;
	}
}
