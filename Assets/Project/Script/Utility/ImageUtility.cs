using UnityEngine;
using System.Collections;

public class ImageUtility
{
	public static Sprite CreateSpriteFromObject(Object o)
	{
		Texture2D tex = o as Texture2D;
		return Sprite.Create(o as Texture2D, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
	}

	public static IEnumerator CreateSpriteFromUrl (string url, System.Action<Sprite> actSpriteFunc)
	{
		WWW www = new WWW (url);

		yield return www;

		Sprite sp = Sprite.Create (www.texture, new Rect (0.0f, 0.0f, www.texture.width, www.texture.height), new Vector2 (0.5f, 0.5f));

		actSpriteFunc (sp);

		yield return null;
	}

}
