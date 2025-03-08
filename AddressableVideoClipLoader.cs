#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

public class AddressableVideoClipLoader : AddressableAssetLoader<VideoClip>
{
	public AssetReferenceT<VideoClip> videoClipRef;

	[SerializeField]
	public VideoPlayer videoPlayer;

	public override void Load()
	{
		if (loaded)
		{
			return;
		}

		operation = videoClipRef.LoadAssetAsync();
		if (instant)
		{
			operation.WaitForCompletion();
			SetVideoClip();
		}
		else
		{
			operation.Completed += delegate
			{
				SetVideoClip();
			};
		}

		loaded = true;
	}

	public void SetVideoClip()
	{
		videoPlayer.clip = operation.Result;
		videoPlayer.Play();
	}
}
