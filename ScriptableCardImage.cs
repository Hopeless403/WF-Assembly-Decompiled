#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ScriptableCardImage : MonoBehaviour
{
	public Entity entity;

	public void Assign(Entity entity)
	{
		this.entity = entity;
		AssignEvent();
	}

	public virtual void AssignEvent()
	{
	}

	public virtual void UpdateEvent()
	{
	}
}
