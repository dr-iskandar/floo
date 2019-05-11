using UnityEngine;
using System.Collections;

public class SoundUtility : MonoBehaviour 
{
	public AudioSource BGMSource;
	public AudioSource SFXSource;
	public AudioClip[] BGMClip;
	public AudioClip[] SFXClip;

	private static SoundUtility instance;

	public static SoundUtility Instance {
		get 
		{
			if (instance == null)
			{
				Object prefabSoundManager = Resources.Load ("Prefab/SoundManager");
				GameObject go = Instantiate (prefabSoundManager) as GameObject;

				instance = go.GetComponent<SoundUtility>();
				DontDestroyOnLoad(go);
			}

			return instance;
		}
	}

	#region Setting
	public void SetSettingBGM (bool isOn)
	{
		EssentialData.Instance.SaveSettingBGM (isOn);
		BGMSource.mute = !EssentialData.Instance.LoadSettingBGM ();
	}

	public void SetSettingSFX (bool isOn)
	{
		EssentialData.Instance.SaveSettingSFX (isOn);
		SFXSource.mute = !EssentialData.Instance.LoadSettingSFX ();
	}
	#endregion

	#region Play Audio

	public void SetBGM(BGMData BGMType)
	{
		if(BGMSource!=null)
		{
			BGMSource.clip = BGMClip[(int)BGMType - 1];
			BGMSource.Play();
			BGMSource.loop = true;
			BGMSource.mute = !EssentialData.Instance.LoadSettingBGM ();
		}
	}

	public void SetBGM(AudioClip BGM)
	{
		if(BGMSource!=null)
		{
			BGMSource.clip = BGM;
			BGMSource.Play();
			BGMSource.loop = true;
			BGMSource.mute = !EssentialData.Instance.LoadSettingBGM ();
		}
	}

	public void PlaySFX(SFXData SFXType)
	{
		if(SFXSource!=null)
		{
			SFXSource.mute = !EssentialData.Instance.LoadSettingSFX ();
			SFXSource.PlayOneShot(SFXClip[(int)SFXType-1]);
		}
	}

	public void PlaySFXClips(AudioClip SFX)
	{
		if(SFXSource!=null)
		{
			SFXSource.mute = !EssentialData.Instance.LoadSettingSFX ();
			SFXSource.PlayOneShot(SFX);
		}
	}

	#endregion
}
