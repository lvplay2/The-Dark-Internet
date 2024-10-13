using System;
using GoogleMobileAds.Api;

public class Interstitial
{
	public delegate void SimpleDelegate();

	public delegate void MessageDelegate(string message);

	private string adUnitID;

	private bool useEmulatorAsATestDevice;

	private string[] testDeviceIDs;

	private string[] keywords;

	private Gender? gender;

	private bool? childDirectedTreatment;

	private InterstitialAd interstitial;

	private bool failedLoading;

	private bool anunciosPersonalizados;

	public SimpleDelegate OnAdClosed;

	public MessageDelegate OnAdFailedToLoad;

	public SimpleDelegate OnAdLeftApplication;

	public SimpleDelegate OnAdLoaded;

	public SimpleDelegate OnAdOpened;

	public Interstitial(string adUnitID, bool useEmulatorAsATestDevice, string[] testDeviceIDs, string[] keywords, Gender? gender, bool? childDirectedTreatment, bool anunciosPersonalizados)
	{
		this.adUnitID = adUnitID;
		this.useEmulatorAsATestDevice = useEmulatorAsATestDevice;
		this.testDeviceIDs = testDeviceIDs;
		this.keywords = keywords;
		this.gender = gender;
		this.childDirectedTreatment = childDirectedTreatment;
		this.anunciosPersonalizados = anunciosPersonalizados;
		BuildInterstitial();
	}

	public void BuildInterstitial()
	{
		failedLoading = false;
		interstitial = new InterstitialAd(adUnitID);
		interstitial.OnAdClosed += HandleInterstitialClosed;
		interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
		interstitial.OnAdLeavingApplication += HandleLeftApplication;
		interstitial.OnAdLoaded += HandleLoaded;
		interstitial.OnAdOpening += HandleOpened;
		AdRequest.Builder builder = new AdRequest.Builder();
		if (!anunciosPersonalizados)
		{
			builder.AddExtra("npa", "1");
		}
		if (useEmulatorAsATestDevice)
		{
			builder.AddTestDevice("SIMULATOR");
		}
		if (testDeviceIDs != null && testDeviceIDs.Length != 0)
		{
			string[] array = testDeviceIDs;
			foreach (string deviceId in array)
			{
				builder.AddTestDevice(deviceId);
			}
		}
		if (keywords != null && keywords.Length != 0)
		{
			string[] array = keywords;
			foreach (string keyword in array)
			{
				builder.AddKeyword(keyword);
			}
		}
		if (gender.HasValue)
		{
			builder.SetGender(gender.Value);
		}
		if (childDirectedTreatment.HasValue)
		{
			builder.TagForChildDirectedTreatment(childDirectedTreatment.Value);
		}
		AdRequest request = builder.Build();
		interstitial.LoadAd(request);
	}

	private void HandleInterstitialClosed(object sender, EventArgs args)
	{
		if (OnAdClosed != null)
		{
			OnAdClosed();
		}
		RebuildInterstitial();
	}

	private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		if (OnAdFailedToLoad != null)
		{
			OnAdFailedToLoad(args.Message);
		}
		failedLoading = true;
	}

	private void HandleLeftApplication(object sender, EventArgs args)
	{
		if (OnAdLeftApplication != null)
		{
			OnAdLeftApplication();
		}
	}

	private void HandleLoaded(object sender, EventArgs args)
	{
		if (OnAdLoaded != null)
		{
			OnAdLoaded();
		}
	}

	private void HandleOpened(object sender, EventArgs args)
	{
		if (OnAdOpened != null)
		{
			OnAdOpened();
		}
	}

	public void RebuildInterstitial()
	{
		if (interstitial != null)
		{
			interstitial.Destroy();
			interstitial = null;
		}
		BuildInterstitial();
	}

	public void Show()
	{
		if (failedLoading)
		{
			RebuildInterstitial();
		}
		else if (interstitial != null && interstitial.IsLoaded())
		{
			interstitial.Show();
		}
	}
}
