using UnityEngine;
using System.Collections;

public class BuffEffect : MonoBehaviour {

	public MeshRenderer[] meshRenderer;
	public SkinnedMeshRenderer[] skinMeshRenderer;
	public SpriteRenderer[] spriteRends;

	private int sorting = 20;

	void Awake()
	{
		AlphaTransition (meshRenderer);
		AlphaTransition (skinMeshRenderer);
		AlphaTransition (spriteRends);
	}

	void AlphaTransition(MeshRenderer[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			objects [i].sortingOrder = objects [i].sortingOrder = 0;
		}
	}

	void AlphaTransition(SkinnedMeshRenderer[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			objects [i].sortingOrder = objects [i].sortingOrder = 0;
		}
	}	

	void AlphaTransition(SpriteRenderer[] objects)
	{
		for (int i = 0; i < objects.Length; i++)
		{
			objects [i].sortingOrder = objects [i].sortingOrder = 0;
		}
	}
}
