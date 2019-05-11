using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class API : MonoBehaviour {

	private const string Url = "http://103.196.116.229:8080/fl_api_new/login";

	public GameObject email;
	public GameObject login;

	public Button btnlogin;

	public string Email;
	public string password;

	[SerializeField]
	public class UserDetail
	{
		public string message;
		public int action;
		public Data data;
	}

	[SerializeField]
	public class Data
	{
		public string firs_name;
	}

	void Start()
	{
		btnlogin.onClick.AddListener (validateLogin);
	}

	private void validateLogin()
	{
		Email = email.GetComponent<InputField> ().text;

		if (Email != "") {
			StartCoroutine (CallLogin (Email));
		} else {
		}
	}

	public IEnumerator CallLogin(string Email)
	{
		WWWForm form = new WWWForm ();
		form.AddField ("email", Email);

		UnityWebRequest www = UnityWebRequest.Post (Url,form);

		yield return www.Send ();

		if (www.error != null) {
			Debug.Log ("Error" + www.error);
		} else 
		{
			Debug.Log ("Response" + www.downloadHandler.text);

//			UserDetail userDetail = JsonUtility.FromJson<UserDetail> (www.downloadHandler.text);

			/*if (userDetail.action == 2) 
			{
				print ("login success" +userDetail.data.firs_name);
			}*/
		}
	}
}
