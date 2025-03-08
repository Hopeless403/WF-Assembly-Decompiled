#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class Spinner : MonoBehaviour
{
	[SerializeField]
	public bool ignoreTimeScale;

	public Vector3 speed = new Vector3(0f, 0f, 1f);

	public bool setTargetSpeed;

	[ShowIf("setTargetSpeed")]
	public Vector3 targetSpeed;

	[ShowIf("setTargetSpeed")]
	public float targetSpeedAcc = 100f;

	public void Update()
	{
		if (setTargetSpeed)
		{
			float maxDistanceDelta = targetSpeedAcc * Time.deltaTime;
			speed = Vector3.MoveTowards(speed, targetSpeed, maxDistanceDelta);
		}

		base.transform.localEulerAngles = base.transform.localEulerAngles + speed * (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
	}
}
