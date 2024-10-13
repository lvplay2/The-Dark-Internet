using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AppLovin
{
	public const float AD_POSITION_CENTER = -10000f;

	public const float AD_POSITION_LEFT = -20000f;

	public const float AD_POSITION_RIGHT = -30000f;

	public const float AD_POSITION_TOP = -40000f;

	public const float AD_POSITION_BOTTOM = -50000f;

	private const char _InternalPrimarySeparator = '\u001c';

	private const char _InternalSecondarySeparator = '\u001d';

	public AndroidJavaClass applovinFacade = new AndroidJavaClass("com.applovin.sdk.unity.AppLovinFacade");

	public AndroidJavaObject currentActivity;

	public static AppLovin DefaultPlugin;

	public static AppLovin getDefaultPlugin()
	{
		if (DefaultPlugin == null)
		{
			DefaultPlugin = new AppLovin(new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"));
		}
		return DefaultPlugin;
	}

	public AppLovin(AndroidJavaObject activity)
	{
		if (activity == null)
		{
			throw new MissingReferenceException("No parent activity specified");
		}
		currentActivity = activity;
	}

	public AppLovin()
	{
	}

	public void initializeSdk()
	{
		applovinFacade.CallStatic("InitializeSdk", currentActivity);
	}

	public void showAd(string zoneId = null)
	{
		applovinFacade.CallStatic("ShowAd", currentActivity, zoneId);
	}

	public void showInterstitial(string placement = null)
	{
		applovinFacade.CallStatic("ShowInterstitial", currentActivity, placement);
	}

	public void showInterstitialForZoneId(string zoneId = null)
	{
		applovinFacade.CallStatic("ShowInterstitialForZoneId", currentActivity, zoneId);
	}

	public void hideAd()
	{
		applovinFacade.CallStatic("HideAd", currentActivity);
	}

	public void setAdPosition(float x, float y)
	{
		applovinFacade.CallStatic("SetAdPosition", currentActivity, x, y);
	}

	public void setAdWidth(int width)
	{
		applovinFacade.CallStatic("SetAdWidth", currentActivity, width);
	}

	public void setVerboseLoggingOn(string verboseLoggingOn)
	{
		applovinFacade.CallStatic("SetVerboseLoggingOn", verboseLoggingOn);
	}

	private void setMuted(string muted)
	{
		applovinFacade.CallStatic("SetMuted", muted);
	}

	private bool isMuted()
	{
		return bool.Parse(applovinFacade.CallStatic<string>("IsMuted", Array.Empty<object>()));
	}

	private void setTestAdsEnabled(string enabled)
	{
		applovinFacade.CallStatic("SetTestAdsEnabled", enabled);
	}

	private bool isTestAdsEnabled()
	{
		return bool.Parse(applovinFacade.CallStatic<string>("IsTestAdsEnabled", Array.Empty<object>()));
	}

	public void setSdkKey(string sdkKey)
	{
		applovinFacade.CallStatic("SetSdkKey", currentActivity, sdkKey);
	}

	public void preloadInterstitial(string zoneId = null)
	{
		applovinFacade.CallStatic("PreloadInterstitial", currentActivity, zoneId);
	}

	public bool hasPreloadedInterstitial(string zoneId = null)
	{
		return bool.Parse(applovinFacade.CallStatic<string>("IsInterstitialReady", new object[2] { currentActivity, zoneId }));
	}

	public bool isInterstitialShowing()
	{
		return bool.Parse(applovinFacade.CallStatic<string>("IsInterstitialShowing", new object[1] { currentActivity }));
	}

	public void setAdListener(string gameObjectToNotify)
	{
		applovinFacade.CallStatic("SetUnityAdListener", gameObjectToNotify);
	}

	public void setRewardedVideoUsername(string username)
	{
		applovinFacade.CallStatic("SetIncentivizedUsername", currentActivity, username);
	}

	public void loadIncentInterstitial(string zoneId = null)
	{
		applovinFacade.CallStatic("LoadIncentInterstitial", currentActivity, zoneId);
	}

	public void showIncentInterstitial(string placement = null)
	{
		applovinFacade.CallStatic("ShowIncentInterstitial", currentActivity, placement);
	}

	public void showIncentInterstitialForZoneId(string zoneId = null)
	{
		applovinFacade.CallStatic("ShowIncentInterstitialForZoneId", currentActivity, zoneId);
	}

	public bool isIncentInterstitialReady(string zoneId = null)
	{
		return bool.Parse(applovinFacade.CallStatic<string>("IsIncentReady", new object[2] { currentActivity, zoneId }));
	}

	public bool isPreloadedInterstitialVideo()
	{
		return bool.Parse(applovinFacade.CallStatic<string>("IsCurrentInterstitialVideo", new object[1] { currentActivity }));
	}

	public void trackEvent(string eventType, IDictionary<string, string> parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (parameters != null)
		{
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				if (parameter.Key != null && parameter.Value != null)
				{
					stringBuilder.Append(parameter.Key);
					stringBuilder.Append('\u001d');
					stringBuilder.Append(parameter.Value);
					stringBuilder.Append('\u001c');
				}
			}
		}
		applovinFacade.CallStatic("TrackEvent", currentActivity, eventType, stringBuilder.ToString());
	}

	public void enableImmersiveMode()
	{
		applovinFacade.CallStatic("EnableImmersiveMode", currentActivity);
	}

	private void setHasUserConsent(string hasUserConsent)
	{
		applovinFacade.CallStatic("SetHasUserConsent", hasUserConsent, currentActivity);
	}

	private bool hasUserConsent()
	{
		return bool.Parse(applovinFacade.CallStatic<string>("HasUserConsent", new object[1] { currentActivity }));
	}

	private void setIsAgeRestrictedUser(string isAgeRestrictedUser)
	{
		applovinFacade.CallStatic("SetIsAgeRestrictedUser", isAgeRestrictedUser, currentActivity);
	}

	private bool isAgeRestrictedUser()
	{
		return bool.Parse(applovinFacade.CallStatic<string>("IsAgeRestrictedUser", new object[1] { currentActivity }));
	}

	public static void ShowAd(string zoneId = null)
	{
		getDefaultPlugin().showAd(zoneId);
	}

	public static void ShowAd(float x, float y)
	{
		SetAdPosition(x, y);
		ShowAd();
	}

	public static void ShowInterstitial()
	{
		getDefaultPlugin().showInterstitial();
	}

	public static void ShowInterstitial(string placement)
	{
		getDefaultPlugin().showInterstitial(placement);
	}

	public static void ShowInterstitialForZoneId(string zoneId)
	{
		getDefaultPlugin().showInterstitialForZoneId(zoneId);
	}

	public static void LoadRewardedInterstitial(string zoneId = null)
	{
		getDefaultPlugin().loadIncentInterstitial(zoneId);
	}

	public static void ShowRewardedInterstitial()
	{
		getDefaultPlugin().showIncentInterstitial();
	}

	public static void ShowRewardedInterstitial(string placement)
	{
		getDefaultPlugin().showIncentInterstitial(placement);
	}

	public static void ShowRewardedInterstitialForZoneId(string zoneId = null)
	{
		getDefaultPlugin().showIncentInterstitialForZoneId(zoneId);
	}

	public static void HideAd()
	{
		getDefaultPlugin().hideAd();
	}

	public static void SetAdPosition(float x, float y)
	{
		getDefaultPlugin().setAdPosition(x, y);
	}

	public static void SetAdWidth(int width)
	{
		getDefaultPlugin().setAdWidth(width);
	}

	public static void SetSdkKey(string sdkKey)
	{
		getDefaultPlugin().setSdkKey(sdkKey);
	}

	public static void SetVerboseLoggingOn(string verboseLogging)
	{
		getDefaultPlugin().setVerboseLoggingOn(verboseLogging);
	}

	public static void SetMuted(string muted)
	{
		getDefaultPlugin().setMuted(muted);
	}

	public static bool IsMuted()
	{
		return getDefaultPlugin().isMuted();
	}

	public static void SetTestAdsEnabled(string enabled)
	{
		getDefaultPlugin().setTestAdsEnabled(enabled);
	}

	public static bool IsTestAdsEnabled()
	{
		return getDefaultPlugin().isTestAdsEnabled();
	}

	public static void PreloadInterstitial(string zoneId = null)
	{
		getDefaultPlugin().preloadInterstitial(zoneId);
	}

	public static bool HasPreloadedInterstitial(string zoneId = null)
	{
		return getDefaultPlugin().hasPreloadedInterstitial(zoneId);
	}

	public static bool IsInterstitialShowing()
	{
		return getDefaultPlugin().isInterstitialShowing();
	}

	public static bool IsIncentInterstitialReady(string zoneId = null)
	{
		return getDefaultPlugin().isIncentInterstitialReady(zoneId);
	}

	public static bool IsPreloadedInterstitialVideo()
	{
		return getDefaultPlugin().isPreloadedInterstitialVideo();
	}

	public static void InitializeSdk()
	{
		getDefaultPlugin().initializeSdk();
	}

	public static void SetUnityAdListener(string gameObjectToNotify)
	{
		getDefaultPlugin().setAdListener(gameObjectToNotify);
	}

	public static void SetRewardedVideoUsername(string username)
	{
		getDefaultPlugin().setRewardedVideoUsername(username);
	}

	public static void TrackEvent(string eventType, IDictionary<string, string> parameters)
	{
		getDefaultPlugin().trackEvent(eventType, parameters);
	}

	public static void EnableImmersiveMode()
	{
		getDefaultPlugin().enableImmersiveMode();
	}

	public static void SetHasUserConsent(string hasUserConsent)
	{
		getDefaultPlugin().setHasUserConsent(hasUserConsent);
	}

	public static bool HasUserConsent()
	{
		return getDefaultPlugin().hasUserConsent();
	}

	public static void SetIsAgeRestrictedUser(string isAgeRestrictedUser)
	{
		getDefaultPlugin().setIsAgeRestrictedUser(isAgeRestrictedUser);
	}

	public static bool IsAgeRestrictedUser()
	{
		return getDefaultPlugin().isAgeRestrictedUser();
	}
}
