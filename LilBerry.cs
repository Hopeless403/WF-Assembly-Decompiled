#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class LilBerry : ScriptableCardImage
{
	[SerializeField]
	public Transform image;

	[SerializeField]
	public AnimationCurve scaleCurve;

	[Header("Scale Tween")]
	[SerializeField]
	public AnimationCurve tweenCurve;

	[SerializeField]
	public float tweenDur;

	public float scaleFrom = 1f;

	public float scaleTo = 1f;

	public float tweenT;

	public bool tween;

	public bool currentSet;

	public int current;

	public override void UpdateEvent()
	{
		if (!currentSet || current != entity.damage.current)
		{
			currentSet = true;
			current = entity.damage.current;
			SetScale();
		}
	}

	public void SetScale()
	{
		scaleFrom = Mathf.Lerp(1f, image.localScale.x, 0.5f);
		scaleTo = scaleCurve.Evaluate(current);
		StartScaleTween();
	}

	public void StartScaleTween()
	{
		tween = true;
		tweenT = 0f;
	}

	public void Update()
	{
		if (tween)
		{
			tweenT += Time.deltaTime / tweenDur;
			float num = scaleFrom + tweenCurve.Evaluate(tweenT) * (scaleTo - scaleFrom);
			image.transform.localScale = new Vector3(num, num, 1f);
			if (tweenT > 1f)
			{
				tween = false;
			}
		}
	}
}
