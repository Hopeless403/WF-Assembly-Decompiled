#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ImageCutout : ImageSprite
{
	public override Material materialForRendering
	{
		get
		{
			if (!base.maskable)
			{
				return base.materialForRendering;
			}

			Material obj = new Material(base.materialForRendering);
			obj.SetInt("_StencilComp", 6);
			return obj;
		}
	}
}
