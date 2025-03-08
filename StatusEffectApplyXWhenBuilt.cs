#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Built", fileName = "Apply X When Built")]
public class StatusEffectApplyXWhenBuilt : StatusEffectApplyX
{
	public override void Init()
	{
		base.OnBuild += Build;
	}

	public override bool RunBuildEvent(Entity entity)
	{
		return entity == target;
	}

	public IEnumerator Build(Entity entity)
	{
		yield return Run(GetTargets());
		yield return Remove();
	}
}
