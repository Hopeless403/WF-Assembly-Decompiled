#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Amount/Fixed Amount", fileName = "One")]
public class ScriptableFixedAmount : ScriptableAmount
{
	[SerializeField]
	public int amount = 1;

	public override int Get(Entity entity)
	{
		return amount;
	}
}
