#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class Fader : MonoBehaviour
{
	public Graphic _graphic;

	[SerializeField]
	public Gradient gradient;

	[SerializeField]
	public bool startFadedIn;

	[SerializeField]
	public LeanTweenType ease = LeanTweenType.easeInOutQuad;

	[SerializeField]
	public float delay;

	[SerializeField]
	public float dur = 0.5f;

	[SerializeField]
	public bool onAwake;

	[SerializeField]
	public bool onEnable;

	[SerializeField]
	public bool loop;

	[SerializeField]
	public bool ignoreTimeScale;

	public float current;

	public Graphic graphic => _graphic ?? (_graphic = GetComponent<Graphic>());

	public void Awake()
	{
		Set(startFadedIn ? 1f : 0f);
		if (onAwake)
		{
			In();
		}
	}

	public void OnEnable()
	{
		if (onEnable)
		{
			Set(0f);
			In();
		}
	}

	public void Set(float value)
	{
		current = value;
		graphic.color = gradient.Evaluate(value);
	}

	public void In()
	{
		Tween(1f, dur, ease);
	}

	public void In(float dur)
	{
		Tween(1f, dur, ease);
	}

	public void In(float dur, LeanTweenType ease)
	{
		Tween(1f, dur, ease);
	}

	public void Out()
	{
		Tween(0f, dur, ease);
	}

	public void Out(float dur)
	{
		Tween(0f, dur, ease);
	}

	public void Out(float dur, LeanTweenType ease)
	{
		Tween(0f, dur, ease);
	}

	public void Tween(float to, float dur, LeanTweenType ease)
	{
		LeanTween.cancel(base.gameObject);
		LTDescr lTDescr = LeanTween.value(base.gameObject, current, to, dur).setDelay(delay).setEase(ease)
			.setOnUpdate(Set)
			.setIgnoreTimeScale(ignoreTimeScale);
		if (loop)
		{
			lTDescr.setLoopCount(-1);
		}
	}
}
