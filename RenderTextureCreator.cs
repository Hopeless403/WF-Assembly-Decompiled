#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;

public class RenderTextureCreator : MonoBehaviour
{
	[SerializeField]
	public int width;

	[SerializeField]
	public int height;

	[SerializeField]
	public GraphicsFormat colorFormat;

	[SerializeField]
	public GraphicsFormat depthStencilFormat;

	[SerializeField]
	public int mipCount;

	[SerializeField]
	public bool destroyOnDisable = true;

	[SerializeField]
	public UnityEvent<RenderTexture> onCreate;

	public RenderTexture rt;

	public void OnEnable()
	{
		if (rt != null)
		{
			rt.Destroy();
		}

		rt = new RenderTexture(width, height, colorFormat, depthStencilFormat, mipCount);
		onCreate.Invoke(rt);
	}

	public void OnDisable()
	{
		if (destroyOnDisable && rt != null)
		{
			rt.Destroy();
		}
	}
}
