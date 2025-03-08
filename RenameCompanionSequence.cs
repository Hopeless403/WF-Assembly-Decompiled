#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Steamworks;
using TMPro;
using UnityEngine;

public class RenameCompanionSequence : UISequence
{
	[Header("Custom Values")]
	[SerializeField]
	public Transform cardHolder;

	[SerializeField]
	public TMP_InputField inputField;

	public Entity unit;

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

	public bool endImmediate;

	public void OnEnable()
	{
		SteamUtils.OnGamepadTextInputDismissed += GamepadTextInputDismissed;
	}

	public void OnDisable()
	{
		SteamUtils.OnGamepadTextInputDismissed -= GamepadTextInputDismissed;
	}

	public void GamepadTextInputDismissed(bool success)
	{
		if (success)
		{
			inputField.text = SteamUtils.GetEnteredGamepadText();
			Confirm();
		}
		else
		{
			Cancel();
		}
	}

	public void SetUnit(Entity unit)
	{
		this.unit = unit;
		if (inputField.placeholder is TMP_Text tMP_Text)
		{
			tMP_Text.text = unit.data.title;
		}
	}

	public override IEnumerator Run()
	{
		endImmediate = false;
		inputField.text = "";
		UIAnchors anchors = GetComponent<UIAnchors>();
		UIAnchors.AnchorPoint[] list = anchors.list;
		foreach (UIAnchors.AnchorPoint obj in list)
		{
			obj.Deactivate();
			obj.SetUp();
		}

		yield return Sequences.Wait(startDelay);
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
		unit.transform.SetParent(cardHolder, worldPositionStays: true);
		LeanTween.cancel(unit.gameObject);
		LeanTween.moveLocal(unit.gameObject, Vector3.zero, unitMoveDur).setEase(unitMoveCurve);
		LeanTween.rotateLocal(unit.gameObject, Vector3.zero, unitMoveDur).setEase(unitMoveCurve);
		LeanTween.scale(unit.gameObject, Vector3.one, unitMoveDur).setEase(unitMoveCurve);
		SfxSystem.OneShot("event:/sfx/ui/card_renaming_open");
		int c = anchors.Count;
		for (int i = 0; i < c; i++)
		{
			anchors.Activate(i);
			yield return null;
			StartCoroutine(anchors.Reveal(i));
			yield return Sequences.Wait(delayBetween);
		}

		StartCoroutine(anchors.UpdatePositions());
		if (SteamManager.init)
		{
			SteamUtils.ShowGamepadTextInput(GamepadTextInputMode.Normal, GamepadTextInputLineMode.SingleLine, "", 20, unit.data.title);
		}

		while (!promptEnd)
		{
			yield return null;
		}

		promptEnd = false;
		if (!endImmediate && unit != null)
		{
			unit.transform.SetParent(unitPreParent, worldPositionStays: true);
			LeanTween.cancel(unit.gameObject);
			LeanTween.moveLocal(unit.gameObject, unitPrePosition, unitMoveDur).setEase(unitMoveCurve);
			LeanTween.rotateLocal(unit.gameObject, unitPreRotation, unitMoveDur).setEase(unitMoveCurve);
			LeanTween.scale(unit.gameObject, unitPreScale, unitMoveDur).setEase(unitMoveCurve);
		}

		base.gameObject.SetActive(value: false);
	}

	public void Confirm()
	{
		if (!inputField.text.IsNullOrWhitespace())
		{
			Entity entity = unit;
			string newName = inputField.text;
			if (Events.CheckRename(ref entity, ref newName))
			{
				SfxSystem.OneShot("event:/sfx/ui/card_renaming_accept");
				entity.data.forceTitle = newName;
				if (entity.display is Card card)
				{
					card.SetName(entity.data.title);
				}

				Events.InvokeRename(entity, newName);
			}
		}

		End();
	}

	public void Cancel()
	{
		End();
	}

	public void EndImmediate()
	{
		endImmediate = true;
		End();
	}
}
