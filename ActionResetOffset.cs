#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ActionResetOffset : PlayAction
{
	public readonly Entity entity;

	public const float dur = 0.33f;

	public const LeanTweenType ease = LeanTweenType.easeOutQuint;

	public override bool IsRoutine => false;

	public ActionResetOffset(Entity entity)
	{
		this.entity = entity;
	}

	public override void Process()
	{
		if (entity.IsAliveAndExists())
		{
			GameObject gameObject = entity.offset.gameObject;
			LeanTween.cancel(gameObject);
			LeanTween.scale(gameObject, Vector3.one, 0.33f).setEase(LeanTweenType.easeOutQuint);
			LeanTween.moveLocal(gameObject, Vector3.zero, 0.33f).setEase(LeanTweenType.easeOutQuint);
			LeanTween.rotateLocal(gameObject, Vector3.zero, 0.33f).setEase(LeanTweenType.easeOutQuint);
			entity.ResetDrawOrder();
		}
	}
}
