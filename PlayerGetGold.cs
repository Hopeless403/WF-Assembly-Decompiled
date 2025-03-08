#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class PlayerGetGold : MonoBehaviour
{
	public void GetGold(int amount)
	{
		Character player = References.Player;
		if (player != null)
		{
			player.data.inventory.gold += amount;
			player.entity.PromptUpdate();
		}
	}
}
