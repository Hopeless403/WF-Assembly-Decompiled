#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ChallengeStone : MonoBehaviour
{
	public ChallengeData challenge;

	[SerializeField]
	public GameObject door;

	[SerializeField]
	public CardCharmHolder charmHolder;

	[SerializeField]
	public UINavigationItem navItem;

	[SerializeField]
	public LocalizeStringEvent title;

	[SerializeField]
	public LocalizeStringEvent text;

	public static readonly Vector2 popUpOffset = new Vector2(1f, 0f);

	public static readonly Vector4 raycastPadding = new Vector4(0f, 0f, 0f, 0f);

	public void OnEnable()
	{
		if ((bool)challenge)
		{
			title.StringReference = challenge.titleKey;
			if (!challenge.hidden)
			{
				text.StringReference = challenge.textKey;
			}
		}
	}

	public void Open(CardUpgradeData upgradeData)
	{
		navItem.enabled = false;
		door.SetActive(value: false);
		UpgradeDisplay upgradeDisplay = charmHolder.Create(upgradeData);
		Image component = upgradeDisplay.GetComponent<Image>();
		if ((object)component != null)
		{
			component.raycastPadding = raycastPadding;
		}

		CardCharmInteraction component2 = upgradeDisplay.GetComponent<CardCharmInteraction>();
		if ((object)component2 != null)
		{
			component2.popUpOffset = popUpOffset;
			component2.canDrag = false;
		}

		if (challenge.hidden)
		{
			text.StringReference = challenge.textKey;
		}
	}
}
