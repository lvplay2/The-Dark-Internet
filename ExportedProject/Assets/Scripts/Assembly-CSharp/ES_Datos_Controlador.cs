using System;

[Serializable]
public class ES_Datos_Controlador
{
	public enum TipoOtro
	{
		Poder = 0,
		HuevoDeOro = 1
	}

	public enum TipoSkin
	{
		Gorro = 0,
		Arma = 1,
		Puerta = 2,
		Muñeco = 3,
		PantallaCine = 4,
		Dron = 5
	}

	public enum Accion
	{
		Desbloquear = 0,
		Seleccionar = 1
	}

	public ES_Datos datos = new ES_Datos();

	private ES_Activados extrasActivados = new ES_Activados();

	private const string nombreEnDisco = "datos";

	private bool inicializado;

	public void Inicializar()
	{
		if (!inicializado)
		{
			inicializado = true;
			if (!ST_Datos.Existe("datos"))
			{
				GuardarPaquete();
			}
			else
			{
				CargarPaquete();
			}
		}
	}

	private void GuardarPaquete()
	{
		ST_Datos.Guardar("datos", datos);
	}

	private void CargarPaquete()
	{
		datos = ST_Datos.Cargar<ES_Datos>("datos");
	}

	public bool Consultar_Skin_Desbloqueada(int index, TipoSkin skin)
	{
		switch (skin)
		{
		case TipoSkin.Gorro:
			return datos.skinsGorro_Desbloqueado[index];
		case TipoSkin.Arma:
			return datos.skinsArma_Desbloqueado[index];
		case TipoSkin.Puerta:
			return datos.skinsPuerta_Desbloqueado[index];
		case TipoSkin.Muñeco:
			return datos.skinsMuñeco_Desbloqueado[index];
		case TipoSkin.PantallaCine:
			return datos.skinsPantallaCine_Desbloqueado[index];
		case TipoSkin.Dron:
			return datos.skinsDron_Desbloqueado[index];
		default:
			return false;
		}
	}

	public bool Consultar_Seccion_Desbloqueada(int tipoSkin)
	{
		bool result = true;
		switch ((TipoSkin)tipoSkin)
		{
		case TipoSkin.Gorro:
		{
			bool[] skinsDron_Desbloqueado = datos.skinsGorro_Desbloqueado;
			for (int i = 0; i < skinsDron_Desbloqueado.Length; i++)
			{
				if (!skinsDron_Desbloqueado[i])
				{
					result = false;
					break;
				}
			}
			break;
		}
		case TipoSkin.Arma:
		{
			bool[] skinsDron_Desbloqueado = datos.skinsArma_Desbloqueado;
			for (int i = 0; i < skinsDron_Desbloqueado.Length; i++)
			{
				if (!skinsDron_Desbloqueado[i])
				{
					result = false;
					break;
				}
			}
			break;
		}
		case TipoSkin.Puerta:
		{
			bool[] skinsDron_Desbloqueado = datos.skinsPuerta_Desbloqueado;
			for (int i = 0; i < skinsDron_Desbloqueado.Length; i++)
			{
				if (!skinsDron_Desbloqueado[i])
				{
					result = false;
					break;
				}
			}
			break;
		}
		case TipoSkin.Muñeco:
		{
			bool[] skinsDron_Desbloqueado = datos.skinsMuñeco_Desbloqueado;
			for (int i = 0; i < skinsDron_Desbloqueado.Length; i++)
			{
				if (!skinsDron_Desbloqueado[i])
				{
					result = false;
					break;
				}
			}
			break;
		}
		case TipoSkin.PantallaCine:
		{
			bool[] skinsDron_Desbloqueado = datos.skinsPantallaCine_Desbloqueado;
			for (int i = 0; i < skinsDron_Desbloqueado.Length; i++)
			{
				if (!skinsDron_Desbloqueado[i])
				{
					result = false;
					break;
				}
			}
			break;
		}
		case TipoSkin.Dron:
		{
			bool[] skinsDron_Desbloqueado = datos.skinsDron_Desbloqueado;
			for (int i = 0; i < skinsDron_Desbloqueado.Length; i++)
			{
				if (!skinsDron_Desbloqueado[i])
				{
					result = false;
					break;
				}
			}
			break;
		}
		}
		return result;
	}

	public int Consultar_Skin_Seleccionada(TipoSkin skin)
	{
		switch (skin)
		{
		case TipoSkin.Gorro:
			return datos.skinsGorro_Seleccionado;
		case TipoSkin.Arma:
			return datos.skinsArma_Seleccionado;
		case TipoSkin.Puerta:
			return datos.skinsPuerta_Seleccionado;
		case TipoSkin.Muñeco:
			return datos.skinsMuñeco_Seleccionado;
		case TipoSkin.PantallaCine:
			return datos.skinsPantallaCine_Seleccionado;
		case TipoSkin.Dron:
			return datos.skinsDron_Seleccionado;
		default:
			return 0;
		}
	}

	public bool Consultar_Todas_Las_Skins_Desbloqueadas()
	{
		if (ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Seccion_Desbloqueada(0) && ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Seccion_Desbloqueada(1) && ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Seccion_Desbloqueada(2) && ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Seccion_Desbloqueada(3) && ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Seccion_Desbloqueada(4))
		{
			return ES_EstadoJuego.estadoJuego.DatosControlador.Consultar_Seccion_Desbloqueada(5);
		}
		return false;
	}

	public void Registrar_Skin(int index, TipoSkin skin, Accion accion)
	{
		switch (skin)
		{
		case TipoSkin.Gorro:
			if (accion == Accion.Desbloquear)
			{
				datos.skinsGorro_Desbloqueado[index] = true;
			}
			if (accion == Accion.Seleccionar)
			{
				datos.skinsGorro_Seleccionado = index;
			}
			break;
		case TipoSkin.Arma:
			if (accion == Accion.Desbloquear)
			{
				datos.skinsArma_Desbloqueado[index] = true;
			}
			if (accion == Accion.Seleccionar)
			{
				datos.skinsArma_Seleccionado = index;
			}
			break;
		case TipoSkin.Puerta:
			if (accion == Accion.Desbloquear)
			{
				datos.skinsPuerta_Desbloqueado[index] = true;
			}
			if (accion == Accion.Seleccionar)
			{
				datos.skinsPuerta_Seleccionado = index;
			}
			break;
		case TipoSkin.Muñeco:
			if (accion == Accion.Desbloquear)
			{
				datos.skinsMuñeco_Desbloqueado[index] = true;
			}
			if (accion == Accion.Seleccionar)
			{
				datos.skinsMuñeco_Seleccionado = index;
			}
			break;
		case TipoSkin.PantallaCine:
			if (accion == Accion.Desbloquear)
			{
				datos.skinsPantallaCine_Desbloqueado[index] = true;
			}
			if (accion == Accion.Seleccionar)
			{
				datos.skinsPantallaCine_Seleccionado = index;
			}
			break;
		case TipoSkin.Dron:
			if (accion == Accion.Desbloquear)
			{
				datos.skinsDron_Desbloqueado[index] = true;
			}
			if (accion == Accion.Seleccionar)
			{
				datos.skinsDron_Seleccionado = index;
			}
			break;
		}
		GuardarPaquete();
	}

	public void Registrar_Seccion_Skin(TipoSkin skin)
	{
		switch (skin)
		{
		case TipoSkin.Gorro:
		{
			for (int l = 0; l < datos.skinsGorro_Desbloqueado.Length; l++)
			{
				datos.skinsGorro_Desbloqueado[l] = true;
			}
			break;
		}
		case TipoSkin.Arma:
		{
			for (int n = 0; n < datos.skinsArma_Desbloqueado.Length; n++)
			{
				datos.skinsArma_Desbloqueado[n] = true;
			}
			break;
		}
		case TipoSkin.Puerta:
		{
			for (int j = 0; j < datos.skinsPuerta_Desbloqueado.Length; j++)
			{
				datos.skinsPuerta_Desbloqueado[j] = true;
			}
			break;
		}
		case TipoSkin.Muñeco:
		{
			for (int m = 0; m < datos.skinsMuñeco_Desbloqueado.Length; m++)
			{
				datos.skinsMuñeco_Desbloqueado[m] = true;
			}
			break;
		}
		case TipoSkin.PantallaCine:
		{
			for (int k = 0; k < datos.skinsPantallaCine_Desbloqueado.Length; k++)
			{
				datos.skinsPantallaCine_Desbloqueado[k] = true;
			}
			break;
		}
		case TipoSkin.Dron:
		{
			for (int i = 0; i < datos.skinsDron_Desbloqueado.Length; i++)
			{
				datos.skinsDron_Desbloqueado[i] = true;
			}
			break;
		}
		}
		GuardarPaquete();
	}

	public void Registrar_Todas_Las_Skins()
	{
		for (int i = 0; i < datos.skinsGorro_Desbloqueado.Length; i++)
		{
			datos.skinsGorro_Desbloqueado[i] = true;
		}
		for (int j = 0; j < datos.skinsArma_Desbloqueado.Length; j++)
		{
			datos.skinsArma_Desbloqueado[j] = true;
		}
		for (int k = 0; k < datos.skinsPuerta_Desbloqueado.Length; k++)
		{
			datos.skinsPuerta_Desbloqueado[k] = true;
		}
		for (int l = 0; l < datos.skinsMuñeco_Desbloqueado.Length; l++)
		{
			datos.skinsMuñeco_Desbloqueado[l] = true;
		}
		for (int m = 0; m < datos.skinsPantallaCine_Desbloqueado.Length; m++)
		{
			datos.skinsPantallaCine_Desbloqueado[m] = true;
		}
		for (int n = 0; n < datos.skinsDron_Desbloqueado.Length; n++)
		{
			datos.skinsDron_Desbloqueado[n] = true;
		}
		GuardarPaquete();
	}

	public void Activar_Extra_Unicamente(int identificador, TipoOtro tipo)
	{
		switch (tipo)
		{
		case TipoOtro.Poder:
			extrasActivados.poder_Activado = identificador;
			break;
		case TipoOtro.HuevoDeOro:
			extrasActivados.huevoDeOro_Activado = identificador;
			break;
		}
	}
}
