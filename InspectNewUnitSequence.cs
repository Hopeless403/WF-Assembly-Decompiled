#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class InspectNewUnitSequence : UISequence
{
	[Header("Custom Values")]
	public bool takeCard;

	public Transform cardHolder;

	[SerializeField]
	public SpeechBubble speechBubble;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString[] defaultGreetMessages;

	[SerializeField]
	public Color nameHighlightColour;

	[SerializeField]
	public Color nameTextColour;

	public Entity unit;

	public string greeting;

	public Transform unitPreParent;

	public Vector3 unitPrePosition;

	public Vector3 unitPreRotation;

	public Vector3 unitPreScale;

	[SerializeField]
	public float unitMoveWobble = 1.5f;

	[SerializeField]
	public AnimationCurve unitMoveCurve;

	[SerializeField]
	public float unitMoveDur = 0.33f;

	public CardSelector cardSelector;

	[SerializeField]
	public RenameCompanionSequence renameSequence;

	public bool destroyOnEnd;

	public void SetUnit(Entity unit)
	{
		SetUnit(unit, updateGreeting: true);
	}

	public void SetUnit(Entity unit, bool updateGreeting)
	{
		this.unit = unit;
		if (updateGreeting && speechBubble != null)
		{
			int num = unit.data.greetMessages.Length;
			greeting = ((num > 0) ? unit.data.greetMessages.RandomItem() : defaultGreetMessages.RandomItem().GetLocalizedString());
		}

		UpdateUnit();
	}

	public void UpdateUnit()
	{
		if (!(speechBubble == null))
		{
			string text = unit.data.title;
			if (nameTextColour.a > 0f)
			{
				text = "<color=#" + nameTextColour.ToHexRGBA() + ">" + text + "</color>";
			}

			if (nameHighlightColour.a > 0f)
			{
				text = "<mark=#" + nameHighlightColour.ToHexRGBA() + ">" + text + "</mark>";
			}

			string text2 = greeting.Replace("<name>", text);
			speechBubble.SetText(text2);
		}
	}

	public void UnsetUnit()
	{
		unit = null;
	}

	public override IEnumerator Run()
	{
		UIAnchors anchors = GetComponent<UIAnchors>();
		UIAnchors.AnchorPoint[] list = anchors.list;
		foreach (UIAnchors.AnchorPoint obj in list)
		{
			obj.Deactivate();
			obj.SetUp();
		}

		yield return Sequences.Wait(startDelay);
		Events.InvokeInspectNewCard(unit);
		Events.InvokeEntityFocus(unit);
		if (base.gameObject.activeSelf)
		{
			GetComponent<UINavigationLayer>().OnEnable();
		}

		base.gameObject.SetActive(value: true);
		if (unitMoveWobble != 0f && unit.wobbler != null)
		{
			unit.wobbler.WobbleRandom(unitMoveWobble);
		}

		Transform transform = unit.transform;
		unitPreParent = transform.parent;
		unitPrePosition = transform.localPosition;
		unitPreRotation = transform.localEulerAngles;
		unitPreScale = transform.localScale;
		transform.SetParent(cardHolder, worldPositionStays: true);
		LeanTween.cancel(unit.gameObject);
		LeanTween.moveLocal(unit.gameObject, Vector3.zero, unitMoveDur).setEase(unitMoveCurve);
		LeanTween.rotateLocal(unit.gameObject, Vector3.zero, unitMoveDur).setEase(unitMoveCurve);
		LeanTween.scale(unit.gameObject, Vector3.one, unitMoveDur).setEase(unitMoveCurve);
		int c = anchors.Count;
		for (int i = 0; i < c; i++)
		{
			anchors.Activate(i);
			yield return null;
			StartCoroutine(anchors.Reveal(i));
			yield return Sequences.Wait(delayBetween);
		}

		StartCoroutine(anchors.UpdatePositions());
		while (!promptEnd)
		{
			yield return null;
		}

		promptEnd = false;
		MonoBehaviourSingleton<UINavigationSystem>.instance.RemoveActiveLayer();
		if (!takeCard && unit != null)
		{
			unit.transform.SetParent(unitPreParent, worldPositionStays: true);
			LeanTween.cancel(unit.gameObject);
			LeanTween.moveLocal(unit.gameObject, unitPrePosition, unitMoveDur).setEase(unitMoveCurve);
			LeanTween.rotateLocal(unit.gameObject, unitPreRotation, unitMoveDur).setEase(unitMoveCurve);
			LeanTween.scale(unit.gameObject, unitPreScale, unitMoveDur).setEase(unitMoveCurve);
			if (unitMoveWobble != 0f && unit.wobbler != null)
			{
				unit.wobbler.WobbleRandom(unitMoveWobble);
			}
		}

		if (destroyOnEnd)
		{
			base.gameObject.Destroy();
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void TakeCard()
	{
		if (!takeCard)
		{
			End();
			if (unit != null)
			{
				takeCard = true;
				cardSelector.TakeCard(unit);
			}
		}
	}

	public void StartRename()
	{
		if (!takeCard)
		{
			renameSequence.SetUnit(unit);
			renameSequence.Begin();
		}
	}
}
