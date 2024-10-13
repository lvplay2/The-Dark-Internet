using System.Collections.Generic;
using UnityEngine;

public class SC_SistemaCofre : MonoBehaviour
{
	public enum Pildora
	{
		Skin = 0,
		Poder = 1,
		HuevoDeOro = 2
	}

	private class Skin
	{
		public int index;

		public List<Skin_Contenedor> skinsEstados;

		public Skin(int index, List<bool> skinsEstados)
		{
			this.index = index;
			this.skinsEstados = Crear_Contenedor(skinsEstados);
		}

		private List<Skin_Contenedor> Crear_Contenedor(List<bool> skinsEstados)
		{
			List<Skin_Contenedor> list = new List<Skin_Contenedor>();
			for (int i = 0; i < skinsEstados.Count; i++)
			{
				list.Add(new Skin_Contenedor(i, skinsEstados[i], ES_EstadoJuego.estadoJuego.DatosContenido.ObtenerSkin_Contenedor(i, (ES_Datos_Controlador.TipoSkin)index).probabilidad));
			}
			return list;
		}

		public float[] ObtenerProbabilidades()
		{
			float[] array = new float[skinsEstados.Count];
			for (int i = 0; i < skinsEstados.Count; i++)
			{
				array[i] = skinsEstados[i].probabilidad;
			}
			return array;
		}
	}

	private class Skin_Contenedor
	{
		public int index;

		public bool valor;

		public float probabilidad;

		public Skin_Contenedor(int index, bool valor, float probabilidad)
		{
			this.index = index;
			this.valor = valor;
			this.probabilidad = probabilidad;
		}
	}

	private List<Skin> secciones_Skins = new List<Skin>();

	[HideInInspector]
	public int[] poderes = new int[0];

	private UI_VisualizadorSkins visualizadorSkins;

	public UI_Logro[] logros;

	private void Awake()
	{
		visualizadorSkins = Object.FindObjectOfType<UI_VisualizadorSkins>();
	}

	private void Start()
	{
		Actualizar_Datos();
	}

	private void Actualizar_Datos()
	{
		secciones_Skins.Clear();
		secciones_Skins.Add(new Skin(0, new List<bool>(ES_EstadoJuego.estadoJuego.DatosControlador.datos.skinsGorro_Desbloqueado)));
		secciones_Skins.Add(new Skin(1, new List<bool>(ES_EstadoJuego.estadoJuego.DatosControlador.datos.skinsArma_Desbloqueado)));
		secciones_Skins.Add(new Skin(2, new List<bool>(ES_EstadoJuego.estadoJuego.DatosControlador.datos.skinsPuerta_Desbloqueado)));
		secciones_Skins.Add(new Skin(3, new List<bool>(ES_EstadoJuego.estadoJuego.DatosControlador.datos.skinsMuñeco_Desbloqueado)));
		secciones_Skins.Add(new Skin(4, new List<bool>(ES_EstadoJuego.estadoJuego.DatosControlador.datos.skinsPantallaCine_Desbloqueado)));
		secciones_Skins.Add(new Skin(5, new List<bool>(ES_EstadoJuego.estadoJuego.DatosControlador.datos.skinsDron_Desbloqueado)));
		poderes = new int[3] { 7, 8, 9 };
	}

	public Pildora GenerarPildora()
	{
		return (Pildora)SC_SistemaSeleccion.GenerarIndex(new float[3] { 0.85f, 0.15f, 0f });
	}

	public ES_Skin_Contenedor GenerarSkin(Skin_Informaicion skin_Informaicion = null)
	{
		Actualizar_Datos();
		secciones_Skins.Mezclar();
		while (secciones_Skins.Count > 0)
		{
			if (Esta_Todo_Desbloqueado(secciones_Skins[0].skinsEstados))
			{
				secciones_Skins.RemoveAt(0);
				continue;
			}
			while (secciones_Skins[0].skinsEstados.Count > 0)
			{
				int index = SC_SistemaSeleccion.GenerarIndex(secciones_Skins[0].ObtenerProbabilidades());
				if (secciones_Skins[0].skinsEstados[index].valor)
				{
					secciones_Skins[0].skinsEstados.RemoveAt(index);
					continue;
				}
				int index2 = secciones_Skins[0].skinsEstados[index].index;
				ES_Datos_Controlador.TipoSkin index3 = (ES_Datos_Controlador.TipoSkin)secciones_Skins[0].index;
				ES_EstadoJuego.estadoJuego.DatosControlador.Registrar_Skin(index2, index3, ES_Datos_Controlador.Accion.Desbloquear);
				UI_VisualizadorSkins.Visualizador_Skins_Aplicar_Cambios visualizador_Skins_Aplicar_Cambios = visualizadorSkins.visualizador_Skins_Aplicar_Cambios;
				if (visualizador_Skins_Aplicar_Cambios != null)
				{
					visualizador_Skins_Aplicar_Cambios();
				}
				if (skin_Informaicion != null)
				{
					skin_Informaicion.index = index2;
					skin_Informaicion.tipoSkin = index3;
				}
				return ES_EstadoJuego.estadoJuego.DatosContenido.ObtenerSkin_Contenedor(secciones_Skins[0].skinsEstados[index].index, (ES_Datos_Controlador.TipoSkin)secciones_Skins[0].index);
			}
		}
		return null;
	}

	private bool Esta_Todo_Desbloqueado(List<Skin_Contenedor> lista)
	{
		for (int i = 0; i < lista.Count; i++)
		{
			if (!lista[i].valor)
			{
				return false;
			}
		}
		return true;
	}
}
