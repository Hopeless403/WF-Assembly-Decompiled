#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class EyePupil : MonoBehaviour
{
	[SerializeField]
	public Transform target;

	[SerializeField]
	public AnimationCurve aimAmount;

	[SerializeField]
	public float lerp = 0.2f;

	public Vector3 targetPos;

	public void OnEnable()
	{
		Events.OnEntityHover += EntityHover;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
	}

	public void Update()
	{
		target.localPosition = Delta.Lerp(target.localPosition, targetPos, lerp, Time.deltaTime);
	}

	public void EntityHover(Entity entity)
	{
		LookAt(entity.transform.position);
	}

	public void LookAt(Vector3 worldPosition)
	{
		Vector3 vector = (worldPosition - base.transform.position).WithZ(0f);
		float num = aimAmount.Evaluate(vector.magnitude);
		targetPos = vector.normalized * num;
	}
}
