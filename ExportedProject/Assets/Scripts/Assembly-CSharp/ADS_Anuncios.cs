using UnityEngine;

public class ADS_Anuncios : MonoBehaviour
{
	public delegate void VideoRecompensadoCerrado(bool completado);

	public static ADS_Anuncios anuncios;

	public VideoRecompensadoCerrado videoRecompensadoCerrado;

	private void Awake()
	{
		if (anuncios == null)
		{
			anuncios = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else if (anuncios != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		bool activado = ES_EstadoJuego.estadoJuego.AnunciosControlador.anunciosPersonalizados.activado;
		EasyGoogleMobileAds.GetInterstitialManager().AnunciosPersonalizados(activado);
		InicializarIntersticial();
		Advertisements.Instance.SetUserConsent(activado);
		Advertisements.Instance.Initialize();
	}

	private void InicializarIntersticial()
	{
		EasyGoogleMobileAds.GetInterstitialManager().PrepareInterstitial("ca-app-pub-3603623452195128/1783174599");
	}

	public void MostrarIntersticial()
	{
		if (!AnunciosDesactivados())
		{
			EasyGoogleMobileAds.GetInterstitialManager().ShowInterstitial();
		}
	}

	public bool VideoRecompensadoDisponible()
	{
		return Advertisements.Instance.IsRewardVideoAvailable();
	}

	public void MostrarVideoRecompensado()
	{
		Advertisements.Instance.ShowRewardedVideo(_VideoRecompensadoCerrado);
	}

	private bool AnunciosDesactivados()
	{
		return ES_EstadoJuego.estadoJuego.AnunciosControlador.anuncios.desactivados;
	}

	private void _IntersticialCerrado(string advertiser)
	{
		Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
	}

	private void _VideoRecompensadoCerrado(bool completed, string advertiser)
	{
		Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
		VideoRecompensadoCerrado obj = videoRecompensadoCerrado;
		if (obj != null)
		{
			obj(completed);
		}
	}
}
