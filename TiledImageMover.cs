#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class TiledImageMover : MonoBehaviourCacheTransform
{
	[SerializeField]
	public float moveX = 1f;

	[SerializeField]
	public float resetPosX = 0.5f;

	public void Update()
	{
		Vector3 localPosition = base.transform.localPosition;
		float num = localPosition.x + moveX * Time.deltaTime;
		if (num >= resetPosX)
		{
			num -= resetPosX * 2f;
		}
		else if (num <= 0f - resetPosX)
		{
			num += resetPosX * 2f;
		}

		base.transform.localPosition = localPosition.WithX(num);
	}
}
