#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class GainCompanionSequence : UISequence
{
	public Entity target;

	public void SetTarget(Entity target)
	{
		this.target = target;
	}

	public override IEnumerator Run()
	{
		yield return null;
	}
}
