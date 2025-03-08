#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderPetNoomlin : ItemHolderPet
{
	[SerializeField]
	public ItemHolderPetUsed usedPrefab;

	[SerializeField]
	public TweenUI showTween;

	[SerializeField]
	public AngleWobbler headWobbler;

	[Header("Head")]
	[SerializeField]
	public Sprite[] headOptions;

	[SerializeField]
	public Image head;

	[Header("SFX")]
	[SerializeField]
	public EventReference showSfx;

	[SerializeField]
	public EventReference usedSfx;

	public override void Show()
	{
		base.Show();
		if ((bool)head && headOptions.Length != 0)
		{
			int num = Mathf.RoundToInt((float)headOptions.Length * owner.data.random3.x).Mod(headOptions.Length);
			head.sprite = headOptions[num];
		}

		showTween.Fire();
		headWobbler.WobbleRandom();
		if (!showSfx.IsNull && owner.inPlay)
		{
			SfxSystem.OneShot(showSfx);
		}
	}

	public override void Used()
	{
		base.Used();
		ItemHolderPetUsed itemHolderPetUsed = Object.Instantiate(usedPrefab, null);
		itemHolderPetUsed.transform.position = base.transform.position;
		itemHolderPetUsed.transform.eulerAngles = base.transform.eulerAngles;
		itemHolderPetUsed.SetUp(head.sprite);
		if (!usedSfx.IsNull && owner.inPlay)
		{
			SfxSystem.OneShot(usedSfx);
		}
	}
}
