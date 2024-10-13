using UnityEngine;

public class ActDificultad : MonoBehaviour
{
	public bool facil;

	public bool normal;

	public bool dificil;

	public bool extremo;

	public bool fantasma;

	private void Start()
	{
		base.gameObject.SetActive(false);
		ES_EstadoJuego.Dificultad? dificultad = ES_EstadoJuego.estadoJuego.dificultad;
		if (!dificultad.HasValue)
		{
			return;
		}
		switch (dificultad.GetValueOrDefault())
		{
		case ES_EstadoJuego.Dificultad.Facil:
			if (facil)
			{
				base.gameObject.SetActive(true);
			}
			break;
		case ES_EstadoJuego.Dificultad.Normal:
			if (normal)
			{
				base.gameObject.SetActive(true);
			}
			break;
		case ES_EstadoJuego.Dificultad.Dificil:
			if (dificil)
			{
				base.gameObject.SetActive(true);
			}
			break;
		case ES_EstadoJuego.Dificultad.Extremo:
			if (extremo)
			{
				base.gameObject.SetActive(true);
			}
			break;
		case ES_EstadoJuego.Dificultad.Fantasma:
			if (fantasma)
			{
				base.gameObject.SetActive(true);
			}
			break;
		}
	}
}
