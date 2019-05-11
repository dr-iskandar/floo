using UnityEngine;
using System.Collections;

public class UnitManager : MonoBehaviour {

	//Mesh of the units. Already has the animation Effect attached inside. child no 1 is main Texture, no 2 is second Tex, and so on
//	public GameObject lubu;
//	public GameObject sudirman;
//	public Transform placementTranform;
//
//	public void AssignLubuTexture(string keywordA, string keywordB)
//	{
//		TextureManager.Instance.SetTwoTexture (lubu,keywordA,keywordB);
//	}
//		
//	public GameObject PlaceUnit(GameObject go, Transform tf)
//	{
//		GameObject aa = Instantiate (go) as GameObject;
//		aa.transform.position = tf.position;
//		aa.transform.rotation = tf.rotation;
//		return aa;
//	}
//
//	public IEnumerator SequentialPlay(GameObject go)
//	{
//		go.GetComponent<Animator>().Play ("lubu_deploy");
//		yield return new WaitForSeconds (1.1f);
//		go.GetComponent<Animator>().Play ("lubu_move");
//		yield return new WaitForSeconds(3);
//		go.GetComponent<Animator>().Play ("lubu_skill_charge_move");
//		yield return new WaitForSeconds(2);
//		go.GetComponent<Animator>().Play ("lubu_skill_charge_attack");
//		yield return new WaitForSeconds(0.8f);
//		go.GetComponent<Animator>().Play ("lubu_move");
//		yield return new WaitForSeconds(3);
//
//		StartCoroutine (SequentialPlay (go));
//	}
}
