#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Scroller : MonoBehaviourRect
{
	[SerializeField]
	public UINavigationLayer navLayer;

	public bool interactable = true;

	public float scrollSpeed = 0.2f;

	public float scrollAmount = 1f;

	public bool horizontal;

	public RectTransform bounds;

	[Range(0f, 1f)]
	public float boundsHardness = 0.2f;

	public UnityEvent onScroll;

	public UnityEvent<float> afterScroll;

	public Vector2 targetPos;

	public Vector2 preTargetPos;

	[SerializeField]
	public bool hasStartScroll;

	[SerializeField]
	[ShowIf("hasStartScroll")]
	[Range(0f, 1f)]
	public float startScroll;

	[SerializeField]
	public bool ignoreTimeScale;

	[Header("Scroll Knob")]
	[SerializeField]
	public RectTransform scrollKnob;

	[SerializeField]
	public RectTransform scrollKnobBounds;

	public float boundsDelay;

	public static float boundsDelayMax;

	public bool checkBounds;

	public float DeltaTime
	{
		get
		{
			if (!ignoreTimeScale)
			{
				return Time.deltaTime;
			}

			return Time.unscaledDeltaTime;
		}
	}

	public float s => GetRelevant(base.rectTransform.rect.size) - GetRelevant(bounds.rect.size);

	public float TargetPos
	{
		get
		{
			if (!horizontal)
			{
				return targetPos.y;
			}

			return targetPos.x;
		}
		set
		{
			if (horizontal)
			{
				targetPos.x = value;
			}
			else
			{
				targetPos.y = value;
			}
		}
	}

	public float ScrollAmount
	{
		get
		{
			return (s * 0.5f - TargetPos) / s;
		}
		set
		{
			TargetPos = (0f - s) * 0.5f + value * s;
		}
	}

	public void Awake()
	{
		if (!bounds && base.transform.parent.transform is RectTransform rectTransform)
		{
			bounds = rectTransform;
		}

		if ((object)navLayer == null)
		{
			navLayer = FindComponentInParents<UINavigationLayer>(base.transform);
		}

		UpdateScrollKnob();
		boundsDelay = boundsDelayMax;
		InvokeAfterScroll();
		preTargetPos = targetPos;
	}

	public T FindComponentInParents<T>(Transform parent) where T : Component
	{
		while (true)
		{
			T component = parent.GetComponent<T>();
			if ((Object)component != (Object)null)
			{
				return component;
			}

			if (parent.parent == null)
			{
				break;
			}

			parent = parent.parent;
		}

		return null;
	}

	public void OnEnable()
	{
		if (hasStartScroll)
		{
			ScrollTo(startScroll);
			base.rectTransform.anchoredPosition = targetPos;
		}
	}

	public bool CheckNavigationLayer()
	{
		if ((bool)navLayer)
		{
			return UINavigationSystem.ActiveNavigationLayer == navLayer;
		}

		return true;
	}

	public void Update()
	{
		if (interactable && CheckNavigationLayer())
		{
			float num = 0f;
			if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
			{
				num = (horizontal ? (0f - Input.mouseScrollDelta.y) : Input.mouseScrollDelta.y);
			}

			if (num != 0f)
			{
				Scroll(num * scrollAmount);
			}
		}

		if (boundsDelay > 0f)
		{
			boundsDelay -= DeltaTime;
		}
		else if (checkBounds && (bool)bounds)
		{
			CheckBounds();
		}

		Vector2 anchoredPosition = base.rectTransform.anchoredPosition;
		if (anchoredPosition != targetPos)
		{
			anchoredPosition = Delta.Lerp(anchoredPosition, targetPos, scrollSpeed, DeltaTime);
			base.rectTransform.anchoredPosition = anchoredPosition;
		}
	}

	public void LateUpdate()
	{
		if (preTargetPos != targetPos)
		{
			InvokeAfterScroll();
			preTargetPos = targetPos;
		}
	}

	public void InvokeAfterScroll()
	{
		if (afterScroll != null && (bool)bounds)
		{
			float num = s;
			float num2 = num * 0.5f;
			float arg = 1f - (TargetPos + num2 - GetRelevant(bounds.localPosition)) / num;
			afterScroll.Invoke(arg);
		}
	}

	public void Scroll(float amount)
	{
		TargetPos -= amount;
		onScroll?.Invoke();
		UpdateScrollKnob();
		checkBounds = true;
	}

	public void ScrollImmediate(float amount)
	{
		TargetPos -= amount;
		onScroll?.Invoke();
		CheckBounds();
		base.rectTransform.anchoredPosition = targetPos;
		UpdateScrollKnob();
	}

	public void ScrollTo(float position)
	{
		ScrollAmount = position;
		onScroll?.Invoke();
		UpdateScrollKnob();
	}

	public void ScrollTo(Vector2 targetPos)
	{
		this.targetPos = targetPos;
		onScroll?.Invoke();
		UpdateScrollKnob();
		checkBounds = true;
	}

	public void CheckBounds()
	{
		Vector2 vector = bounds.rect.size * 0.5f;
		Vector2 anchoredPosition = bounds.anchoredPosition;
		float num = GetRelevant(anchoredPosition) - GetRelevant(vector);
		float num2 = GetRelevant(anchoredPosition) + GetRelevant(vector);
		Vector2 vector2 = base.rectTransform.rect.size * 0.5f;
		float num3 = TargetPos - GetRelevant(vector2);
		float num4 = TargetPos + GetRelevant(vector2);
		bool flag = num3 >= num;
		bool flag2 = num4 <= num2;
		Vector2 vector3 = targetPos;
		if (flag || flag2)
		{
			vector3 = ((flag && flag2) ? SetRelevant(vector3, 0f) : ((!flag) ? AddRelevant(vector3, num2 - num4) : AddRelevant(vector3, num - num3)));
			targetPos = ((boundsHardness < 1f) ? Delta.Lerp(targetPos, vector3, boundsHardness, DeltaTime) : vector3);
		}
		else
		{
			checkBounds = false;
		}
	}

	public float GetRelevant(Vector2 vector2)
	{
		if (!horizontal)
		{
			return vector2.y;
		}

		return vector2.x;
	}

	public Vector2 SetRelevant(Vector2 vector2, float value)
	{
		if (horizontal)
		{
			vector2.x = value;
		}
		else
		{
			vector2.y = value;
		}

		return vector2;
	}

	public Vector2 AddRelevant(Vector2 vector2, float value)
	{
		if (horizontal)
		{
			vector2.x += value;
		}
		else
		{
			vector2.y += value;
		}

		return vector2;
	}

	public void UpdateScrollKnob()
	{
		if ((bool)scrollKnob && (bool)scrollKnobBounds)
		{
			float relevant = GetRelevant(scrollKnobBounds.rect.size);
			float value = (0f - relevant) * 0.5f + relevant * Mathf.Clamp(ScrollAmount, 0f, 1f);
			LeanTween.cancel(scrollKnob);
			Vector2 vector = (horizontal ? scrollKnob.anchoredPosition.WithX(value) : scrollKnob.anchoredPosition.WithY(value));
			LeanTween.move(scrollKnob, vector, 0.2f).setEase(LeanTweenType.easeOutQuart);
		}
	}

	public void SetInteractable(bool value)
	{
		interactable = value;
	}

	public bool ContentLargerThanBounds()
	{
		if (!horizontal)
		{
			return base.rectTransform.sizeDelta.y > bounds.sizeDelta.y;
		}

		return base.rectTransform.sizeDelta.x > bounds.sizeDelta.x;
	}
}
