#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Kill", fileName = "Kill")]
public class StatusEffectInstantKill : StatusEffectInstant
{
	[SerializeField]
	public TargetConstraint[] killConstraints;

	[SerializeField]
	public string[] statusesToClear = new string[3] { "block", "shell", "scrap" };

	public override IEnumerator Process()
	{
		Routine.Clump clump = new Routine.Clump();
		string[] array = statusesToClear;
		foreach (string text in array)
		{
			StatusEffectData statusEffectData = target.FindStatus(text);
			if (statusEffectData != null)
			{
				clump.Add(statusEffectData.Remove());
			}
		}

		yield return clump.WaitForEnd();
		if (killConstraints == null || killConstraints.Length == 0 || killConstraints.Any((TargetConstraint a) => a.Check(target)))
		{
			target.forceKill = DeathType.Normal;
		}

		yield return base.Process();
	}
}
