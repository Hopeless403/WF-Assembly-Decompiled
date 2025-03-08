#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Set Counter", menuName = "Card Scripts/Set Counter")]
public class CardScriptSetCounter : CardScript
{
	[SerializeField]
	public Vector2Int counterRange;

	public override void Run(CardData target)
	{
		if (target.counter >= 1)
		{
			target.counter = counterRange.Random();
			target.counter = Mathf.Max(1, target.counter);
		}
	}
}
