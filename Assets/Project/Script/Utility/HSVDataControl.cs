using UnityEngine;
using System.Collections;

public class HSVDataControl : MonoBehaviour {

	private static HSVDataControl instance;

	public static HSVDataControl Instance 
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("HSV Data Control");
				instance = go.AddComponent<HSVDataControl>();
			}
			return instance;
		}
	}

	private FishHSVData fishHSVData = new FishHSVData();

	public FishHSVData GetFishHSV(int code)
	{
		switch (code) 
		{

		//Nemo
		case 11:
			SetNemoPink ();
			break;
		case 12:
			SetNemoPurple ();
			break;
		case 13:
			SetNemoGreen ();
			break;
		case 14:
			SetNemoBlue ();
			break;
		case 15:
			SetNemoYellow ();
			break;
		case 16:
			SetNemoRed ();
			break;
		case 17:
			SetNemoDarkBlue ();
			break;
		case 18:
			SetNemoDarkRed ();
			break;
		case 19:
			SetNemoSilver ();
			break;

		//Dory
		case 21: 
			SetDoryGreen ();
			break;
		case 22: 
			SetDoryRed ();
			break;
		case 23: 
			SetDoryPink ();
			break;
		case 24: 
			SetDoryBlue ();
			break;
		case 25: 
			SetDoryBlack ();
			break;
		case 26: 
			SetDorySilver ();
			break;

		//Angler
		case 31:
			SetAnglerPurple ();
			break;
		case 32:
			SetAnglerRed ();
			break;
		case 33:
			SetAnglerBlue ();
			break;
		case 34:
			SetAnglerPink ();
			break;
		case 35:
			SetAnglerGreen ();
			break;
		case 36:
			SetAnglerSilver ();
			break;
		case 37:
			SetAnglerGold ();
			break;

		case 41:
			SetFlowerGreen();
			break;
		case 42:
			SetFlowerBlue();
			break;
		case 43:
			SetFlowerPink();
			break;
		case 44:
			SetFlowerGray();
			break;
		case 45:
			SetFlowerYellow();
			break;
		case 46:
			SetFlowerOrange();
			break;

		default:
			SetDefaultHSV ();
			break;

		}

		return fishHSVData;
	}

	private void SetFishHSV(float H, float S, float V)
	{
		fishHSVData.fishHue = H;
		fishHSVData.fishSaturation = S;
		fishHSVData.fishValue = V;
	}

	private void SetEyesHSV(float H, float S, float V)
	{
		fishHSVData.fishEyeHue = H;
		fishHSVData.fishEyeSaturation = S;
		fishHSVData.fishEyeValue = V;
	}

	private void SetDefaultHSV()
	{
		SetFishHSV (0,1,1);
		SetEyesHSV (0,1,1);
	}

	#region Nemo
	public void SetNemoPink()
	{
		SetFishHSV (FishVariant.nemoPinkHue, FishVariant.nemoPinkSaturation, FishVariant.nemoPinkValue);	
		SetEyesHSV (FishVariant.nemoPinkEyeHue, FishVariant.nemoPinkEyeSaturation, FishVariant.nemoPinkEyeValue);
	}
	public void SetNemoPurple()
	{
		SetFishHSV (FishVariant.nemoPurpleHue, FishVariant.nemoPurpleSaturation, FishVariant.nemoPurpleValue);	
		SetEyesHSV (FishVariant.nemoPurpleEyeHue, FishVariant.nemoPurpleEyeSaturation, FishVariant.nemoPurpleEyeValue);
	}
	public void SetNemoGreen()
	{
		SetFishHSV (FishVariant.nemoGreenHue, FishVariant.nemoGreenSaturation, FishVariant.nemoGreenValue);	
		SetEyesHSV (FishVariant.nemoGreenEyeHue, FishVariant.nemoGreenEyeSaturation, FishVariant.nemoGreenEyeValue);
	}
	public void SetNemoBlue()
	{
		SetFishHSV (FishVariant.nemoBlueHue, FishVariant.nemoBlueSaturation, FishVariant.nemoBlueValue);	
		SetEyesHSV (FishVariant.nemoBlueEyeHue, FishVariant.nemoBlueEyeSaturation, FishVariant.nemoBlueEyeValue);
	}
	public void SetNemoYellow()
	{
		SetFishHSV (FishVariant.nemoYellowHue, FishVariant.nemoYellowSaturation, FishVariant.nemoYellowValue);	
		SetEyesHSV (FishVariant.nemoYellowEyeHue, FishVariant.nemoYellowEyeSaturation, FishVariant.nemoYellowEyeValue);
	}
	public void SetNemoRed()
	{
		SetFishHSV (FishVariant.nemoRedHue, FishVariant.nemoRedSaturation, FishVariant.nemoRedValue);	
		SetEyesHSV (FishVariant.nemoRedEyeHue, FishVariant.nemoRedEyeSaturation, FishVariant.nemoRedEyeValue);
	}
	public void SetNemoDarkBlue()
	{
		SetFishHSV (FishVariant.nemoDarkBlueHue, FishVariant.nemoDarkBlueSaturation, FishVariant.nemoDarkBlueValue);	
		SetEyesHSV (FishVariant.nemoDarkBlueEyeHue, FishVariant.nemoDarkBlueEyeSaturation, FishVariant.nemoDarkBlueEyeValue);
	}
	public void SetNemoDarkRed()
	{
		SetFishHSV (FishVariant.nemoDarkRedHue, FishVariant.nemoDarkRedSaturation, FishVariant.nemoDarkRedValue);	
		SetEyesHSV (FishVariant.nemoDarkRedEyeHue, FishVariant.nemoDarkRedEyeSaturation, FishVariant.nemoDarkRedEyeValue);
	}
	public void SetNemoSilver()
	{
		SetFishHSV (FishVariant.nemoSilverHue, FishVariant.nemoSilverSaturation, FishVariant.nemoSilverValue);	
		SetEyesHSV (FishVariant.nemoSilverEyeHue, FishVariant.nemoSilverEyeSaturation, FishVariant.nemoSilverEyeValue);
	}
	#endregion

	#region Dory
	public void SetDoryGreen()
	{
		SetFishHSV (FishVariant.doryGreenHue, FishVariant.doryGreenSaturation, FishVariant.doryGreenValue);	
		SetEyesHSV (FishVariant.doryGreenEyeHue, FishVariant.doryGreenEyeSaturation, FishVariant.doryGreenEyeValue);
	}
	public void SetDoryRed()
	{
		SetFishHSV (FishVariant.doryRedHue, FishVariant.doryRedSaturation, FishVariant.doryRedValue);	
		SetEyesHSV (FishVariant.doryRedEyeHue, FishVariant.doryRedEyeSaturation, FishVariant.doryRedEyeValue);
	}
	public void SetDoryPink()
	{
		SetFishHSV (FishVariant.doryPinkHue, FishVariant.doryPinkSaturation, FishVariant.doryPinkValue);	
		SetEyesHSV (FishVariant.doryPinkEyeHue, FishVariant.doryPinkEyeSaturation, FishVariant.doryPinkEyeValue);
	}
	public void SetDoryBlue()
	{
		SetFishHSV (FishVariant.doryBlueHue, FishVariant.doryBlueSaturation, FishVariant.doryBlueValue);	
		SetEyesHSV (FishVariant.doryBlueEyeHue, FishVariant.doryBlueEyeSaturation, FishVariant.doryBlueEyeValue);
	}
	public void SetDoryBlack()
	{
		SetFishHSV (FishVariant.doryBlackHue, FishVariant.doryBlackSaturation, FishVariant.doryBlackValue);	
		SetEyesHSV (FishVariant.doryBlackEyeHue, FishVariant.doryBlackEyeSaturation, FishVariant.doryBlackEyeValue);
	}
	public void SetDorySilver()
	{
		SetFishHSV (FishVariant.dorySilverHue, FishVariant.dorySilverSaturation, FishVariant.dorySilverValue);	
		SetEyesHSV (FishVariant.dorySilverEyeHue, FishVariant.dorySilverEyeSaturation, FishVariant.dorySilverEyeValue);
	}
	#endregion

	#region Angler
	public void SetAnglerPurple()
	{
		SetFishHSV (FishVariant.anglerPurpleHue, FishVariant.anglerPurpleSaturation, FishVariant.anglerPurpleValue);	
		SetEyesHSV (FishVariant.anglerPurpleEyeHue, FishVariant.anglerPurpleEyeSaturation, FishVariant.anglerPurpleEyeValue);
	}
	public void SetAnglerRed()
	{
		SetFishHSV (FishVariant.anglerRedHue, FishVariant.anglerRedSaturation, FishVariant.anglerRedValue);	
		SetEyesHSV (FishVariant.anglerRedEyeHue, FishVariant.anglerRedEyeSaturation, FishVariant.anglerRedEyeValue);
	}
	public void SetAnglerBlue()
	{
		SetFishHSV (FishVariant.anglerBlueHue, FishVariant.anglerBlueSaturation, FishVariant.anglerBlueValue);	
		SetEyesHSV (FishVariant.anglerBlueEyeHue, FishVariant.anglerBlueEyeSaturation, FishVariant.anglerBlueEyeValue);
	}
	public void SetAnglerPink()
	{
		SetFishHSV (FishVariant.anglerPinkHue, FishVariant.anglerPinkSaturation, FishVariant.anglerPinkValue);	
		SetEyesHSV (FishVariant.anglerPinkEyeHue, FishVariant.anglerPinkEyeSaturation, FishVariant.anglerPinkEyeValue);
	}
	public void SetAnglerGreen()
	{
		SetFishHSV (FishVariant.anglerGreenHue, FishVariant.anglerGreenSaturation, FishVariant.anglerGreenValue);	
		SetEyesHSV (FishVariant.anglerGreenEyeHue, FishVariant.anglerGreenEyeSaturation, FishVariant.anglerGreenEyeValue);
	}
	public void SetAnglerSilver()
	{
		SetFishHSV (FishVariant.anglerSilverHue, FishVariant.anglerSilverSaturation, FishVariant.anglerSilverValue);	
		SetEyesHSV (FishVariant.anglerSilverEyeHue, FishVariant.anglerSilverEyeSaturation, FishVariant.anglerSilverEyeValue);
	}
	public void SetAnglerGold()
	{
		SetFishHSV (FishVariant.anglerGoldHue, FishVariant.anglerGoldSaturation, FishVariant.anglerGoldValue);	
		SetEyesHSV (FishVariant.anglerGoldEyeHue, FishVariant.anglerGoldEyeSaturation, FishVariant.anglerGoldEyeValue);
	}
	#endregion

	#region Flower
	public void SetFlowerGreen()
	{
		SetFishHSV (FishVariant.flowerGreenHue, FishVariant.flowerGreenSaturation, FishVariant.flowerGreenValue);	
		SetEyesHSV (FishVariant.flowerGreenEyeHue, FishVariant.flowerGreenEyeSaturation, FishVariant.flowerGreenEyeValue);
	}
	public void SetFlowerBlue()
	{
		SetFishHSV (FishVariant.flowerBlueHue, FishVariant.flowerBlueSaturation, FishVariant.flowerBlueValue);	
		SetEyesHSV (FishVariant.flowerBlueEyeHue, FishVariant.flowerBlueEyeSaturation, FishVariant.flowerBlueEyeValue);
	}
	public void SetFlowerPink()
	{
		SetFishHSV (FishVariant.flowerPinkHue, FishVariant.flowerPinkSaturation, FishVariant.flowerPinkValue);	
		SetEyesHSV (FishVariant.flowerPinkEyeHue, FishVariant.flowerPinkEyeSaturation, FishVariant.flowerPinkEyeValue);
	}
	public void SetFlowerGray()
	{
		SetFishHSV (FishVariant.flowerGrayHue, FishVariant.flowerGraySaturation, FishVariant.flowerGrayValue);	
		SetEyesHSV (FishVariant.flowerGrayEyeHue, FishVariant.flowerGrayEyeSaturation, FishVariant.flowerGrayEyeValue);
	}
	public void SetFlowerYellow()
	{
		SetFishHSV (FishVariant.flowerYellowHue, FishVariant.flowerYellowSaturation, FishVariant.flowerYellowValue);	
		SetEyesHSV (FishVariant.flowerYellowEyeHue, FishVariant.flowerYellowEyeSaturation, FishVariant.flowerYellowEyeValue);
	}
	public void SetFlowerOrange()
	{
		SetFishHSV (FishVariant.flowerOrangeHue, FishVariant.flowerOrangeSaturation, FishVariant.flowerOrangeValue);	
		SetEyesHSV (FishVariant.flowerOrangeEyeHue, FishVariant.flowerOrangeEyeSaturation, FishVariant.flowerOrangeEyeValue);
	}
	#endregion
}
