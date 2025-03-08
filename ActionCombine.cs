#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;

public class ActionCombine : PlayAction
{
	public readonly Entity[] entities;

	public readonly Entity finalEntity;

	public ActionCombine(Entity[] entities)
	{
		this.entities = entities;
		finalEntity = entities.Last();
	}

	public override IEnumerator Run()
	{
		if ((bool)CombineSystem.instance)
		{
			yield return CombineSystem.instance.Combine(entities, finalEntity);
		}
	}
}
