#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Amount/Cards In Hand", fileName = "CardsInHand")]
public class ScriptableCardsInHand : ScriptableAmount
{
	public override int Get(Entity entity)
	{
		return References.Player.handContainer.Count;
	}
}
