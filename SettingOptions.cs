#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SettingOptions : Setting<int>
{
	[SerializeField]
	public TMP_Dropdown dropdown;

	[SerializeField]
	public GameObject tweener;

	[SerializeField]
	public AnimationCurve tweenCurve;

	[SerializeField]
	public float tweenDur = 0.33f;

	[SerializeField]
	public UnityEvent<int> onValueChanged;

	public override void SetValue(int value)
	{
		if (value < 0)
		{
			value += dropdown.options.Count;
		}

		value %= dropdown.options.Count;
		dropdown.value = value;
	}

	public void Add(float single)
	{
		if (single > 0f)
		{
			Increase();
		}
		else if (single < 0f)
		{
			Decrease();
		}
	}

	public void Increase()
	{
		SetValue(dropdown.value + 1);
		Tween(1);
		onValueChanged?.Invoke(dropdown.value);
	}

	public void Decrease()
	{
		SetValue(dropdown.value - 1);
		Tween(-1);
		onValueChanged?.Invoke(dropdown.value);
	}

	public void Tween(int dir)
	{
		LeanTween.cancel(tweener);
		LeanTween.moveLocalX(tweener, 0.1f * (float)dir, tweenDur).setFrom(0f).setEase(tweenCurve)
			.setIgnoreTimeScale(useUnScaledTime: true);
	}
}
