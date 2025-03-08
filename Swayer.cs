#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using NaughtyAttributes;
using UnityEngine;

public class Swayer : MonoBehaviour
{
	[SerializeField]
	public bool randomStart;

	[Header("Position")]
	[OnValueChanged("SwayPositionToggled")]
	public bool swayPosition;

	[ShowIf("swayPosition")]
	public Vector3 positionInfluence = Vector3.one;

	[ShowIf("swayPosition")]
	public Vector3 positionSway = Vector3.one;

	[Header("Rotation")]
	[OnValueChanged("SwayRotationToggled")]
	public bool swayRotation;

	[ShowIf("swayRotation")]
	public Vector3 rotationInfluence = Vector3.one;

	[ShowIf("swayRotation")]
	public Vector3 rotationSway = Vector3.one;

	public Vector3 startPosition;

	public Vector3 startRotation;

	public float t;

	public new Transform transform;

	public void Awake()
	{
		transform = GetComponent<Transform>();
		startPosition = transform.localPosition;
		startRotation = transform.localEulerAngles;
		if (randomStart)
		{
			t = PettyRandom.Range(0f, 10f);
		}
	}

	public void Update()
	{
		float num = Time.timeSinceLevelLoad + t;
		if (swayPosition)
		{
			Vector3 localPosition = startPosition + Vector3.Scale(new Vector3(Mathf.Sin(num * positionSway.x), Mathf.Sin(num * positionSway.y), Mathf.Sin(num * positionSway.z)), positionInfluence);
			transform.localPosition = localPosition;
		}

		if (swayRotation)
		{
			Vector3 localEulerAngles = startRotation + Vector3.Scale(new Vector3(Mathf.Sin(num * rotationSway.x), Mathf.Sin(num * rotationSway.y), Mathf.Sin(num * rotationSway.z)), rotationInfluence);
			transform.localEulerAngles = localEulerAngles;
		}
	}

	public void SwayPositionToggled()
	{
		if (!swayPosition)
		{
			transform.localPosition = startPosition;
		}
	}

	public void SwayRotationToggled()
	{
		if (!swayRotation)
		{
			transform.localEulerAngles = startRotation;
		}
	}
}
