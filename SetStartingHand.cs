#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

public class SetStartingHand : MonoBehaviour
{
	[SerializeField]
	public CardData[] startingHand;

	public void OnEnable()
	{
		Events.OnBattleStart += BattleStart;
	}

	public void OnDisable()
	{
		Events.OnBattleStart -= BattleStart;
	}

	public void BattleStart()
	{
		References.Player.OrderNextCards(startingHand.Select((CardData a) => a.name).ToArray());
	}
}
