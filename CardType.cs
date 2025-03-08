#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CardType", menuName = "Card Type")]
public class CardType : DataFile
{
	public int sortPriority;

	[ShowAssetPreview(64, 64)]
	public Sprite icon;

	public AssetReference prefabRef;

	[Header("Details")]
	public Sprite textBoxSprite;

	public Sprite nameTagSprite;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleKey;

	public bool canDie;

	public bool canTakeCrown;

	public bool canRecall;

	public bool canReserve;

	public bool item;

	public bool unit;

	public string tag;

	public bool miniboss;

	public bool discoverInJournal;

	[Header("Colours")]
	public Text.ColourProfileHex descriptionColours;

	public string title => titleKey.GetLocalizedString();

	public override bool Equals(object other)
	{
		if (other is CardType cardType)
		{
			return base.name == cardType.name;
		}

		return base.Equals(other);
	}
}
