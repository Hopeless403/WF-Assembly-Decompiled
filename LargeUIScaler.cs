#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class LargeUIScaler : WorldSpaceCanvasUpdater
{
	[SerializeField]
	public bool setScale = true;

	[SerializeField]
	[ShowIf("setScale")]
	[FormerlySerializedAs("scaleMinV")]
	public Vector3 scaleMin = Vector3.one;

	[SerializeField]
	[ShowIf("setScale")]
	[FormerlySerializedAs("scaleMaxV")]
	public Vector3 scaleMax = Vector3.one;

	[SerializeField]
	public bool setPosition;

	[SerializeField]
	[ShowIf("setPosition")]
	public Vector3 positionMin;

	[SerializeField]
	[ShowIf("setPosition")]
	public Vector3 positionMax;

	[SerializeField]
	public AnimationCurve aspectRatioInfluence;

	public override void UpdateSize()
	{
		if (setScale || setPosition)
		{
			float num = 0f;
			if (aspectRatioInfluence.length > 0)
			{
				num *= aspectRatioInfluence.Evaluate(WorldSpaceCanvasFitScreenSystem.AspectRatio);
			}

			if (setScale)
			{
				base.rectTransform.localScale = Vector3.Lerp(scaleMin, scaleMax, num);
			}

			if (setPosition)
			{
				base.rectTransform.anchoredPosition3D = Vector3.Lerp(positionMin, positionMax, num);
			}
		}
	}
}
