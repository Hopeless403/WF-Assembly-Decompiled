#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Amount/Current Attack", fileName = "CurrentAttack")]
public class ScriptableCurrentAttack : ScriptableAmount
{
	[SerializeField]
	public float multiplier = 1f;

	[SerializeField]
	public bool roundUp;

	public override int Get(Entity entity)
	{
		if (!entity)
		{
			return 0;
		}

		return Mult(entity.damage.current + entity.tempDamage.Value);
	}

	public int Mult(int amount)
	{
		if (!roundUp)
		{
			return Mathf.FloorToInt((float)amount * multiplier);
		}

		return Mathf.RoundToInt((float)amount * multiplier);
	}
}
