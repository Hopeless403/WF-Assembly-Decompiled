#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class Mover : MonoBehaviour
{
	public Vector3 velocity;

	public float frictMult = 0.9f;

	public bool removeWhenStopped = true;

	public bool removeWhenDisabled = true;

	public void Update()
	{
		if (removeWhenStopped && velocity.sqrMagnitude <= 0.01f)
		{
			Object.Destroy(this);
			return;
		}

		base.transform.position += velocity * Time.deltaTime;
		velocity = Delta.Multiply(velocity, frictMult, Time.deltaTime);
	}

	public void OnDisable()
	{
		if (removeWhenDisabled)
		{
			Object.Destroy(this);
		}
	}
}
