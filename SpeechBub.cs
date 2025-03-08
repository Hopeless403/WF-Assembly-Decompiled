#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class SpeechBub : MonoBehaviourRect
{
	[SerializeField]
	public TMP_Text textAsset;

	[SerializeField]
	public Transform tail;

	[SerializeField]
	public float minHeight = 1f;

	[SerializeField]
	public float tailOffsetX = -0.225f;

	[SerializeField]
	public float tailOffsetY = -0.125f;

	[SerializeField]
	public float tailAngleOffset = 45f;

	[SerializeField]
	public bool constantAngle = true;

	[SerializeField]
	public string highlightHex = "328AD7";

	[Header("Animations")]
	[SerializeField]
	public TweenUI destroyTween;

	[SerializeField]
	public AnimationCurve moveCurve;

	[SerializeField]
	public Vector2 moveDurRange = new Vector2(0.75f, 1f);

	public float tailAnchorX = 1f;

	public float tailAnchorY = -1f;

	public Transform target;

	public float duration;

	public float durationMax;

	public bool _hasTail = true;

	public bool destroying;

	public float durationFactor => duration / durationMax;

	public bool hasTail
	{
		get
		{
			return _hasTail;
		}
		set
		{
			_hasTail = value;
			tail.gameObject.SetActive(value);
		}
	}

	public bool sizeUpdated { get; set; }

	public void OnEnable()
	{
		SfxSystem.OneShot("event:/sfx/ui/speech_bubble");
	}

	public void Set(SpeechBubbleData data)
	{
		textAsset.text = ProcessText(data.text);
		target = data.target;
		hasTail = target != null;
		duration = data.duration;
		durationMax = data.duration;
		StartCoroutine(UpdateSizeNextFrame());
	}

	public void SetPosition(Vector3 localPos)
	{
		sizeUpdated = false;
		LeanTween.cancel(base.gameObject);
		LeanTween.moveLocal(base.gameObject, localPos, moveDurRange.PettyRandom()).setEase(moveCurve);
	}

	[Button(null, EButtonEnableMode.Always)]
	public void UpdateSize()
	{
		float num = textAsset.margin.y + textAsset.margin.w;
		float value = Mathf.Max(minHeight, textAsset.textBounds.size.y + num);
		base.rectTransform.sizeDelta = base.rectTransform.sizeDelta.WithY(value);
		if (hasTail)
		{
			Vector2 sizeDelta = base.rectTransform.sizeDelta;
			float x = tailAnchorX * 0.5f * sizeDelta.x + tailOffsetX * tailAnchorX;
			float y = tailAnchorY * 0.5f * sizeDelta.y + tailOffsetY * tailAnchorY;
			tail.localPosition = new Vector2(x, y);
		}
	}

	public IEnumerator UpdateSizeNextFrame()
	{
		yield return null;
		UpdateSize();
		sizeUpdated = true;
	}

	public void LateUpdate()
	{
		if (!destroying)
		{
			Transform transform = base.transform;
			if (constantAngle && transform.eulerAngles.z != 0f)
			{
				transform.eulerAngles = transform.eulerAngles.WithZ(0f);
			}

			duration -= Time.deltaTime;
			if (duration <= 0f)
			{
				destroying = true;
				StartCoroutine(Destroy());
			}

			if (hasTail && target != null)
			{
				tail.eulerAngles = tail.eulerAngles.WithZ(Angle(tail.position, target.position) + tailAngleOffset);
			}
		}
	}

	public static float Angle(Vector2 from, Vector2 to)
	{
		Vector2 to2 = to - from;
		int num = ((to2.y >= 0f) ? 1 : (-1));
		return Vector2.Angle(Vector2.right, to2) * (float)num;
	}

	public IEnumerator Destroy()
	{
		destroyTween.Fire();
		yield return new WaitForSeconds(destroyTween.GetDuration());
		base.gameObject.Destroy();
	}

	public string ProcessText(string input)
	{
		string text = input.Trim();
		int length = text.Length;
		for (int i = 0; i < length; i++)
		{
			if (text[i] != '<')
			{
				continue;
			}

			string text2 = Text.FindTag(text, i);
			if (text2.Length <= 0)
			{
				continue;
			}

			string text3 = highlightHex;
			text = text.Remove(i, text2.Length + 2);
			int num = text2.IndexOf(' ');
			if (num > 0)
			{
				string text4 = text2.Substring(0, num);
				if (text4[0] == '#')
				{
					text3 = text4.Substring(1);
					text2 = text2.Substring(num);
				}
			}

			string text5 = "<#" + text3 + ">" + text2 + "</color>";
			text = text.Insert(i, text5);
			i += text5.Length;
			length = text.Length;
			i--;
		}

		return text;
	}
}
