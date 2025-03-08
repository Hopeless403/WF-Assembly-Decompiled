#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Multiply Counter", menuName = "Card Scripts/Multiply Counter")]
public class CardScriptMultiplyCounter : CardScript
{
	[SerializeField]
	public float multiply;

	[SerializeField]
	public bool roundUp;

	public override void Run(CardData target)
	{
		target.counter = (roundUp ? Mathf.CeilToInt((float)target.counter * multiply) : Mathf.RoundToInt((float)target.counter * multiply));
		target.counter = Mathf.Max(1, target.counter);
	}
}
