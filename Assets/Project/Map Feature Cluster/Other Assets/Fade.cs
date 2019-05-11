using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

	public MeshRenderers[] meshRendererPacket1;
	public MeshRenderers[] meshRendererPacket2;
	void OnTriggerEnter()
	{
		meshRendererPacket1 [0].TriggerEnter();
		meshRendererPacket2 [0].TriggerEnter();
	}

	void OnTriggerExit()
	{
		meshRendererPacket1 [0].TriggerExit();
		meshRendererPacket2 [0].TriggerExit();
	}
}
