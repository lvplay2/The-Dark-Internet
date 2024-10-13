using UnityEngine;

public class EC_Camara : MonoBehaviour
{
	private JG_Jugador jugador;

	private FP_Controller jugadorControlador;

	private FP_Input jugadorInput;

	private JG_Vision vision;

	private Camera camara;

	private Vector2 _rotacionInicial;

	private Vector2 _mirarA;

	private float _sensibilidad;

	private float _inputX;

	private float _inputY;

	private float _xInputDelta;

	private float _zInputDelta;

	private bool _activado;

	private float rotacionX;

	private float rotacionY;

	[Header("Configuración")]
	public float sensibilidad;

	public float minimoX;

	public float maximoX;

	public float minimoY;

	public float maximoY;

	public float suavidad;

	private void Awake()
	{
		ObtenerReferencias();
	}

	private void ObtenerReferencias()
	{
		jugador = Object.FindObjectOfType<JG_Jugador>();
		jugadorControlador = Object.FindObjectOfType<FP_Controller>();
		jugadorInput = Object.FindObjectOfType<FP_Input>();
		vision = Object.FindObjectOfType<JG_Vision>();
		camara = GetComponent<Camera>();
	}

	private void OnEnable()
	{
		Activar();
	}

	private void OnDisable()
	{
		Desactivar();
	}

	public void Activar()
	{
		rotacionX = _rotacionInicial.y;
		rotacionY = 0f - _rotacionInicial.x;
		camara.enabled = true;
		_activado = true;
		ES_EstadoEscena.estadoEscena.camaraPrincipal = camara;
		ES_EstadoEscena.estadoEscena.audioListenerJugador.enabled = false;
	}

	public void Desactivar()
	{
		camara.enabled = false;
		jugadorControlador.inputX = 0f;
		jugadorControlador.inputZ = 0f;
		_activado = false;
		ES_EstadoEscena.estadoEscena.camaraPrincipal = ES_EstadoEscena.estadoEscena.camaraJugador;
		ES_EstadoEscena.estadoEscena.audioListenerJugador.enabled = true;
	}

	private void Update()
	{
		if (_activado)
		{
			vision.ActualizarVision(base.transform);
		}
		bool canControl = jugadorControlador.canControl;
		switch (jugadorInput.UseMobileInput)
		{
		case true:
			_inputX = jugadorInput.LookInput().x + jugadorInput.ShotInput().x;
			_inputY = jugadorInput.LookInput().y + jugadorInput.ShotInput().y;
			break;
		case false:
			_inputX = Input.GetAxis("Mouse X") * 10f;
			_inputY = Input.GetAxis("Mouse Y") * 10f;
			break;
		}
		_sensibilidad = sensibilidad;
		_mirarA.x = Mathf.Lerp(_mirarA.x, _inputX, suavidad * Time.deltaTime);
		_mirarA.y = Mathf.Lerp(_mirarA.y, _inputY, suavidad * Time.deltaTime);
		rotacionX += _mirarA.x * (_sensibilidad / 10f);
		rotacionY += _mirarA.y * (_sensibilidad / 10f);
		if (minimoX != float.PositiveInfinity && maximoX != float.PositiveInfinity)
		{
			rotacionX = Mathf.Clamp(rotacionX, minimoX, maximoX);
		}
		if (minimoY != float.PositiveInfinity && maximoY != float.PositiveInfinity)
		{
			rotacionY = Mathf.Clamp(rotacionY, minimoY, maximoY);
		}
		base.transform.localEulerAngles = new Vector3(0f - rotacionY, rotacionX, 0f);
	}
}
