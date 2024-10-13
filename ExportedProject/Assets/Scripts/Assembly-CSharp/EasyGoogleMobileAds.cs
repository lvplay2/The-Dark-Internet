using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class EasyGoogleMobileAds : MonoBehaviour
{
	public enum Languages
	{
		English = 0,
		Español = 1
	}

	public enum Sizes
	{
		Banner = 0,
		IABBanner = 1,
		Leaderboard = 2,
		MediumRectangle = 3,
		SmartBanner = 4
	}

	public enum TagForChildDirectedTreatment
	{
		NotTagged = 0,
		True = 1,
		False = 2
	}

	public Languages editorLanguage;

	public string adUnitID;

	public string adUnitIDAndroid;

	public string adUnitIDIOS;

	public Sizes adSize;

	public AdPosition adPosition;

	public bool customSize;

	public int customWidth = 320;

	public int customHeight = 50;

	public List<string> testDevices = new List<string>();

	public bool useEmulatorAsATestDevice;

	public string keywords = string.Empty;

	public Gender gender;

	public TagForChildDirectedTreatment tagForChildDirectedTreatment;

	public BannerView bannerView;

	private static InterstitialManager interstitialManager;

	private void OnEnable()
	{
		destroyAd();
		adUnitID = adUnitIDAndroid;
		bannerView = new BannerView(adUnitID, getAdSize(), adPosition);
		bannerView.OnAdLoaded += HandleAdLoaded;
		bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
		bannerView.OnAdOpening += HandleAdOpened;
		bannerView.OnAdClosed += HandleAdClosed;
		bannerView.OnAdLeavingApplication += HandleAdLeftApplication;
		bannerView.LoadAd(getAdRequest());
	}

	private void OnDisable()
	{
		destroyAd();
	}

	private void OnDestroy()
	{
		destroyAd();
	}

	private void destroyAd()
	{
		if (bannerView != null)
		{
			bannerView.Hide();
			bannerView.Destroy();
			bannerView = null;
		}
	}

	private AdRequest getAdRequest()
	{
		AdRequest.Builder builder = new AdRequest.Builder();
		if (useEmulatorAsATestDevice)
		{
			builder.AddTestDevice("SIMULATOR");
		}
		foreach (string testDevice in testDevices)
		{
			if (!string.IsNullOrEmpty(testDevice))
			{
				builder.AddTestDevice(testDevice);
			}
		}
		string[] array = keywords.Split(',');
		foreach (string text in array)
		{
			if (text.Trim() != string.Empty)
			{
				builder.AddKeyword(text.Trim());
			}
		}
		if (gender != 0)
		{
			builder.SetGender(gender);
		}
		if (tagForChildDirectedTreatment != 0)
		{
			builder.TagForChildDirectedTreatment(tagForChildDirectedTreatment == TagForChildDirectedTreatment.True);
		}
		return builder.Build();
	}

	private AdSize getAdSize()
	{
		AdSize result = null;
		if (customSize)
		{
			result = new AdSize(customWidth, customHeight);
		}
		else
		{
			switch (adSize)
			{
			case Sizes.Banner:
				result = AdSize.Banner;
				break;
			case Sizes.IABBanner:
				result = AdSize.IABBanner;
				break;
			case Sizes.Leaderboard:
				result = AdSize.Leaderboard;
				break;
			case Sizes.MediumRectangle:
				result = AdSize.MediumRectangle;
				break;
			case Sizes.SmartBanner:
				result = AdSize.SmartBanner;
				break;
			}
		}
		return result;
	}

	public static InterstitialManager GetInterstitialManager()
	{
		if (interstitialManager == null)
		{
			interstitialManager = new InterstitialManager();
		}
		return interstitialManager;
	}

	public void HandleAdLoaded(object sender, EventArgs args)
	{
		SendMessage("OnAdLoaded", SendMessageOptions.DontRequireReceiver);
	}

	public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		SendMessage("OnAdFailedToLoad", args.Message, SendMessageOptions.DontRequireReceiver);
	}

	public void HandleAdOpened(object sender, EventArgs args)
	{
		SendMessage("OnAdOpened", SendMessageOptions.DontRequireReceiver);
	}

	public void HandleAdClosed(object sender, EventArgs args)
	{
		SendMessage("OnAdClosed", SendMessageOptions.DontRequireReceiver);
	}

	public void HandleAdLeftApplication(object sender, EventArgs args)
	{
		SendMessage("OnAdLeftApplication", SendMessageOptions.DontRequireReceiver);
	}
}
