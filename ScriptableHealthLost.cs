#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Amount/Health Lost", fileName = "HealthLost")]
public class ScriptableHealthLost : ScriptableAmount
{
	public override int Get(Entity entity)
	{
		if (!entity)
		{
			return 0;
		}

		return entity.lastHit?.damageDealt ?? 0;
	}
}
