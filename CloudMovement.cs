#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
	public Transform mover;

	public float speed = 0.1f;

	public float width = 20f;

	public void Awake()
	{
		Vector3 localPosition = mover.localPosition;
		localPosition.x = PettyRandom.Range(0f - width, width);
		mover.localPosition = localPosition;
	}

	public void Update()
	{
		Vector3 localPosition = mover.localPosition;
		localPosition.x += speed * Time.deltaTime;
		if (localPosition.x > width)
		{
			localPosition.x = 0f - width;
		}
		else if (localPosition.x < 0f - width)
		{
			localPosition.x = width;
		}

		mover.localPosition = localPosition;
	}
}
