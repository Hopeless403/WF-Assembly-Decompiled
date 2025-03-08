#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Campaign/Final Boss Card Modifier", fileName = "Final Boss Card Modifier")]
public class FinalBossCardModifier : ScriptableObject
{
	public CardData card;

	public CardScript[] runAll;

	public void Run(CardData card)
	{
		CardScript[] array = runAll;
		foreach (CardScript cardScript in array)
		{
			Debug.Log("Running [" + cardScript.name + "] on " + card.name);
			cardScript.Run(card);
		}
	}
}
