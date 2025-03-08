#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class RenameMidBattleSystem : MonoBehaviour
{
	public void OnEnable()
	{
		Events.OnRename += Rename;
	}

	public void OnDisable()
	{
		Events.OnRename -= Rename;
	}

	public void Rename(Entity entity, string newName)
	{
		foreach (Entity card2 in References.Battle.cards)
		{
			if (card2.inPlay && card2.data.id == entity.data.id && card2.display is Card card)
			{
				card.SetName(newName);
			}
		}
	}
}
