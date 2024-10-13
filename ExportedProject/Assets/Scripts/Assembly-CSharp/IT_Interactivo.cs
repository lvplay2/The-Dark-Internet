using System.Linq;
using UnityEngine;

public class IT_Interactivo : MonoBehaviour
{
	public enum Acciones
	{
		Recoger = 0,
		Soltar = 1,
		Manejar = 2,
		Acelerar = 3,
		Frenar = 4,
		DejarDeUsar = 5,
		Entrar = 6,
		Salir = 7,
		Usar_ObjetoRecodigo = 8,
		Observar = 9,
		MoverArriba = 10,
		MoverAbajo = 11,
		MoverIzquierda = 12,
		MoverDerecha = 13,
		BajarGancho = 14,
		SinFlechas = 15,
		Moverse = 16,
		MirarAlrededor = 17,
		Llamar = 18,
		Agacharse = 19,
		Disparar = 20,
		DejarEnSuelo = 21,
		Conducto = 22,
		Casete = 23
	}

	public static readonly Acciones[] AccionesPredeterminadas = new Acciones[11]
	{
		Acciones.Moverse,
		Acciones.MirarAlrededor,
		Acciones.Recoger,
		Acciones.Soltar,
		Acciones.Manejar,
		Acciones.Agacharse,
		Acciones.Entrar,
		Acciones.Conducto,
		Acciones.Casete,
		Acciones.Disparar,
		Acciones.Llamar
	};

	private static int[] accionesActivadas = new int[0];

	public static Acciones[] acciones { get; private set; }

	public bool VisibleParaMano { get; protected set; }

	public bool VisibleParaOtro { get; protected set; }

	public void Icono()
	{
	}

	public virtual void Interaccionar(Acciones accion, bool seSolto)
	{
	}

	public static void AsignarAcciones(Acciones[] acciones)
	{
		IT_Interactivo.acciones = acciones;
		int[] array = Enumerable.ToArray(Enumerable.Cast<int>(acciones));
		if (!Enumerable.SequenceEqual(array, accionesActivadas))
		{
			UI_Canvas.canvas.ActivarBotones(acciones, true);
			accionesActivadas = new int[array.Length];
			array.CopyTo(accionesActivadas, 0);
		}
	}
}
