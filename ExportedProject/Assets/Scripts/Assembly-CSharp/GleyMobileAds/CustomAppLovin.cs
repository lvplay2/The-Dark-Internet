using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GleyMobileAds
{
	public class CustomAppLovin : MonoBehaviour, ICustomAds
	{
		private const int reloadInterval = 20;

		private const int maxRetryCount = 10;

		private bool debug;

		private bool initialized;

		private int retryNumberInterstitial;

		private int retryNumberRewarded;

		private UnityAction OnInterstitialClosed;

		private UnityAction<string> OnInterstitialClosedWithAdvertiser;

		private UnityAction<bool> OnCompleteMethod;

		private UnityAction<bool, string> OnCompleteMethodWithAdvertiser;

		private bool bannerUsed;

		private BannerPosition position;

		private BannerType bannerType;

		private UnityAction<bool, BannerPosition, BannerType> DisplayResult;

		public void InitializeAds(GDPRConsent consent, List<PlatformSettings> platformSettings)
		{
			debug = Advertisements.Instance.debug;
			if (!initialized)
			{
				if (debug)
				{
					AppLovin.SetVerboseLoggingOn("true");
				}
				PlatformSettings platformSettings2 = Enumerable.First(platformSettings, (PlatformSettings cond) => cond.platform == SupportedPlatforms.Android);
				AppLovin.SetSdkKey(platformSettings2.appId.id.ToString());
				if (consent == GDPRConsent.Accept || consent == GDPRConsent.Unset)
				{
					AppLovin.SetHasUserConsent("true");
				}
				else
				{
					AppLovin.SetHasUserConsent("false");
				}
				if (platformSettings2.directedForChildren)
				{
					AppLovin.SetIsAgeRestrictedUser("true");
				}
				else
				{
					AppLovin.SetIsAgeRestrictedUser("false");
				}
				AppLovin.InitializeSdk();
				AppLovin.SetTestAdsEnabled("false");
				AppLovin.SetUnityAdListener(base.gameObject.name);
				if (debug)
				{
					Debug.Log(string.Concat(this, " Start Initialization"));
					ScreenWriter.Write(string.Concat(this, " Start Initialization"));
					Debug.Log(string.Concat(this, " SDK key: ", platformSettings2.appId.id));
					ScreenWriter.Write(string.Concat(this, " SDK key: ", platformSettings2.appId.id));
				}
				PreloadInterstitial();
				PreloadRewardedVideo();
				initialized = true;
			}
		}

		public void UpdateConsent(GDPRConsent consent)
		{
			if (consent == GDPRConsent.Accept || consent == GDPRConsent.Unset)
			{
				AppLovin.SetHasUserConsent("true");
			}
			else
			{
				AppLovin.SetHasUserConsent("false");
			}
		}

		public bool IsBannerAvailable()
		{
			return true;
		}

		public void ResetBannerUsage()
		{
			bannerUsed = false;
		}

		public bool BannerAlreadyUsed()
		{
			return bannerUsed;
		}

		public void ShowBanner(BannerPosition position, BannerType bannerType, UnityAction<bool, BannerPosition, BannerType> DisplayResult)
		{
			bannerUsed = true;
			this.position = position;
			this.bannerType = bannerType;
			this.DisplayResult = DisplayResult;
			if (position == BannerPosition.BOTTOM)
			{
				AppLovin.ShowAd(-10000f, -50000f);
			}
			else
			{
				AppLovin.ShowAd(-10000f, -40000f);
			}
		}

		public void HideBanner()
		{
			AppLovin.HideAd();
		}

		public bool IsInterstitialAvailable()
		{
			return AppLovin.HasPreloadedInterstitial();
		}

		public void ShowInterstitial(UnityAction InterstitialClosed)
		{
			if (IsInterstitialAvailable())
			{
				OnInterstitialClosed = InterstitialClosed;
				AppLovin.ShowInterstitial();
			}
		}

		public void ShowInterstitial(UnityAction<string> InterstitialClosed)
		{
			if (IsInterstitialAvailable())
			{
				OnInterstitialClosedWithAdvertiser = InterstitialClosed;
				AppLovin.ShowInterstitial();
			}
		}

		public bool IsRewardVideoAvailable()
		{
			return AppLovin.IsIncentInterstitialReady();
		}

		public void ShowRewardVideo(UnityAction<bool> CompleteMethod)
		{
			if (IsRewardVideoAvailable())
			{
				OnCompleteMethod = CompleteMethod;
				AppLovin.ShowRewardedInterstitial();
			}
		}

		public void ShowRewardVideo(UnityAction<bool, string> CompleteMethod)
		{
			if (IsRewardVideoAvailable())
			{
				OnCompleteMethodWithAdvertiser = CompleteMethod;
				AppLovin.ShowRewardedInterstitial();
			}
		}

		private void onAppLovinEventReceived(string ev)
		{
			if (debug)
			{
				Debug.Log(string.Concat(this, " ", ev));
				ScreenWriter.Write(string.Concat(this, " ", ev));
			}
			if (ev.Contains("LOADEDBANNER"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " banner ad shown"));
					ScreenWriter.Write(string.Concat(this, " banner ad shown"));
				}
				if (DisplayResult != null)
				{
					DisplayResult(true, position, bannerType);
					DisplayResult = null;
				}
			}
			else if (ev.Contains("LOADBANNERFAILED"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " banner ad failed to load"));
					ScreenWriter.Write(string.Concat(this, " banner ad failed to load"));
				}
				if (DisplayResult != null)
				{
					DisplayResult(false, position, bannerType);
					DisplayResult = null;
				}
			}
			if (ev.Contains("DISPLAYEDINTER"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " interstitial ad was shown"));
					ScreenWriter.Write(string.Concat(this, " interstitial ad was shown"));
				}
			}
			else if (ev.Contains("HIDDENINTER"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " interstitial ad was closed"));
					ScreenWriter.Write(string.Concat(this, " interstitial ad was closed"));
				}
				if (OnInterstitialClosed != null)
				{
					OnInterstitialClosed();
					OnInterstitialClosed = null;
				}
				if (OnInterstitialClosedWithAdvertiser != null)
				{
					OnInterstitialClosedWithAdvertiser(SupportedAdvertisers.AppLovin.ToString());
					OnInterstitialClosedWithAdvertiser = null;
				}
				PreloadInterstitial();
			}
			else if (ev.Contains("LOADEDINTER"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " interstitial ad was loaded"));
					ScreenWriter.Write(string.Concat(this, " interstitial ad was loaded"));
				}
				retryNumberInterstitial = 0;
			}
			else if (string.Equals(ev, "LOADINTERFAILED"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " interstitial ad failed to load"));
					ScreenWriter.Write(string.Concat(this, " interstitial ad failed to load"));
					Debug.Log(string.Concat(this, " reloading ", retryNumberInterstitial, " in ", 20, " sec"));
					ScreenWriter.Write(string.Concat(this, " reloading ", retryNumberInterstitial, " in ", 20, " sec"));
				}
				Invoke("PreloadInterstitial", 20f);
			}
			if (ev.Contains("REWARDAPPROVEDINFO"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " rewarded videw was completed"));
					ScreenWriter.Write(string.Concat(this, " rewarded videw was completed"));
				}
				if (OnCompleteMethod != null)
				{
					OnCompleteMethod(true);
					OnCompleteMethod = null;
				}
				if (OnCompleteMethodWithAdvertiser != null)
				{
					OnCompleteMethodWithAdvertiser(true, SupportedAdvertisers.AppLovin.ToString());
					OnCompleteMethodWithAdvertiser = null;
				}
			}
			else if (ev.Contains("LOADEDREWARDED"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " rewarded video was successfully loaded"));
					ScreenWriter.Write(string.Concat(this, " rewarded video was successfully loaded"));
				}
				retryNumberRewarded = 0;
			}
			else if (ev.Contains("LOADREWARDEDFAILED"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " rewarded video failed to load"));
					ScreenWriter.Write(string.Concat(this, " rewarded video failed to load"));
					Debug.Log(string.Concat(this, " reloading ", retryNumberRewarded, " in ", 20, " sec"));
					ScreenWriter.Write(string.Concat(this, " reloading ", retryNumberRewarded, " in ", 20, " sec"));
				}
				Invoke("PreloadRewardedVideo", 20f);
			}
			else if (ev.Contains("HIDDENREWARDED"))
			{
				if (debug)
				{
					Debug.Log(string.Concat(this, " rewarded video was closed"));
					ScreenWriter.Write(string.Concat(this, " rewarded video was closed"));
				}
				PreloadRewardedVideo();
			}
		}

		private void PreloadInterstitial()
		{
			retryNumberInterstitial++;
			if (retryNumberInterstitial < 10)
			{
				AppLovin.PreloadInterstitial();
			}
		}

		private void PreloadRewardedVideo()
		{
			retryNumberRewarded++;
			if (retryNumberRewarded < 10)
			{
				AppLovin.LoadRewardedInterstitial();
			}
		}
	}
}
