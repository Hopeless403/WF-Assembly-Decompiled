#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardDestroyed : FlyOffScreen, ICardDestroyed, IRemoveWhenPooled
{
	public AngleWobbler[] wobblers;

	public override void Begin()
	{
		wobblers = GetComponentsInChildren<AngleWobbler>();
		AngleWobbler[] array = wobblers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].globalSpace = false;
		}
	}

	public override void End()
	{
		Final();
	}

	public void Final()
	{
		Object.Destroy(this);
		CardManager.ReturnToPool(base.gameObject.GetComponent<Card>());
	}

	public void OnDisable()
	{
		AngleWobbler[] array = wobblers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].globalSpace = true;
		}

		wobblers = null;
	}
}
