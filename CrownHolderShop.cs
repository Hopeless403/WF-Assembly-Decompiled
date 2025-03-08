#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class CrownHolderShop : MonoBehaviour
{
	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public GameObject crown;

	[SerializeField]
	public Image crownImage;

	[SerializeField]
	public Image crownWhiteImage;

	[SerializeField]
	public Image interaction;

	[SerializeField]
	public Vector2 popUpOffset = new Vector2(0.7f, 0.25f);

	public CardUpgradeData crownData;

	public string popUpName;

	public string popUpTitle;

	public string popUpBody;

	public bool hover;

	public bool _hasCrown = true;

	public bool hasCrown
	{
		get
		{
			return _hasCrown;
		}
		set
		{
			_hasCrown = value;
			crown.SetActive(value);
			interaction.enabled = value;
		}
	}

	public void SetCrownData(CardUpgradeData crownData)
	{
		this.crownData = crownData;
		crownImage.sprite = crownData.image;
		crownWhiteImage.sprite = crownData.image;
		popUpName = crownData.name;
		popUpTitle = crownData.title;
		popUpBody = crownData.text;
	}

	public CardUpgradeData GetCrownData()
	{
		return crownData;
	}

	public bool CanTake()
	{
		if (_hasCrown)
		{
			return base.enabled;
		}

		return false;
	}

	public void Hover()
	{
		if (!hover && CanTake())
		{
			hover = true;
			animator.SetBool("Hover", hover);
			CardPopUp.AssignTo(interaction.rectTransform, popUpOffset.x, popUpOffset.y);
			CardPopUp.AddPanel(popUpName, popUpTitle, popUpBody);
		}
	}

	public void UnHover()
	{
		if (hover)
		{
			hover = false;
			animator.SetBool("Hover", hover);
			CardPopUp.RemovePanel(popUpName);
		}
	}

	public void TakeCrown()
	{
		hasCrown = false;
	}
}
