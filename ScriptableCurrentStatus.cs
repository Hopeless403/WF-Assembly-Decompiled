#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Amount/Current Status", fileName = "CurrentStatus")]
public class ScriptableCurrentStatus : ScriptableAmount
{
	[SerializeField]
	public string statusType = "shroom";

	[SerializeField]
	public int offset;

	[SerializeField]
	public float multiplier = 1f;

	[SerializeField]
	public bool roundUp;

	public override int Get(Entity entity)
	{
		int num;
		if (!entity)
		{
			num = offset;
		}
		else
		{
			StatusEffectData statusEffectData = entity.FindStatus(statusType);
			num = (((object)statusEffectData != null) ? (statusEffectData.count + offset) : offset);
		}

		int amount = num;
		return Mult(amount);
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
