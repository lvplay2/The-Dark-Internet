using System;

[Serializable]
public class ES_Datos
{
	public readonly int skinsPorObjeto = 10;

	public bool[] skinsGorro_Desbloqueado = new bool[10];

	public bool[] skinsArma_Desbloqueado = new bool[10];

	public bool[] skinsPuerta_Desbloqueado = new bool[10];

	public bool[] skinsMuñeco_Desbloqueado = new bool[10];

	public bool[] skinsPantallaCine_Desbloqueado = new bool[10];

	public bool[] skinsDron_Desbloqueado = new bool[10];

	public int skinsGorro_Seleccionado;

	public int skinsArma_Seleccionado;

	public int skinsPuerta_Seleccionado;

	public int skinsMuñeco_Seleccionado;

	public int skinsPantallaCine_Seleccionado;

	public int skinsDron_Seleccionado;

	public bool[] cartas_Desbloqueado = new bool[300];

	public ES_Datos()
	{
		skinsGorro_Desbloqueado[0] = true;
		skinsGorro_Seleccionado = 0;
		skinsArma_Desbloqueado[0] = true;
		skinsArma_Seleccionado = 0;
		skinsPuerta_Desbloqueado[0] = true;
		skinsPuerta_Seleccionado = 0;
		skinsMuñeco_Desbloqueado[0] = true;
		skinsMuñeco_Seleccionado = 0;
		skinsPantallaCine_Desbloqueado[0] = true;
		skinsPantallaCine_Seleccionado = 0;
		skinsDron_Desbloqueado[0] = true;
		skinsDron_Seleccionado = 0;
	}
}
