using UnityEngine;

public class ES_Datos_Contenido : MonoBehaviour
{
	public ES_Skin_Contenedor[] skinsGorro;

	public ES_Skin_Contenedor[] skinsArma;

	public ES_Skin_Contenedor[] skinsPuerta;

	public ES_Skin_Contenedor[] skinsMuñeco;

	public ES_Skin_Contenedor[] skinsPantallaCine;

	public ES_Skin_Contenedor[] skinsDron;

	public ES_Logro_Contenedor[] logros;

	public ES_Skin_Contenedor ObtenerSkin_Contenedor(int index, ES_Datos_Controlador.TipoSkin tipoSkin)
	{
		switch (tipoSkin)
		{
		case ES_Datos_Controlador.TipoSkin.Gorro:
			return skinsGorro[index];
		case ES_Datos_Controlador.TipoSkin.Arma:
			return skinsArma[index];
		case ES_Datos_Controlador.TipoSkin.Puerta:
			return skinsPuerta[index];
		case ES_Datos_Controlador.TipoSkin.Muñeco:
			return skinsMuñeco[index];
		case ES_Datos_Controlador.TipoSkin.PantallaCine:
			return skinsPantallaCine[index];
		case ES_Datos_Controlador.TipoSkin.Dron:
			return skinsDron[index];
		default:
			return null;
		}
	}
}
