#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[Serializable]
public struct Tween
{
	public LeanTweenType ease;

	public float dur;

	public bool hasFrom;

	public float scaleFrom;

	public float scaleTo;

	public void Run(GameObject gameObject)
	{
		LeanTween.cancel(gameObject);
		LeanTween.scale(gameObject, Vector3.one * scaleTo, dur).setEase(ease).setFrom(hasFrom ? (Vector3.one * scaleFrom) : gameObject.transform.localScale);
	}
}
