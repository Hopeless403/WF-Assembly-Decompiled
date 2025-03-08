#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class HandleSpinner : MonoBehaviour
{
	[SerializeField]
	public Transform handle;

	[SerializeField]
	public float startSpeed = 100f;

	[SerializeField]
	public float targetSpeed = 100f;

	[SerializeField]
	public float acceleration = 10f;

	[SerializeField]
	public float deceleration = 10f;

	public bool spinning;

	public float speed;

	[Button(null, EButtonEnableMode.Always)]
	public void Spin()
	{
		base.enabled = true;
		spinning = true;
		speed = startSpeed;
	}

	[Button(null, EButtonEnableMode.Always)]
	public void Stop()
	{
		spinning = false;
	}

	public void Update()
	{
		float num = (0f - speed) * Time.deltaTime;
		base.transform.Rotate(num, 0f, 0f);
		handle.Rotate(0f - num, 0f, 0f);
		if (spinning)
		{
			speed = Mathf.Min(speed + acceleration * Time.deltaTime, targetSpeed);
		}
		else
		{
			speed = Mathf.Max(0f, speed - deceleration * Time.deltaTime);
		}
	}
}
