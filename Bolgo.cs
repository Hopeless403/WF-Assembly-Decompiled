#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class Bolgo : ScriptableCardImage
{
	[SerializeField]
	public Image image;

	[Header("Scale Based On Damage")]
	[SerializeField]
	public AnimationCurve scaleCurve;

	[SerializeField]
	public AnimationCurve tweenCurve;

	[SerializeField]
	public float tweenDur;

	[Header("Set Sprite Based On Shell")]
	[SerializeField]
	public Sprite[] sprites;

	[SerializeField]
	public AnimationCurve spriteIndexCurve;

	public int currentShell;

	public bool currentDamageSet;

	public int currentDamage;

	public float scaleFrom = 1f;

	public float scaleTo = 1f;

	public float tweenT;

	public bool tween;

	public override void UpdateEvent()
	{
		if (!currentDamageSet || currentDamage != entity.damage.current)
		{
			currentDamageSet = true;
			currentDamage = entity.damage.current;
			SetScale();
		}

		int num = entity.FindStatus("shell")?.count ?? 0;
		if (num != currentShell)
		{
			currentShell = num;
			int num2 = Mathf.RoundToInt(Mathf.Clamp(spriteIndexCurve.Evaluate(currentShell), 0f, 1f) * ((float)sprites.Length - 1f));
			image.sprite = sprites[num2];
		}
	}

	public void SetScale()
	{
		scaleFrom = Mathf.Lerp(1f, image.transform.localScale.x, 0.5f);
		scaleTo = scaleCurve.Evaluate(currentDamage);
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
