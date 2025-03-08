#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public abstract class PlayAction
{
	public float pauseBefore;

	public float pauseAfter;

	public int priority;

	public bool fixedPosition;

	public bool parallel;

	public string note;

	public virtual bool IsRoutine => true;

	public virtual string Name => GetType().Name + (note.IsNullOrWhitespace() ? "" : (" [" + note + "]"));

	public virtual void Process()
	{
	}

	public virtual IEnumerator Run()
	{
		return null;
	}

	public PlayAction()
	{
	}
}
