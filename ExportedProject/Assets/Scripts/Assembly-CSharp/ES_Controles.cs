using UnityEngine;
using UnityEngine.SceneManagement;

public class ES_Controles : MonoBehaviour
{
	public delegate void Interaccionar_Click(IT_Interactivo.Acciones accion, bool seSolto);

	public static ES_Controles controles;

	public Interaccionar_Click interaccionar_Click;

	private void Awake()
	{
		controles = this;
	}

	public void Click(IT_Interactivo.Acciones accion, bool seSolto)
	{
		bool flag = IT_Cartera.cartera.ElementoEnCartera != null;
		bool flag2 = IT_Cartera.cartera.ElementoEnUso != null;
		switch (accion)
		{
		case IT_Interactivo.Acciones.Recoger:
		case IT_Interactivo.Acciones.Manejar:
		case IT_Interactivo.Acciones.Entrar:
		case IT_Interactivo.Acciones.Conducto:
			interaccionar_Click(accion, seSolto);
			break;
		case IT_Interactivo.Acciones.Soltar:
		case IT_Interactivo.Acciones.Llamar:
		case IT_Interactivo.Acciones.Disparar:
		case IT_Interactivo.Acciones.DejarEnSuelo:
		case IT_Interactivo.Acciones.Casete:
			if (flag)
			{
				IT_Cartera.cartera.ElementoEnCartera.Interaccionar(accion, seSolto);
			}
			break;
		case IT_Interactivo.Acciones.Acelerar:
		case IT_Interactivo.Acciones.Frenar:
		case IT_Interactivo.Acciones.Salir:
		case IT_Interactivo.Acciones.MoverArriba:
		case IT_Interactivo.Acciones.MoverAbajo:
		case IT_Interactivo.Acciones.MoverIzquierda:
		case IT_Interactivo.Acciones.MoverDerecha:
		case IT_Interactivo.Acciones.BajarGancho:
			if (flag2)
			{
				IT_Cartera.cartera.ElementoEnUso.Interaccionar(accion, seSolto);
			}
			break;
		case IT_Interactivo.Acciones.DejarDeUsar:
		case IT_Interactivo.Acciones.Usar_ObjetoRecodigo:
		case IT_Interactivo.Acciones.Observar:
		case IT_Interactivo.Acciones.SinFlechas:
		case IT_Interactivo.Acciones.Moverse:
		case IT_Interactivo.Acciones.MirarAlrededor:
		case IT_Interactivo.Acciones.Agacharse:
			break;
		}
	}

	private void Update()
	{
		if (IT_Cartera.cartera.ElementoEnUso != null && Input.touchCount <= 0)
		{
			Click(IT_Interactivo.Acciones.SinFlechas, false);
		}
		if (Input.GetKeyDown("e"))
		{
			Click(IT_Interactivo.Acciones.Recoger, false);
			Click(IT_Interactivo.Acciones.Entrar, false);
		}
		if (Input.GetKeyDown("r"))
		{
			if (IT_Cartera.cartera.ElementoEnCartera != null)
			{
				Click(IT_Interactivo.Acciones.Soltar, false);
			}
			Click(IT_Interactivo.Acciones.Salir, false);
		}
		if (Input.GetKeyDown("f"))
		{
			interaccionar_Click(IT_Interactivo.Acciones.Manejar, false);
		}
		if (Input.GetKeyDown("w") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.Acelerar, false);
		}
		if (Input.GetKeyUp("w") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.Acelerar, true);
		}
		if (Input.GetKeyDown("s") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.Frenar, false);
		}
		if (Input.GetKeyUp("s") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.Frenar, true);
		}
		if (Input.GetKeyDown("a") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.MoverIzquierda, false);
		}
		if (Input.GetKeyUp("a") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.MoverIzquierda, true);
		}
		if (Input.GetKeyDown("d") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.MoverDerecha, false);
		}
		if (Input.GetKeyUp("d") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.MoverDerecha, true);
		}
		if (Input.GetKeyDown("g") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.DejarDeUsar, false);
		}
		if (Input.GetKeyDown("q"))
		{
			Object.FindObjectOfType<EN_Vision>()._ignorarJugador = !Object.FindObjectOfType<EN_Vision>()._ignorarJugador;
		}
		if (Input.GetKeyDown("x") && IT_Cartera.cartera.ElementoEnCartera != null)
		{
			IT_Cartera.cartera.ElementoEnCartera.Interaccionar(IT_Interactivo.Acciones.Usar_ObjetoRecodigo, false);
		}
		if (Input.GetKeyDown("t"))
		{
			interaccionar_Click(IT_Interactivo.Acciones.Entrar, false);
		}
		if (Input.GetKeyDown("y") && IT_Cartera.cartera.ElementoEnUso != null)
		{
			IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.Salir, false);
		}
		if (Input.GetKeyDown("space"))
		{
			if (IT_Cartera.cartera.ElementoEnUso != null)
			{
				IT_Cartera.cartera.ElementoEnUso.Interaccionar(IT_Interactivo.Acciones.BajarGancho, false);
			}
			if ((bool)IT_Cartera.cartera.ElementoEnCartera)
			{
				IT_Cartera.cartera.ElementoEnCartera.Interaccionar(IT_Interactivo.Acciones.Disparar, false);
			}
		}
		if (Input.GetKeyDown("p"))
		{
			SceneManager.LoadScene(0);
		}
	}
}
