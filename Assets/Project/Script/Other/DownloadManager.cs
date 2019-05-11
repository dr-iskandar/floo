using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DownloadManager : MonoBehaviour {

	private GameObject lubuGO;
	private Texture2D lubuTexture;
	private Texture2D lubuHorseTexture;
	private Texture2D testTex;

	[System.NonSerialized] public bool downloadDone; 
	[System.NonSerialized] public bool allAssetDownloaded;

	public void StartDownload(List<string> listDL,List<string> keyword ,List<string> filename )
	{
		StartCoroutine(DownloadTexture (listDL,keyword,filename));
	}

	IEnumerator DownloadTexture(List<string> listDownload, List<string> keyword, List<string> filename)
	{
		allAssetDownloaded = false;
		for (int i=0;i<listDownload.Count;i++)
		{
			StartCoroutine(DownloadAndSaveTexture(keyword[i],filename[i], listDownload[i]));
			if (!downloadDone) 
			{
				yield return new WaitForSeconds (0.5f);
			}
			downloadDone = false; //prepare for redownload
		}
		allAssetDownloaded = true;
	}

	// Save As = Keyword || Image Name = File Name
	public IEnumerator DownloadAndSaveTexture(string saveAs, string imageName, string downloadUrl)
	{
		Texture2D tex;
		byte[] byteArray;
		WWW www= new WWW(downloadUrl);
		yield return www;

		if (www.texture != null) {
			Debug.Log (String.Format ("Done Downloading Asset\n{0}\nKeyword = {1}\nFileName = {2}", downloadUrl, saveAs, imageName));
		} else {
			Debug.LogError (String.Format ("Failed Downloading Asset!\n{0}\nKeyword = {1}\nFileName = {2}", downloadUrl, saveAs, imageName));
		}

		if (allAssetDownloaded)
			Debug.Log ("All Asset Downloaded");
		
		tex=www.texture;
		byteArray=tex.EncodeToPNG();
		string temp=Convert.ToBase64String(byteArray);

		PlayerPrefs.SetString(saveAs,temp);      // save it to file
		PlayerPrefs.SetInt(saveAs+"_w",tex.width);
		PlayerPrefs.SetInt(saveAs+"_h",tex.height);

		downloadDone = true;
	}

}