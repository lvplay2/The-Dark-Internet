using System.Collections;
using UnityEngine;

public class PZ_4_Marcador : IT_Interactivo
{
	public enum Tipo
	{
		Letra1 = 0,
		Letra2 = 1,
		Numero = 2
	}

	public PZ_4_Puerta puerta;

	public AudioSource audioSource;

	public Tipo tipo;

	[HideInInspector]
	public int digito;

	[HideInInspector]
	public bool completado;

	private char[] digitos_letras_1 = new char[6] { 'A', 'B', 'C', 'D', 'E', 'F' };

	private char[] digitos_letras_2 = new char[6] { 'R', 'S', 'T', 'U', 'V', 'W' };

	private char[] digitos_numeros = new char[5] { '1', '2', '3', '4', '5' };

	private float grados = 60f;

	private float tiempoDeGiro = 0.5f;

	private bool _girando;

	private bool _girandoConstantemente;

	private void Update()
	{
		base.VisibleParaMano = !completado;
	}

	public override void Interaccionar(Acciones accion, bool seSolto)
	{
		base.Interaccionar(accion, seSolto);
		if (accion == Acciones.Recoger && !_girando && !completado)
		{
			StartCoroutine(Girar());
		}
	}

	public char ObtenerDigito()
	{
		switch (tipo)
		{
		case Tipo.Letra1:
			return digitos_letras_1[digito];
		case Tipo.Letra2:
			return digitos_letras_2[digito];
		case Tipo.Numero:
			return digitos_numeros[digito];
		default:
			return '.';
		}
	}

	public void GirarConstantemente(int sentido)
	{
		if (!_girandoConstantemente)
		{
			StartCoroutine(Girar_C(sentido));
		}
	}

	private IEnumerator Girar()
	{
		_girando = true;
		ST_Audio.audio.ReproducirAudioEnPosicion(Sonidos.sonidos.marcador_girar, base.transform.position, 0.175f, ES_EstadoEscena.estadoEscena.audioGlobal);
		float tiempo = 0f;
		Quaternion rotacionActual = base.transform.localRotation;
		Quaternion rotacionFinal = rotacionActual * Quaternion.Euler(new Vector3(grados, 0f, 0f));
		while (tiempo < 1f)
		{
			base.transform.localRotation = Quaternion.Lerp(rotacionActual, rotacionFinal, tiempo);
			tiempo += Time.deltaTime / tiempoDeGiro;
			yield return null;
		}
		base.transform.localRotation = rotacionFinal;
		digito = ((digito != digitos_letras_1.Length - 1) ? (digito + 1) : 0);
		puerta.ComprobarCodigo();
		_girando = false;
	}

	private IEnumerator Girar_C(int sentido)
	{
		_girandoConstantemente = true;
		audioSource.Play();
		while (true)
		{
			base.transform.Rotate(Vector3.right * grados * (Time.deltaTime / (tiempoDeGiro * 0.8f)) * sentido);
			yield return null;
		}
	}
}
