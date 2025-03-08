#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldDisplay : MonoBehaviour
{
	[SerializeField]
	public Image icon;

	[SerializeField]
	public bool doPing = true;

	[SerializeField]
	public LeanTweenType pingEase = LeanTweenType.easeOutElastic;

	[SerializeField]
	[ShowIf("PingIsAnimationCurve")]
	public AnimationCurve pingCurve;

	[SerializeField]
	public float pingDur = 1f;

	[SerializeField]
	public TMP_Text textAsset;

	[SerializeField]
	[TextArea]
	public string format = "{0}";

	[SerializeField]
	[TextArea]
	public string formatAdd = "{0}<#ffff00>+{1}";

	[SerializeField]
	[TextArea]
	public string formatSub = "{0}<#e05822>-{1}";

	[SerializeField]
	[TextArea]
	public string formatChangeUp = "<s>{0}</s><#ffff00> {2}";

	[SerializeField]
	[TextArea]
	public string formatChangeDown = "<s>{0}</s><#e05822> {2}";

	[SerializeField]
	public float addDelay = 1f;

	[SerializeField]
	public float totalAddTime = 1f;

	[SerializeField]
	public float addBetweenTimeMax = 0.05f;

	public int goldPre;

	public float current;

	public float change;

	public bool PingIsAnimationCurve => pingEase == LeanTweenType.animationCurve;

	public float add { get; set; }

	public void Set(int amount)
	{
		if (amount != goldPre)
		{
			add += amount - goldPre;
			UpdateText();
			ResolveAfter(addDelay);
			if (doPing && pingDur > 0f && pingEase != 0)
			{
				Ping();
			}
		}

		goldPre = amount;
	}

	public void ShowChange(int change)
	{
		this.change = change;
		UpdateText();
	}

	public void HideChange()
	{
		change = 0f;
		UpdateText();
	}

	public void Ping()
	{
		LeanTween.cancel(base.gameObject);
		LTDescr lTDescr = LeanTween.scale(base.gameObject, Vector3.one, pingDur).setFrom(Vector3.zero);
		if (PingIsAnimationCurve)
		{
			lTDescr.setEase(pingCurve);
		}
		else
		{
			lTDescr.setEase(pingEase);
		}
	}

	public void UpdateText()
	{
		if (change == 0f)
		{
			if (add == 0f)
			{
				textAsset.text = string.Format(format, current);
			}
			else if (add > 0f)
			{
				textAsset.text = string.Format(formatAdd, current, add);
			}

			else if (add < 0f)
			{
				textAsset.text = string.Format(formatSub, current, 0f - add);
			}
		}
		else if (change > 0f)
		{
			textAsset.text = string.Format(formatChangeUp, current, change, current + change);
		}

		else if (change < 0f)
		{
			textAsset.text = string.Format(formatChangeDown, current, change, current + change);
		}
	}

	public void ResolveAfter(float delay)
	{
		StopAllCoroutines();
		StartCoroutine(ResolveAdd(delay));
	}

	public IEnumerator ResolveAdd(float delay)
	{
		yield return Sequences.Wait(delay);
		float timeBetween = Mathf.Min(totalAddTime / Mathf.Abs(add), addBetweenTimeMax);
		Events.InvokeGoldCounterStart(this, add);
		while (add != 0f)
		{
			if (add > 0f)
			{
				add--;
				current += 1f;
			}
			else
			{
				add++;
				current -= 1f;
			}

			UpdateText();
			yield return Sequences.Wait(timeBetween);
		}
	}
}
