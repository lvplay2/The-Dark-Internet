using UnityEngine;

public class IT_Cartera : MonoBehaviour
{
	public static IT_Cartera cartera { get; private set; }

	public IT_Interactivo ElementoEnCartera { get; private set; }

	public IT_Interactivo ElementoEnUso { get; private set; }

	private void Awake()
	{
		cartera = this;
	}

	public void Asignar_ElementoEnCartera(IT_Interactivo i)
	{
		ElementoEnCartera = i;
	}

	public void Asignar_ElementoEnUso(IT_Interactivo i)
	{
		ElementoEnUso = i;
	}

	public void Quitar_ElementoEnCartera()
	{
		ElementoEnCartera = null;
	}

	public void Quitar_ElementoEnUso()
	{
		ElementoEnUso = null;
	}

	public void Vaciar()
	{
		ElementoEnCartera = null;
		ElementoEnUso = null;
	}

	public bool ContieneAlgo()
	{
		if (!(ElementoEnCartera != null))
		{
			return ElementoEnUso != null;
		}
		return true;
	}

	public bool Contiene<T>() where T : IT_Interactivo
	{
		if (!(ElementoEnCartera != null) || !(typeof(T) == ElementoEnCartera.GetType()))
		{
			if (ElementoEnUso != null)
			{
				return typeof(T) == ElementoEnUso.GetType();
			}
			return false;
		}
		return true;
	}

	public bool Contiene(IT_Interactivo i)
	{
		if (!(ElementoEnCartera != null) || !(i.GetType() == ElementoEnCartera.GetType()))
		{
			if (ElementoEnUso != null)
			{
				return i.GetType() == ElementoEnUso.GetType();
			}
			return false;
		}
		return true;
	}

	public T Obtener_ElementoEnCartera<T>() where T : IT_Interactivo
	{
		return (T)ElementoEnCartera;
	}

	public T Obtener_ElementoEnUso<T>() where T : IT_Interactivo
	{
		return (T)ElementoEnUso;
	}
}
