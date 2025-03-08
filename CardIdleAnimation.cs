#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using UnityEngine;

public class CardIdleAnimation : MonoBehaviourCacheTransform, IPoolable
{
	public enum Action
	{
		None,
		FadeIn,
		FadeOut
	}

	public CardAnimationProfile profile;

	public Entity _entity;

	public bool alwaysOn;

	public float strength = 1f;

	public float speedFactor = 1f;

	public float fadeInTime = 0.4f;

	public float fadeInStrength = 1f;

	public float speed;

	public float offset;

	public Vector3 basePosition;

	public Vector3 baseRotation;

	public Vector3 baseScale;

	[SerializeField]
	public CardAnimationProfile setProfile;

	[SerializeField]
	public Action currentAction;

	public Entity entity
	{
		get
		{
			return _entity;
		}
		set
		{
			_entity = value;
			SetProfile(_entity.data.idleAnimationProfile);
		}
	}

	public void Awake()
	{
		basePosition = base.transform.localPosition;
		baseRotation = base.transform.localEulerAngles;
		baseScale = base.transform.localScale;
		speed = 1f / PettyRandom.Range(2f, 3f);
	}

	public void OnEnable()
	{
		if (alwaysOn)
		{
			StartAction(Action.FadeIn);
		}
	}

	public void OnDisable()
	{
		if (!alwaysOn)
		{
			fadeInStrength = 0f;
			base.enabled = false;
			currentAction = Action.None;
		}
	}

	public void Update()
	{
		if ((bool)profile && strength != 0f)
		{
			float time = (Time.timeSinceLevelLoad * speed * speedFactor + offset) % 1f;
			if (profile.doMoveX || profile.doMoveY || profile.doMoveZ)
			{
				float num = (profile.doMoveX ? (profile.moveX.Evaluate(time) * profile.moveAmount.x * strength * fadeInStrength) : 0f);
				float num2 = (profile.doMoveY ? (profile.moveY.Evaluate(time) * profile.moveAmount.y * strength * fadeInStrength) : 0f);
				float num3 = (profile.doMoveZ ? (profile.moveZ.Evaluate(time) * profile.moveAmount.z * strength * fadeInStrength) : 0f);
				base.transform.localPosition = new Vector3(basePosition.x + num, basePosition.y + num2, basePosition.z + num3);
			}

			if (profile.doRotateX || profile.doRotateY || profile.doRotateZ)
			{
				float num4 = (profile.doRotateX ? (profile.rotateX.Evaluate(time) * profile.rotateAmount.x * strength * fadeInStrength) : 0f);
				float num5 = (profile.doRotateY ? (profile.rotateY.Evaluate(time) * profile.rotateAmount.y * strength * fadeInStrength) : 0f);
				float num6 = (profile.doRotateZ ? (profile.rotateZ.Evaluate(time) * profile.rotateAmount.z * strength * fadeInStrength) : 0f);
				base.transform.localEulerAngles = new Vector3(baseRotation.x + num4, baseRotation.y + num5, baseRotation.z + num6);
			}

			if (profile.doScaleX || profile.doScaleY || profile.doScaleZ)
			{
				float num7 = (profile.doScaleX ? (profile.ScaleX.Evaluate(time) * profile.scaleAmount.x * strength * fadeInStrength) : 0f);
				float num8 = (profile.doScaleY ? (profile.ScaleY.Evaluate(time) * profile.scaleAmount.y * strength * fadeInStrength) : 0f);
				float num9 = (profile.doScaleZ ? (profile.ScaleZ.Evaluate(time) * profile.scaleAmount.z * strength * fadeInStrength) : 0f);
				base.transform.localScale = new Vector3(baseScale.x + num7, baseScale.y + num8, baseScale.z + num9);
			}
		}

		if ((bool)setProfile)
		{
			SetProfile(setProfile);
			setProfile = null;
		}

		if (currentAction != 0)
		{
			RunAction();
		}
	}

	public void SetSpeed(float speed, float speedFactor, float offset)
	{
		this.speed = speed;
		this.speedFactor = speedFactor;
		this.offset = offset;
	}

	public void SetProfile(CardAnimationProfile profile)
	{
		this.profile = profile;
		float num = (_entity ? (1f / (2f + Mathf.Abs(_entity.random3.x))) : 0.5f);
		speed = num * profile.speedFactor;
		offset = (_entity ? Mathf.Abs(_entity.random3.y) : 0f);
	}

	public void StartAction(Action action)
	{
		currentAction = action;
		switch (action)
		{
			case Action.FadeIn:
				base.enabled = true;
				fadeInStrength = 0f;
				break;
			case Action.FadeOut:
			if (!base.gameObject.activeInHierarchy)
			{
				base.enabled = false;
				fadeInStrength = 0f;
				StartAction(Action.None);
				}
	
				break;
		}
	}

	public void RunAction()
	{
		switch (currentAction)
		{
			case Action.FadeIn:
			if (fadeInStrength < 1f)
			{
				fadeInStrength += 1f / fadeInTime * Time.deltaTime;
				break;
				}
	
				fadeInStrength = 1f;
				StartAction(Action.None);
				break;
			case Action.FadeOut:
			if (fadeInStrength > 0f)
			{
				fadeInStrength -= 1f / fadeInTime * Time.deltaTime;
				break;
				}
	
				base.enabled = false;
				fadeInStrength = 0f;
				StartAction(Action.None);
				break;
		}
	}

	public void FadeIn()
	{
		StartAction(Action.FadeIn);
	}

	public void FadeOut()
	{
		StartAction(Action.FadeOut);
	}

	public void Clear()
	{
		fadeInStrength = 0f;
		currentAction = Action.None;
	}

	public void OnGetFromPool()
	{
	}

	public void OnReturnToPool()
	{
		Clear();
	}
}
