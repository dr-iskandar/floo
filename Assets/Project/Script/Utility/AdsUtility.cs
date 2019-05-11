using UnityEngine;
using System.Collections;

public class AdsUtility : MonoBehaviour {

	public static AdsUtility instance;

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy (this.gameObject);
		}
	}

	void Start () {
		/*GoogleMobileAd.Init();

		GoogleMobileAd.OnInterstitialLoaded += OnInterstisialsLoaded;
		GoogleMobileAd.OnInterstitialOpened += OnInterstisialsOpen;
		GoogleMobileAd.OnInterstitialClosed += OnInterstisialsClosed;
		GoogleMobileAd.OnInterstitialFailedLoading += OnInterstitialFailedLoading;
	*/}

	public void LoadInterstitialAds(){
		//loadin ad:
	//	GoogleMobileAd.LoadInterstitialAd ();
	}

	public void ShowInterstitialAds(){
		//loadin ad:
	//	GoogleMobileAd.ShowInterstitialAd ();
	}

	private void OnInterstisialsLoaded() {
		//ad loaded, strting ad
	}

	private void OnInterstisialsOpen() {
		//pausing the game
	}

	private void OnInterstisialsClosed() {
		//un-pausing the game
	}

	private void OnInterstitialFailedLoading() {
		//un-pausing the game
	}
}
