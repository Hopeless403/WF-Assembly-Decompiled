#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using DeadExtensions;
using UnityEngine;

public class Blink : MonoBehaviour
{
	public Vector2 onRange = new Vector2(5f, 6f);

	public Vector2 offRange = new Vector2(0.1f, 0.1f);

	public AnimationCurve blinkCurve;

	public AnimationCurve unblinkCurve;

	public float blinkCurveDuration = 0.1f;

	[SerializeField]
	public bool startOn = true;

	public bool on;

	public float timer;

	public float preScaleY;

	public void Awake()
	{
		on = startOn;
		if (on)
		{
			timer = onRange.PettyRandom() * PettyRandom.value;
			return;
		}

		Transform obj = base.transform;
		Vector3 localScale = obj.localScale;
		preScaleY = localScale.y;
		localScale.y = 0f;
		obj.localScale = localScale;
		timer = offRange.PettyRandom() * PettyRandom.value;
	}

	public void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			Toggle();
		}
	}

	public void Toggle()
	{
		on = !on;
		LeanTween.cancel(base.gameObject);
		if (on)
		{
			LeanTween.scaleY(base.gameObject, preScaleY, blinkCurveDuration).setEase(unblinkCurve);
		}
		else
		{
			preScaleY = base.transform.localScale.y;
			LeanTween.scaleY(base.gameObject, 0f, blinkCurveDuration).setEase(blinkCurve);
		}

		if (on)
		{
			timer = onRange.PettyRandom();
		}
		else
		{
			timer = offRange.PettyRandom();
		}
	}
}
