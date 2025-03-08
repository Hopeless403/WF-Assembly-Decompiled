#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
	[SerializeField]
	public Character owner;

	public DeckDisplaySequence displaySequence;

	public CompanionLimitSequence companionLimitSequence;

	public CompanionRecoverSequence companionRecoverSequence;

	[SerializeField]
	public CardController[] cardControllers;

	[SerializeField]
	public CardContainer[] cardContainers;

	public UINavigationItem backButtonNavigationItem;

	public void SetOwner(Character owner)
	{
		this.owner = owner;
		displaySequence.owner = owner;
		CardController[] array = cardControllers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].owner = owner;
		}

		CardContainer[] array2 = cardContainers;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].owner = owner;
		}

		if (companionLimitSequence != null)
		{
			companionLimitSequence.owner = owner;
		}

		if (companionRecoverSequence != null)
		{
			companionRecoverSequence.owner = owner;
		}
	}
}
