using UnityEngine;
using System.Collections;

public class FishVariant : MonoBehaviour {

	#region Nemo

	//Nemo Variant 1 - Pink
	public const float nemoPinkHue = 24.36f;
	public const float nemoPinkSaturation = 1.5f;
	public const float nemoPinkValue = 1.5f;
	public const float nemoPinkEyeHue = 44.03f;
	public const float nemoPinkEyeSaturation = 2.5f;
	public const float nemoPinkEyeValue = 1.1f;

	//Nemo Variant 2 - Purple
	public const float nemoPurpleHue = 62.8f;
	public const float nemoPurpleSaturation = 1f;
	public const float nemoPurpleValue = 1f;
	public const float nemoPurpleEyeHue = 131.5f;
	public const float nemoPurpleEyeSaturation = 1f;
	public const float nemoPurpleEyeValue = 1f;

	//Nemo Variant 3 - Green
	public const float nemoGreenHue = 264.7f;
	public const float nemoGreenSaturation = 1.1f;
	public const float nemoGreenValue = 1f;
	public const float nemoGreenEyeHue = 44.03f;
	public const float nemoGreenEyeSaturation = 1.8f;
	public const float nemoGreenEyeValue = 1f;

	//Nemo Variant 4 - Blue
	public const float nemoBlueHue = 166f;
	public const float nemoBlueSaturation = 1.2f;
	public const float nemoBlueValue = 1.5f;
	public const float nemoBlueEyeHue = 302.8f;
	public const float nemoBlueEyeSaturation = 2.5f;
	public const float nemoBlueEyeValue = 1.1f;

	//Nemo Variant 5 - Yellow
	public const float nemoYellowHue = 141.79f;
	public const float nemoYellowSaturation = -3.2f;
	public const float nemoYellowValue = 1.5f;
	public const float nemoYellowEyeHue = -432.8f;
	public const float nemoYellowEyeSaturation = 2f;
	public const float nemoYellowEyeValue = 1.1f;

	//Nemo Variant 6 - Red
	public const float nemoRedHue = 373.5f;
	public const float nemoRedSaturation = 1.6f;
	public const float nemoRedValue = 1.5f;
	public const float nemoRedEyeHue = -366.17f;
	public const float nemoRedEyeSaturation = 2f;
	public const float nemoRedEyeValue = 1.1f;

	//Nemo Variant 7 - Dark Blue
	public const float nemoDarkBlueHue = 137.7f;
	public const float nemoDarkBlueSaturation = -6.7f;
	public const float nemoDarkBlueValue = -0.2f;
	public const float nemoDarkBlueEyeHue = -320.16f;
	public const float nemoDarkBlueEyeSaturation = 1.8f;
	public const float nemoDarkBlueEyeValue = 1f;

	//Nemo Variant 8 - Dark Red
	public const float nemoDarkRedHue = -9.4f;
	public const float nemoDarkRedSaturation = -12.4f;
	public const float nemoDarkRedValue = -0.2f;
	public const float nemoDarkRedEyeHue = -373.4f;
	public const float nemoDarkRedEyeSaturation = 2.51f;
	public const float nemoDarkRedEyeValue = 1f;

	//Nemo Variant 9 - Silver (Special)
	public const float nemoSilverHue = 1f;
	public const float nemoSilverSaturation = -0.1f;
	public const float nemoSilverValue = 1.3f;
	public const float nemoSilverEyeHue = 8.95f;
	public const float nemoSilverEyeSaturation = -1.5f;
	public const float nemoSilverEyeValue = 1.1f;

	#endregion

	#region Dory

	//Dory Variant 1 - Green
	public const float doryGreenHue = 80f;
	public const float doryGreenSaturation = 1.5f;
	public const float doryGreenValue = 1f;
	public const float doryGreenEyeHue = 295f;
	public const float doryGreenEyeSaturation = 3f;
	public const float doryGreenEyeValue = 1f;

	//Dory Variant 2 - Red
	public const float doryRedHue = 182f;
	public const float doryRedSaturation = 3.5f;
	public const float doryRedValue = 1f;
	public const float doryRedEyeHue = 403f;
	public const float doryRedEyeSaturation = 3.2f;
	public const float doryRedEyeValue = 1f;

	//Dory Variant 3 - Pink
	public const float doryPinkHue = 236f;
	public const float doryPinkSaturation = 1.15f;
	public const float doryPinkValue = 2f;
	public const float doryPinkEyeHue = 263f;
	public const float doryPinkEyeSaturation = 1.15f;
	public const float doryPinkEyeValue = 1f;

	//Dory Variant 4 - Blue
	public const float doryBlueHue = -53.41f;
	public const float doryBlueSaturation = 1.5f;
	public const float doryBlueValue = 1.25f;
	public const float doryBlueEyeHue = 568f;
	public const float doryBlueEyeSaturation = 2.5f;
	public const float doryBlueEyeValue = 0.81f;

	//Dory Variant 5 - Black
	public const float doryBlackHue = 31.15f;
	public const float doryBlackSaturation = 0f;
	public const float doryBlackValue = 0.68f;
	public const float doryBlackEyeHue = 568.4f;
	public const float doryBlackEyeSaturation = 2.5f;
	public const float doryBlackEyeValue = 1f;

	//Dory Variant 6 - Silver (Special)
	public const float dorySilverHue = 162f;
	public const float dorySilverSaturation = 0f;
	public const float dorySilverValue = 2f;
	public const float dorySilverEyeHue = -0.59f;
	public const float dorySilverEyeSaturation = 0.24f;
	public const float dorySilverEyeValue = 1f;

	#endregion

	#region Angler
	//Angler Variant 1 - Purple
	public const float anglerPurpleHue = -139.6f;
	public const float anglerPurpleSaturation = -0.7f;
	public const float anglerPurpleValue =  1.1f;
	public const float anglerPurpleEyeHue = -263f;
	public const float anglerPurpleEyeSaturation =  1.8f;
	public const float anglerPurpleEyeValue = 1.34f;

	//Angler Variant 2 - Red
	public const float anglerRedHue = 180.47f;
	public const float anglerRedSaturation = 12.94f;
	public const float anglerRedValue = -0.1f;
	public const float anglerRedEyeHue = -149.38f;
	public const float anglerRedEyeSaturation = -0.2f;
	public const float anglerRedEyeValue = 1f;

	//Angler Variant 3 - Blue
	public const float anglerBlueHue = -207.2f;
	public const float anglerBlueSaturation = 0.782f;
	public const float anglerBlueValue = 1.4f;
	public const float anglerBlueEyeHue = 166.4f;
	public const float anglerBlueEyeSaturation = 7.4f;
	public const float anglerBlueEyeValue = 1.7f;

	//Angler Variant 4 - Pink
	public const float anglerPinkHue = 69.59f;
	public const float anglerPinkSaturation = 1.42f;
	public const float anglerPinkValue = 1.03f;
	public const float anglerPinkEyeHue = 1f;
	public const float anglerPinkEyeSaturation = -9.45f;
	public const float anglerPinkEyeValue = 1f;

	//Angler Variant 5 - Green
	public const float anglerGreenHue = -609.5f;
	public const float anglerGreenSaturation = 3.05f;
	public const float anglerGreenValue = -5.76f;
	public const float anglerGreenEyeHue = -19.15f;
	public const float anglerGreenEyeSaturation = 11.88f;
	public const float anglerGreenEyeValue = 1f;

	//Angler Variant 6 - Silver (Special)
	public const float anglerSilverHue = -149.38f;
	public const float anglerSilverSaturation = -0.2f;
	public const float anglerSilverValue = 1f;
	public const float anglerSilverEyeHue = -150f;
	public const float anglerSilverEyeSaturation = 4.9f;
	public const float anglerSilverEyeValue = 1f;

	//Angler Variant 6 - Gold (Special)
	public const float anglerGoldHue = -403.2f;
	public const float anglerGoldSaturation = 1.2f;
	public const float anglerGoldValue = 1.5f;
	public const float anglerGoldEyeHue = 1f;
	public const float anglerGoldEyeSaturation = 11f;
	public const float anglerGoldEyeValue = 1.2f;
	#endregion

	#region Flower
	//Flower Variant 1 - Green
	public const float flowerGreenHue = -48.6f;
	public const float flowerGreenSaturation = 1f;
	public const float flowerGreenValue =  1f;
	public const float flowerGreenEyeHue = -48.6f;
	public const float flowerGreenEyeSaturation =  1f;
	public const float flowerGreenEyeValue = 1f;

	//Flower Variant 2 - Blue
	public const float flowerBlueHue = -36.7f;
	public const float flowerBlueSaturation = 1.5f;
	public const float flowerBlueValue = 1f;
	public const float flowerBlueEyeHue = -36.7f;
	public const float flowerBlueEyeSaturation = 1.5f;
	public const float flowerBlueEyeValue = 1f;

	//Flower Variant 3 - Pink
	public const float flowerPinkHue = 1.82f;
	public const float flowerPinkSaturation = -1.71f;
	public const float flowerPinkValue = 1f;
	public const float flowerPinkEyeHue = 1.82f;
	public const float flowerPinkEyeSaturation = -1.71f;
	public const float flowerPinkEyeValue = 1f;

	//Flower Variant 4 - Gray
	public const float flowerGrayHue = 0f;
	public const float flowerGraySaturation = 0.19f;
	public const float flowerGrayValue = 1.27f;
	public const float flowerGrayEyeHue = 0f;
	public const float flowerGrayEyeSaturation = 0.19f;
	public const float flowerGrayEyeValue = 1.27f;

	//Flower Variant 5 - Yellow
	public const float flowerYellowHue = -429.56f;
	public const float flowerYellowSaturation = -1.35f;
	public const float flowerYellowValue = 1f;
	public const float flowerYellowEyeHue = -429.56f;
	public const float flowerYellowEyeSaturation = -1.35f;
	public const float flowerYellowEyeValue = 1f;

	//Flower Variant 6 - Orange
	public const float flowerOrangeHue = -32.45f;
	public const float flowerOrangeSaturation = -3.4f;
	public const float flowerOrangeValue = 0.53f;
	public const float flowerOrangeEyeHue = -32.45f;
	public const float flowerOrangeEyeSaturation = -3.4f;
	public const float flowerOrangeEyeValue = 0.53f;

	#endregion
}
