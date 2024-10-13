using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Canvas_Dinamico : MonoBehaviour
{
	public static UI_Canvas_Dinamico canvas_dinamico;

	[Header("Imágenes")]
	public Image fondoNegro;

	[Header("Textos")]
	public Text horaActual;

	private Coroutine desaparecer;

	private void Awake()
	{
		canvas_dinamico = this;
	}

	public void Aparecer(Image ui, float segundosParaDesaparecer, bool desaparecerSuavemente, float segundosSuavidad)
	{
		if (segundosParaDesaparecer >= 0f)
		{
			ui.enabled = true;
			desaparecer = StartCoroutine(Desaparecer(ui, segundosParaDesaparecer, desaparecerSuavemente, segundosSuavidad));
		}
	}

	private IEnumerator Desaparecer(Image ui, float segundosParaDesaparecer, bool desaparecerSuavemente, float segundosSuavidad)
	{
		if (desaparecerSuavemente)
		{
			float tiempo = 0f;
			Color colorInicial = Color.black;
			Color colorFinal = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
			ui.color = colorInicial;
			yield return new WaitForSeconds(segundosParaDesaparecer);
			while (tiempo < 1f)
			{
				ui.color = Color.Lerp(colorInicial, colorFinal, tiempo);
				tiempo += Time.deltaTime / segundosSuavidad;
				yield return null;
			}
			ui.enabled = false;
		}
		else
		{
			yield return new WaitForSeconds(segundosParaDesaparecer);
			ui.enabled = false;
		}
		desaparecer = null;
	}
}
