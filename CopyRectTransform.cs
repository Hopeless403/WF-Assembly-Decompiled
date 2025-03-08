#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class CopyRectTransform : MonoBehaviourRect
{
	[SerializeField]
	public RectTransform target;

	[SerializeField]
	[ShowIf("hasTarget")]
	public bool onEnable;

	[SerializeField]
	[ShowIf("hasTarget")]
	public bool onUpdate = true;

	[SerializeField]
	[ShowIf("hasTarget")]
	public bool onValidate;

	[SerializeField]
	[ShowIf("hasTarget")]
	public bool copyPosition = true;

	[SerializeField]
	[ShowIf("hasTarget")]
	public bool copyRotation = true;

	[SerializeField]
	[ShowIf("hasTarget")]
	public bool copySize;

	[SerializeField]
	[ShowIf("hasTarget")]
	public bool copyPivot;

	[SerializeField]
	[ShowIf("hasTarget")]
	public bool copyScale = true;

	[SerializeField]
	[ShowIf("hasTargetAndCopyScale")]
	public bool invertScale;

	public bool hasTarget => target != null;

	public bool hasTargetAndCopyScale
	{
		get
		{
			if (hasTarget)
			{
				return copyScale;
			}

			return false;
		}
	}

	public void OnEnable()
	{
		if (onEnable)
		{
			Copy();
		}
	}

	public void LateUpdate()
	{
		if (onUpdate)
		{
			Copy();
		}
	}

	[Button("Update", EButtonEnableMode.Always)]
	public void Copy()
	{
		if (hasTarget)
		{
			if (copyPosition)
			{
				base.rectTransform.position = target.position;
			}

			if (copyRotation)
			{
				base.rectTransform.rotation = target.rotation;
			}

			if (copySize)
			{
				base.rectTransform.sizeDelta = target.sizeDelta;
			}

			if (copyPivot)
			{
				base.rectTransform.pivot = target.pivot;
			}

			if (copyScale)
			{
				base.rectTransform.localScale = (invertScale ? target.localScale.Invert() : target.localScale);
			}
		}
	}
}
