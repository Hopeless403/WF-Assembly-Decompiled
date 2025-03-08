#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Multiply Health", menuName = "Card Scripts/Multiply Health")]
public class CardScriptMultiplyHealth : CardScript
{
	[SerializeField]
	public float multiply;

	[SerializeField]
	public bool roundUp;

	public override void Run(CardData target)
	{
		target.hp = (roundUp ? Mathf.CeilToInt((float)target.hp * multiply) : Mathf.RoundToInt((float)target.hp * multiply));
		target.hp = Mathf.Max(1, target.hp);
	}
}
