using UnityEngine;

public class IT_Cajon : IT_Interactivo
{
	public enum Eje
	{
		X = 0,
		Y = 1,
		Z = 2
	}

	[Header("Configuración")]
	public Eje eje = Eje.Y;

	public float recorrido;

	public bool positivo = true;

	private const float _velocidad = 2f;

	private bool _abierto;

	private Vector3 _posicionInicial;

	public OJ_Posicion posicion;

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (posicion != null)
		{
			posicion.Interaccionar();
		}
		if (!seSolto && accion == Acciones.Recoger)
		{
			_abierto = !_abierto;
			ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.cajon_abriendose, base.transform.position, 0.6f, ES_EstadoEscena.estadoEscena.audioGlobal);
		}
	}

	private void Start()
	{
		_posicionInicial = base.transform.localPosition;
	}

	private void Update()
	{
		Vector3 b = (_abierto ? ObtenerPosicion() : _posicionInicial);
		base.transform.localPosition = Vector3.Slerp(base.transform.localPosition, b, 2f * Time.deltaTime);
	}

	private Vector3 ObtenerPosicion()
	{
		float num = (positivo ? recorrido : (0f - recorrido));
		switch (eje)
		{
		case Eje.X:
			return new Vector3(_posicionInicial.x + num, _posicionInicial.y, _posicionInicial.z);
		case Eje.Y:
			return new Vector3(_posicionInicial.x, _posicionInicial.y + num, _posicionInicial.z);
		case Eje.Z:
			return new Vector3(_posicionInicial.x, _posicionInicial.y, _posicionInicial.z + num);
		default:
			return Vector3.zero;
		}
	}
}
