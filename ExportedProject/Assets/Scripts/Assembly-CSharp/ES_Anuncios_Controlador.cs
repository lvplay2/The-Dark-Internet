using System;

[Serializable]
public class ES_Anuncios_Controlador
{
	public ES_Anuncios anuncios = new ES_Anuncios();

	public ES_Anuncios_Personalizados anunciosPersonalizados = new ES_Anuncios_Personalizados();

	private const string anuncios_activados = "anuncios";

	private const string anuncios_personalizados = "anuncios_personalizados";

	private bool inicializado;

	public void Inicializar()
	{
		if (!inicializado)
		{
			inicializado = true;
			if (!ST_Datos.Existe("anuncios"))
			{
				Guardar_AnunciosActivados();
			}
			else
			{
				Cargar_AnunciosActivados();
			}
			if (!ST_Datos.Existe("anuncios_personalizados"))
			{
				Guardar_AnunciosPersonalizados();
			}
			else
			{
				Cargar_AnunciosPersonalizados();
			}
		}
	}

	private void Guardar_AnunciosActivados()
	{
		ST_Datos.Guardar("anuncios", anuncios);
	}

	private void Cargar_AnunciosActivados()
	{
		anuncios = ST_Datos.Cargar<ES_Anuncios>("anuncios");
	}

	private void Guardar_AnunciosPersonalizados()
	{
		ST_Datos.Guardar("anuncios_personalizados", anunciosPersonalizados);
	}

	private void Cargar_AnunciosPersonalizados()
	{
		anunciosPersonalizados = ST_Datos.Cargar<ES_Anuncios_Personalizados>("anuncios_personalizados");
	}

	public void Establecer_GDPR(bool activado)
	{
		anunciosPersonalizados.activado = activado;
		Guardar_AnunciosPersonalizados();
	}

	public void ComprarAnuncios()
	{
		anuncios.desactivados = true;
		Guardar_AnunciosActivados();
	}
}
