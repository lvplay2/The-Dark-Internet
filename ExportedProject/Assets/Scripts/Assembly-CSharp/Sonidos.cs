using UnityEngine;

public class Sonidos : MonoBehaviour
{
	public static Sonidos sonidos;

	[Header("Sonidos")]
	public AudioClip puerta_abriendose;

	public AudioClip puerta_cerrandose;

	public AudioClip cajon_abriendose;

	public AudioClip metal_1;

	public AudioClip metal_2;

	public AudioClip suspenso_1;

	public AudioClip voces_1;

	public AudioClip agarrarObjeto_1;

	public AudioClip baulAbriendose;

	public AudioClip colocarObjeto;

	public AudioClip abrirCajaFuerte;

	public AudioClip autoDeJuguete;

	public AudioClip maquina_comenzar;

	public AudioClip maquina_ganaste;

	public AudioClip maquina_ingresar_moneda;

	public AudioClip maquina_loop_mover;

	public AudioClip maquina_musica;

	public AudioClip marcador_girar;

	public AudioClip puerta_cerrada;

	public AudioClip puerta_metal_abrir;

	public AudioClip expulsar_muñeco;

	public AudioClip escopeta_disparo;

	public AudioClip escopeta_recarga;

	public AudioClip abrir_puerta_pequeña_1;

	public AudioClip acuchillar;

	public AudioClip cuerpo_golpeando_suelo;

	public AudioClip bajar_palanca;

	public AudioClip cerrar_compuerta;

	public AudioClip estas_muerto;

	public AudioClip puerta_abriendose_y_rechinando;

	public AudioClip puerta_cerrandose_2;

	public AudioClip razguñada;

	public AudioClip ronroneo;

	public AudioClip gato_atacar;

	public AudioClip maullido;

	public AudioClip[] piso_cruje;

	public AudioClip[] caida_botella;

	public AudioClip[] caida_metal;

	public AudioClip[] audios_casete;

	[Header("Musica")]
	public AudioSource musicaAmbiente;

	public MS_MusicaPersecucion musicaPersecucion;

	public AudioSource sonidoAmbiente;

	public AudioSource sonidoFinal;

	public AudioSource vocesAmbiente;

	private void Awake()
	{
		sonidos = this;
	}
}
