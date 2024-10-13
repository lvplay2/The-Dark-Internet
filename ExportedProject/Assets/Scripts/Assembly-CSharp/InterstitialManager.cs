using System.Collections.Generic;
using GoogleMobileAds.Api;

public class InterstitialManager
{
	private Dictionary<object, Interstitial> interstitials = new Dictionary<object, Interstitial>();

	private bool useEmulatorAsATestDevice;

	private string[] testDeviceIDs;

	private string[] keywords;

	private Gender? gender;

	private bool? childDirectedTreatment;

	private bool anunciosPersonalizados;

	private object firstKey;

	public InterstitialManager()
	{
		useEmulatorAsATestDevice = false;
		testDeviceIDs = null;
	}

	public void SetTestDevices(bool useEmulatorAsATestDevice, string[] testDeviceIDs)
	{
		this.useEmulatorAsATestDevice = useEmulatorAsATestDevice;
		this.testDeviceIDs = testDeviceIDs;
	}

	public void AnunciosPersonalizados(bool activado)
	{
		anunciosPersonalizados = activado;
	}

	public void SetKeywords(string[] keywords)
	{
		this.keywords = keywords;
	}

	public void SetGender(Gender gender)
	{
		this.gender = gender;
	}

	public void TagForChildDirectedTreatment(bool childDirectedTreatment)
	{
		this.childDirectedTreatment = childDirectedTreatment;
	}

	public void PrepareInterstitial(string adUnitID)
	{
		PrepareInterstitial(adUnitID, adUnitID);
	}

	public void PrepareInterstitial(string adUnitID, object key)
	{
		if (!interstitials.ContainsKey(key))
		{
			interstitials[key] = new Interstitial(adUnitID, useEmulatorAsATestDevice, testDeviceIDs, keywords, gender, childDirectedTreatment, anunciosPersonalizados);
			if (firstKey == null)
			{
				firstKey = key;
			}
		}
	}

	public void ShowInterstitial()
	{
		if (firstKey != null)
		{
			ShowInterstitial(firstKey);
		}
	}

	public void ShowInterstitial(object key)
	{
		Interstitial value;
		if (interstitials.TryGetValue(key, out value))
		{
			value.Show();
		}
	}

	public Interstitial GetInterstitial()
	{
		if (firstKey == null)
		{
			return null;
		}
		return GetInterstitial(firstKey);
	}

	public Interstitial GetInterstitial(object key)
	{
		Interstitial value;
		interstitials.TryGetValue(key, out value);
		return value;
	}
}
