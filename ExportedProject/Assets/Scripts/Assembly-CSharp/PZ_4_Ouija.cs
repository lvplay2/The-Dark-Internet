using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZ_4_Ouija : IT_Interactivo
{
	[Header("Trozos")]
	public PZ_4_TrozoOuija[] trozosScript;

	public GameObject[] trozosTablero;

	[Header("Letras y Numeros")]
	public Transform visorOuija;

	public List<Transform> letras_1;

	public List<Transform> letras_2;

	public List<Transform> numeros;

	private List<Transform> _letras_1;

	private List<Transform> _letras_2;

	private List<Transform> _numeros;

	[Header("Configuración")]
	public float tiempoMovimiento;

	public float tiempoSobreLetra;

	[Header("Puzle Puerta")]
	public PZ_4_Puerta puerta;

	private bool _completado;

	private int _trozosColocados;

	private Coroutine recorrerTablero;

	private bool _recorriendo;

	private string observacion = "Al parecer esta ouija esta incompleta";

	private void Start()
	{
		for (int i = 0; i < trozosScript.Length; i++)
		{
			trozosScript[i].numeroTrozo = i;
		}
		_letras_1 = new List<Transform>(letras_1);
		_letras_2 = new List<Transform>(letras_2);
		_numeros = new List<Transform>(numeros);
		_letras_1.Mezclar();
		_letras_2.Mezclar();
		_numeros.Mezclar();
		puerta.codigo = _letras_1[0].name + _letras_2[0].name + _numeros[0].name;
		Debug.Log(puerta.codigo);
	}

	private void OnEnable()
	{
		base.VisibleParaMano = true;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion != 0)
		{
			return;
		}
		if (!_completado)
		{
			if (IT_Cartera.cartera.Contiene<PZ_4_TrozoOuija>())
			{
				AsignarTrozo();
			}
			else
			{
				UI_Canvas.canvas.observacion.Observar(observacion);
			}
		}
		else if (!_recorriendo)
		{
			recorrerTablero = StartCoroutine(RecorrerTablero());
		}
	}

	private void AsignarTrozo()
	{
		PZ_4_TrozoOuija pZ_4_TrozoOuija = (PZ_4_TrozoOuija)IT_Cartera.cartera.ElementoEnCartera;
		ActivarTrozo(pZ_4_TrozoOuija.numeroTrozo);
		((IT_Recogible)IT_Cartera.cartera.ElementoEnCartera).Soltar(IT_Recogible.Caida.EjercerFuerza);
		pZ_4_TrozoOuija.gameObject.SetActive(false);
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.colocarObjeto, base.transform.position, 0.3f, ES_EstadoEscena.estadoEscena.audioGlobal);
		if (_trozosColocados == trozosTablero.Length)
		{
			OuijaCompletada();
		}
	}

	public void ActivarTrozo(int trozo)
	{
		trozosTablero[trozo].SetActive(true);
		_trozosColocados++;
	}

	private void OuijaCompletada()
	{
		_completado = true;
	}

	private IEnumerator RecorrerTablero()
	{
		_recorriendo = true;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.voces_1, base.transform.position, 0.15f, ES_EstadoEscena.estadoEscena.audioGlobal);
		Transform[] posiciones = new Transform[3]
		{
			_letras_1[0],
			_letras_2[0],
			_numeros[0]
		};
		float tiempo = 0f;
		int index = 0;
		while (true)
		{
			Vector3 posicion = visorOuija.localPosition;
			Quaternion rotacion = visorOuija.localRotation;
			while (tiempo < 1f)
			{
				visorOuija.localPosition = Vector3.Lerp(posicion, posiciones[index].localPosition, tiempo);
				visorOuija.localRotation = Quaternion.Lerp(rotacion, posiciones[index].localRotation, tiempo);
				tiempo += Time.deltaTime / tiempoMovimiento;
				yield return null;
			}
			tiempo = 0f;
			index++;
			if (index == posiciones.Length)
			{
				break;
			}
			yield return new WaitForSeconds(tiempoSobreLetra);
		}
		_recorriendo = false;
		recorrerTablero = null;
	}
}
