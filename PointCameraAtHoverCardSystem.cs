#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;

public class PointCameraAtHoverCardSystem : GameSystem
{
	[SerializeField]
	public Transform cameraMover;

	[SerializeField]
	public bool affectInPlay = true;

	[SerializeField]
	public bool affectNotInPlay = true;

	[Header("Rotate To Face Card")]
	[SerializeField]
	[Range(0f, 1f)]
	public float aimLerp = 0.05f;

	[SerializeField]
	[Range(0f, 1f)]
	public float aimResetLerp = 0.0125f;

	[SerializeField]
	[Range(0f, 1f)]
	public float aimAmount = 0.05f;

	[Header("Pull Position Towards Card")]
	[SerializeField]
	[Range(0f, 1f)]
	public float pullLerp = 0.05f;

	[SerializeField]
	[Range(0f, 1f)]
	public float pullResetLerp = 0.0125f;

	[SerializeField]
	[Range(0f, 1f)]
	public float pullAmount = 0.05f;

	[SerializeField]
	public float pullClamp = 10f;

	public Entity current;

	public float amount;

	public readonly Quaternion originalRotation = Quaternion.identity;

	public Quaternion toRotation;

	public readonly Vector3 originalPosition = Vector3.zero;

	public Vector3 toPosition;

	public void OnEnable()
	{
		amount = Settings.Load("CameraSway", 1f);
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
		Events.OnEntityDestroyed += EntityUnHover;
		Events.OnSceneChanged += SceneChanged;
		Events.OnSettingChanged += SettingChanged;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
		Events.OnEntityDestroyed -= EntityUnHover;
		Events.OnSceneChanged -= SceneChanged;
		Events.OnSettingChanged -= SettingChanged;
	}

	public void Update()
	{
		bool flag = current;
		if (aimAmount > 0f)
		{
			if (flag)
			{
				Vector3 toDirection = current.transform.position - cameraMover.position;
				toRotation = Quaternion.Lerp(originalRotation, Quaternion.FromToRotation(base.transform.forward, toDirection), aimAmount * amount);
			}

			float num = ((!flag) ? aimResetLerp : aimLerp);
			cameraMover.rotation = Delta.Lerp(cameraMover.rotation, toRotation, num, Time.deltaTime);
		}

		if (pullAmount > 0f)
		{
			if (flag)
			{
				Vector3 vector = Vector3.ClampMagnitude(current.transform.position, pullClamp);
				toPosition = Vector3.Lerp(originalPosition, originalPosition + vector, pullAmount * amount);
			}

			float num2 = ((!flag) ? pullResetLerp : pullLerp);
			cameraMover.localPosition = Delta.Lerp(cameraMover.localPosition, toPosition, num2, Time.deltaTime);
		}
	}

	public void EntityHover(Entity entity)
	{
		if (Check(entity))
		{
			current = entity;
		}
	}

	public void EntityUnHover(Entity entity)
	{
		if (current == entity)
		{
			toRotation = originalRotation;
			toPosition = originalPosition;
			current = null;
		}
	}

	public void SceneChanged(Scene scene)
	{
		cameraMover.SetLocalPositionAndRotation(originalPosition, originalRotation);
	}

	public bool Check(Entity entity)
	{
		if (!entity.inPlay || !affectInPlay)
		{
			if (!entity.inPlay)
			{
				return affectNotInPlay;
			}

			return false;
		}

		return true;
	}

	public void SettingChanged(string key, object value)
	{
		if (key == "CameraSway" && value is float)
		{
			float num = (float)value;
			amount = num;
		}
	}
}
