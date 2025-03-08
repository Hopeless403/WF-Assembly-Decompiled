#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class ActionMove : PlayAction
{
	public readonly Entity entity;

	public CardContainer[] toContainers;

	public int insertPos = -1;

	public bool tweenAll = true;

	public ActionMove(Entity entity, params CardContainer[] toContainers)
	{
		this.entity = entity;
		this.toContainers = toContainers;
	}

	public ActionMove(Entity entity, CardContainer[] toContainers, int insertPos)
	{
		this.entity = entity;
		this.toContainers = toContainers;
		this.insertPos = insertPos;
	}

	public override IEnumerator Run()
	{
		if (entity.IsAliveAndExists() && toContainers != null)
		{
			yield return Sequences.CardMove(entity, toContainers, insertPos, tweenAll);
		}
	}
}
