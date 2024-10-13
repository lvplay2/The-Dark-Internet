using System.Collections;
using UnityEngine;

public class PZ_3_Maquina : IT_Interactivo
{
	public JG_Jugador jugador;

	public Transform controlador_H;

	public Transform controlador_V;

	public PZ_3_Gancho gancho;

	public Camera camara;

	public AudioSource audioSource;

	public float velocidad_H;

	public float velocidad_V;

	[Header("Controlador")]
	public float minimo_C;

	public float maximo_C;

	[Header("Controlador")]
	public float minimo_G;

	public float maximo_G;

	[Header("Posicion Salida")]
	public float eje1;

	public float eje2;

	private bool _usando;

	private bool _usandoGancho;

	private bool _moviendoGancho;

	private Coroutine usarGancho;

	private bool _comenzoSonido;

	private bool _loopSonido;

	private string observacion = "Tal vez deba conseguir una moneda para poder utilizarla";

	private int _horizontal;

	private int _vertical;

	private void Start()
	{
		StartCoroutine(Sonido());
	}

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	private IEnumerator Sonido()
	{
		while (true)
		{
			if (_usando)
			{
				if (!_comenzoSonido && (_moviendoGancho || _usandoGancho))
				{
					audioSource.Reproducir(Sonidos.sonidos.maquina_comenzar, 0f, false);
					_comenzoSonido = true;
				}
				else if (_comenzoSonido && (_moviendoGancho || _usandoGancho))
				{
					if (!audioSource.isPlaying)
					{
						audioSource.Reproducir(Sonidos.sonidos.maquina_loop_mover, audioSource.volume, true);
					}
					audioSource.volume = Mathf.Lerp(audioSource.volume, 0.2f, 7f * Time.deltaTime);
				}
				else if (!_moviendoGancho && !_usandoGancho)
				{
					audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, 7f * Time.deltaTime);
				}
			}
			else
			{
				if (!audioSource.isPlaying)
				{
					audioSource.Reproducir(Sonidos.sonidos.maquina_loop_mover, audioSource.volume, true);
				}
				if (!_usandoGancho)
				{
					audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, 3f * Time.deltaTime);
				}
			}
			yield return null;
		}
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		switch (accion)
		{
		case Acciones.Recoger:
			if (!_usando)
			{
				if (IT_Cartera.cartera.Contiene<PZ_3_Moneda>())
				{
					Usar();
				}
				else
				{
					UI_Canvas.canvas.observacion.Observar(observacion);
				}
			}
			break;
		case Acciones.Salir:
			if (_usando)
			{
				DejarDeUsar();
			}
			break;
		}
		if (_usandoGancho)
		{
			return;
		}
		switch (accion)
		{
		case Acciones.MoverAbajo:
			_horizontal = ((!seSolto) ? (-1) : 0);
			break;
		case Acciones.MoverArriba:
			_horizontal = ((!seSolto) ? 1 : 0);
			break;
		case Acciones.MoverIzquierda:
			_vertical = ((!seSolto) ? 1 : 0);
			break;
		case Acciones.MoverDerecha:
			_vertical = ((!seSolto) ? (-1) : 0);
			break;
		case Acciones.BajarGancho:
			if (!_usandoGancho)
			{
				usarGancho = StartCoroutine(UsarGancho());
			}
			break;
		case Acciones.SinFlechas:
			_comenzoSonido = false;
			_moviendoGancho = false;
			break;
		}
	}

	private void Update()
	{
		if (_usandoGancho)
		{
			_horizontal = 0;
			_vertical = 0;
			return;
		}
		controlador_H.localPosition += new Vector3((float)_horizontal * velocidad_H * Time.deltaTime, 0f, 0f);
		controlador_H.localPosition = new Vector3(Mathf.Clamp(controlador_H.localPosition.x, minimo_C, maximo_C), controlador_H.localPosition.y, controlador_H.localPosition.z);
		controlador_V.localPosition += new Vector3((float)_vertical * velocidad_V * Time.deltaTime, 0f, 0f);
		controlador_V.localPosition = new Vector3(Mathf.Clamp(controlador_V.localPosition.x, minimo_C, maximo_C), controlador_V.localPosition.y, controlador_V.localPosition.z);
	}

	private void Usar()
	{
		IT_Interactivo.AsignarAcciones(new Acciones[6]
		{
			Acciones.MoverArriba,
			Acciones.MoverAbajo,
			Acciones.MoverIzquierda,
			Acciones.MoverDerecha,
			Acciones.BajarGancho,
			Acciones.Salir
		});
		PZ_3_Moneda obj = (PZ_3_Moneda)IT_Cartera.cartera.ElementoEnCartera;
		((IT_Recogible)IT_Cartera.cartera.ElementoEnCartera).Soltar(IT_Recogible.Caida.EjercerFuerza);
		Object.Destroy(obj.gameObject);
		jugador.gameObject.SetActive(false);
		camara.enabled = true;
		IT_Cartera.cartera.Asignar_ElementoEnUso(this);
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.maquina_ingresar_moneda, base.transform.position, 0.2f, ES_EstadoEscena.estadoEscena.audioGlobal);
		_usando = true;
	}

	private void DejarDeUsar()
	{
		IT_Interactivo.AsignarAcciones(IT_Interactivo.AccionesPredeterminadas);
		Object.FindObjectOfType<FP_Joystick>().ReseteaarJoystick();
		jugador.gameObject.SetActive(true);
		camara.enabled = false;
		IT_Cartera.cartera.Vaciar();
		_usando = false;
	}

	private IEnumerator UsarGancho()
	{
		_usandoGancho = true;
		bool llego = false;
		while (!llego)
		{
			gancho.transform.localPosition += new Vector3(0f, (0f - velocidad_V) * Time.deltaTime, 0f);
			if (gancho.transform.localPosition.y < minimo_G)
			{
				gancho.transform.localPosition = new Vector3(gancho.transform.localPosition.x, minimo_G, gancho.transform.localPosition.z);
				llego = true;
			}
			yield return null;
		}
		llego = false;
		yield return new WaitForSeconds(0.3f);
		gancho.CerrarGancho();
		yield return new WaitForSeconds(0.5f);
		gancho.IntentarRecoger();
		while (!llego)
		{
			gancho.transform.localPosition += new Vector3(0f, velocidad_V * Time.deltaTime, 0f);
			if (gancho.transform.localPosition.y > maximo_G)
			{
				gancho.transform.localPosition = new Vector3(gancho.transform.localPosition.x, maximo_G, gancho.transform.localPosition.z);
				llego = true;
			}
			yield return null;
		}
		yield return new WaitForSeconds(0.4f);
		Transform _eje1 = controlador_H;
		Transform _eje2 = controlador_V;
		int _direccion_inicial_1 = CalcularDireccion(_eje1.localPosition.x, eje1);
		int _direccion_inicial_2 = CalcularDireccion(_eje2.localPosition.x, eje2);
		bool _llego_1 = false;
		bool _llego_2 = false;
		while (!_llego_1 || !_llego_2)
		{
			int num = CalcularDireccion(_eje1.localPosition.x, eje1);
			int num2 = CalcularDireccion(_eje2.localPosition.x, eje2);
			if (!_llego_1)
			{
				if (num != 0 && num == _direccion_inicial_1)
				{
					_eje1.localPosition += new Vector3(velocidad_H * (float)num * Time.deltaTime, 0f, 0f);
				}
				else
				{
					_eje1.localPosition = new Vector3(eje1, _eje1.localPosition.y, _eje1.localPosition.z);
					_llego_1 = true;
				}
			}
			if (!_llego_2)
			{
				if (num2 != 0 && num2 == _direccion_inicial_2)
				{
					_eje2.localPosition += new Vector3(velocidad_H * (float)num2 * Time.deltaTime, 0f, 0f);
				}
				else
				{
					_eje2.localPosition = new Vector3(eje2, _eje2.localPosition.y, _eje2.localPosition.z);
					_llego_2 = true;
				}
			}
			yield return null;
		}
		yield return new WaitForSeconds(0.3f);
		gancho.Soltar();
		_usandoGancho = false;
		usarGancho = null;
	}

	private int CalcularDireccion(float actual, float objetivo)
	{
		if (!(actual < objetivo))
		{
			if (!(actual > objetivo))
			{
				return 0;
			}
			return -1;
		}
		return 1;
	}
}
