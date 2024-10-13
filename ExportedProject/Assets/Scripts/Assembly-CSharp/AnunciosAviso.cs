using System.Collections;
using UnityEngine;

public class AnunciosAviso : MonoBehaviour
{
	public Animator anuncioAviso;

	private void Start()
	{
		bool flag = !ES_EstadoJuego.estadoJuego.AnunciosControlador.anuncios.desactivados;
		base.gameObject.SetActive(flag);
		if (ES_EstadoJuego.estadoJuego.dificultad == ES_EstadoJuego.Dificultad.Fantasma && flag)
		{
			StartCoroutine(ControlarAnuncioAviso());
		}
	}

	private IEnumerator ControlarAnuncioAviso()
	{
		float tiempoEntreCartel = 0f;
		float tiempoEntreAnuncios = 0f;
		yield return new WaitForSeconds(4f);
		anuncioAviso.Play("Animacion");
		while (true)
		{
			if (tiempoEntreCartel > 50f)
			{
				anuncioAviso.Rebind();
				anuncioAviso.Play("Animacion");
			}
			if (tiempoEntreAnuncios > 90f)
			{
				ADS_Anuncios.anuncios.MostrarIntersticial();
				tiempoEntreAnuncios = 0f;
			}
			float num;
			tiempoEntreAnuncios = (num = tiempoEntreAnuncios + Time.deltaTime);
			tiempoEntreCartel = num;
			yield return null;
		}
	}
}
