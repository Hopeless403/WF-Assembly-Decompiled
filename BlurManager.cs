#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class BlurManager : MonoBehaviour
{
	[SerializeField]
	public Camera blurCamera;

	[SerializeField]
	public Material blurMaterial;

	public void Start()
	{
		if (blurCamera.targetTexture != null)
		{
			blurCamera.targetTexture.Release();
		}

		blurCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
		blurMaterial.SetTexture("_MainTex", blurCamera.targetTexture);
	}
}
