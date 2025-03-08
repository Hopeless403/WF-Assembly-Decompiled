#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class WorldSpaceCanvasScaler : WorldSpaceCanvasUpdater
{
	[SerializeField]
	public bool invert;

	[Header("Position")]
	[SerializeField]
	public bool scalePositionX;

	[SerializeField]
	[ShowIf("scalePositionX")]
	public float basePositionX;

	[SerializeField]
	public bool scalePositionY;

	[SerializeField]
	[ShowIf("scalePositionY")]
	public float basePositionY;

	[SerializeField]
	public bool scalePositionZ;

	[SerializeField]
	[ShowIf("scalePositionZ")]
	public float basePositionZ;

	[Header("Scale")]
	[SerializeField]
	public bool scaleX = true;

	[SerializeField]
	[ShowIf("scaleX")]
	public float baseScaleX = 1f;

	[SerializeField]
	public bool scaleY = true;

	[SerializeField]
	[ShowIf("scaleY")]
	public float baseScaleY = 1f;

	[SerializeField]
	public bool scaleZ;

	[SerializeField]
	[ShowIf("scaleZ")]
	public float baseScaleZ = 1f;

	public override void UpdateSize()
	{
		if (WorldSpaceCanvasFitScreenSystem.exists)
		{
			float num = 11.547f * WorldSpaceCanvasFitScreenSystem.instance.aspectRatio;
			float num2 = ((num < 17.32051f) ? (num / 17.32051f) : 1f);
			if (scalePositionX || scalePositionY || scalePositionZ)
			{
				SetPosition(num2);
			}

			if (scaleX || scaleY || scaleZ)
			{
				SetScale(num2);
			}
		}
	}

	public void SetPosition(float scale)
	{
		Vector3 anchoredPosition3D = base.rectTransform.anchoredPosition3D;
		if (scalePositionX)
		{
			anchoredPosition3D.x = (invert ? (basePositionX / scale) : (basePositionX * scale));
		}

		if (scalePositionY)
		{
			anchoredPosition3D.y = (invert ? (basePositionY / scale) : (basePositionY * scale));
		}

		if (scalePositionZ)
		{
			anchoredPosition3D.z = (invert ? (basePositionZ / scale) : (basePositionZ * scale));
		}

		base.rectTransform.anchoredPosition3D = anchoredPosition3D;
	}

	public void SetScale(float scale)
	{
		Vector3 localScale = base.rectTransform.localScale;
		if (scaleX)
		{
			localScale.x = (invert ? (baseScaleX / scale) : (baseScaleX * scale));
		}

		if (scaleY)
		{
			localScale.y = (invert ? (baseScaleY / scale) : (baseScaleY * scale));
		}

		if (scaleZ)
		{
			localScale.z = (invert ? (baseScaleZ / scale) : (baseScaleZ * scale));
		}

		base.rectTransform.localScale = localScale;
	}
}
