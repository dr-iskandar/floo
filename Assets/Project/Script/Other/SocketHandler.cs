using UnityEngine;
using System.Collections;

public class SocketHandler : MonoBehaviour {
	public bool connected;

	void Start()
	{
		connected = false;
		StartCoroutine(BackEndConnect.Instance.EstablishConnection ());
		StartCoroutine(FirstConnection());
	}

	IEnumerator FirstConnection()
	{
		if (BackEndConnect.Instance.socketReady) {
			string playerID = "dasf";
			BackEndConnect.Instance.SendSocket (APITag.socketTagJoin, playerID);
			while (!connected) {
				string reply = BackEndConnect.Instance.backendWebSocket.RecvString ();
				if (reply != null) {
					connected = true;
					Debug.Log (reply);
					//code here
				}
				if (BackEndConnect.Instance.backendWebSocket.Error != null) {
					Debug.LogError ("Error: " + BackEndConnect.Instance.backendWebSocket.Error);
					break;
				}
				yield return 0;
			}
		} else {
			yield return new WaitForSeconds (0.1f);
			StartCoroutine (FirstConnection ());
		}
	}

	void CheckResult()
	{
		
	}

}
