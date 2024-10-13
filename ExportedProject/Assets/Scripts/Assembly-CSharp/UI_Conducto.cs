using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Conducto : UI_Boton
{
	public delegate void Completado();

	private EN_Enemigo enemigo;

	private JG_Vision vision;

	public Image circulo;

	public Completado completado;

	private bool _completado;

	protected override void Awake()
	{
		base.Awake();
		enemigo = Object.FindObjectOfType<EN_Enemigo>();
		vision = Object.FindObjectOfType<JG_Vision>();
	}

	private void OnEnable()
	{
		StartCoroutine(Bucle());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator Bucle()
	{
		while (true)
		{
			bool enVista = vision.ElementoEnVista is CD_Teletransportar;
			imagen.AsignarEstado(enVista);
			circulo.fillAmount += 0.8f * (float)_presionando * Time.deltaTime;
			circulo.fillAmount = Mathf.Clamp(circulo.fillAmount, 0f, 1f);
			if (!enVista)
			{
				circulo.fillAmount = 0f;
			}
			if (circulo.fillAmount >= 1f && !_completado)
			{
				enemigo.vision._perderDeVista = true;
				yield return null;
				Completado obj = completado;
				if (obj != null)
				{
					obj();
				}
				_completado = true;
			}
			if (!enVista || _presionando < 0)
			{
				_completado = false;
			}
			yield return null;
		}
	}
}
