using System.Collections;
using UnityEngine;

public class JG_Brazo : MonoBehaviour
{
	public enum Lado
	{
		Derecho = 0,
		Izquierdo = 1
	}

	[Header("Referencias")]
	public Animator animator;

	public Transform brazoAnimacion;

	public GameObject brazoAnimado;

	public SkinnedMeshRenderer skinnedMeshRenderer;

	[Header("Seguimiento")]
	public Transform camara;

	public float suavidad;

	[Header("Animacion")]
	public Animator animatorBrazoMovimiento;

	[Header("Seguimiento")]
	public Vector3 posicionInicial;

	public Vector3 rotacionInicial;

	public float velocidad;

	[Header("Contenido")]
	public JG_Brazo_Contenedor[] brazoElementos;

	public JG_Brazo_Contenedor[] brazoElementosRepetidos;

	public JG_Brazo_Contenedor_Estaticos[] brazoObjetosEstaticos;

	private Vector3 _posicionFinal;

	private Quaternion _rotacionFinal;

	private Coroutine animacionRotar;

	private void Start()
	{
		_posicionFinal = brazoAnimacion.localPosition;
		_rotacionFinal = brazoAnimacion.localRotation;
	}

	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, camara.rotation, Mathf.Lerp(0f, 1f, suavidad * Time.deltaTime));
		base.transform.position = camara.position;
	}

	public void Activar(IT_Recogible elemento, Lado lado, string objetoBrazo = "")
	{
		if (lado == Lado.Derecho && base.transform.lossyScale.x < 0f)
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else if (lado == Lado.Izquierdo && base.transform.lossyScale.x > 0f)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		brazoAnimado.SetActive(true);
		if (elemento != null)
		{
			for (int i = 0; i < brazoElementos.Length; i++)
			{
				if (brazoElementos[i].elemento == elemento)
				{
					animator.Play(brazoElementos[i].nombrePosicionAnimacion);
					if (animacionRotar != null)
					{
						StopCoroutine(animacionRotar);
					}
					animacionRotar = StartCoroutine(AnimacionRotar());
					return;
				}
			}
			for (int j = 0; j < brazoElementosRepetidos.Length; j++)
			{
				if (brazoElementosRepetidos[j].elemento.GetType() == elemento.GetType())
				{
					animator.Play(brazoElementosRepetidos[j].nombrePosicionAnimacion);
					if (animacionRotar != null)
					{
						StopCoroutine(animacionRotar);
					}
					animacionRotar = StartCoroutine(AnimacionRotar());
					return;
				}
			}
		}
		else if (objetoBrazo != "")
		{
			for (int k = 0; k < brazoObjetosEstaticos.Length; k++)
			{
				if (objetoBrazo == brazoObjetosEstaticos[k].objeto.name)
				{
					brazoObjetosEstaticos[k].objeto.SetActive(true);
					animator.Play(brazoObjetosEstaticos[k].nombrePosicionAnimacion);
					if (animacionRotar != null)
					{
						StopCoroutine(animacionRotar);
					}
					animacionRotar = StartCoroutine(AnimacionRotar());
					return;
				}
			}
		}
		brazoAnimado.SetActive(false);
	}

	public void Desactivar()
	{
		brazoAnimado.SetActive(false);
		for (int i = 0; i < brazoObjetosEstaticos.Length; i++)
		{
			brazoObjetosEstaticos[i].objeto.SetActive(false);
		}
	}

	public void Escondido(bool estado)
	{
		skinnedMeshRenderer.enabled = !estado;
		MeshRenderer meshRenderer = null;
		if (IT_Cartera.cartera.ElementoEnCartera != null)
		{
			meshRenderer = IT_Cartera.cartera.ElementoEnCartera.GetComponent<MeshRenderer>();
		}
		if ((bool)IT_Cartera.cartera.ElementoEnCartera && meshRenderer == null)
		{
			meshRenderer = IT_Cartera.cartera.ElementoEnCartera.GetComponentInChildren<MeshRenderer>();
		}
		if (meshRenderer != null)
		{
			meshRenderer.enabled = !estado;
		}
	}

	public void ReproducirAnimacion(string animacion)
	{
		animatorBrazoMovimiento.Play(animacion);
	}

	private IEnumerator AnimacionRotar()
	{
		brazoAnimacion.localPosition = posicionInicial;
		brazoAnimacion.localRotation = Quaternion.Euler(rotacionInicial);
		float tiempo = 0f;
		while (true)
		{
			tiempo = Mathf.Lerp(tiempo, 1f, velocidad * Time.deltaTime);
			brazoAnimacion.localPosition = Vector3.Lerp(posicionInicial, _posicionFinal, tiempo);
			brazoAnimacion.localRotation = Quaternion.Lerp(Quaternion.Euler(rotacionInicial), _rotacionFinal, tiempo);
			yield return null;
		}
	}
}
