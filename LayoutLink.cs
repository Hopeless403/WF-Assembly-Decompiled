#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LayoutLink : MonoBehaviourRect
{
	[FormerlySerializedAs("layoutTransform")]
	public RectTransform link;

	public float lerp = 0.1f;

	public bool setPositionOnEnable = true;

	public Vector3 offset;

	[Header("Link to layout element to update layout element size")]
	public LayoutElement layoutElement;

	[ShowIf("HasLayoutElement")]
	public bool setLayoutWidth;

	[ShowIf("HasLayoutElement")]
	public bool setLayoutHeight;

	public bool HasLayoutElement => layoutElement != null;

	public void OnEnable()
	{
		if (setPositionOnEnable && link != null)
		{
			base.rectTransform.position = link.position + offset;
		}
	}

	public void LateUpdate()
	{
		base.rectTransform.position = Delta.Lerp(base.rectTransform.position, link.position + offset, lerp, Time.deltaTime);
	}

	public void Update()
	{
		if (HasLayoutElement)
		{
			Vector2 sizeDelta = base.rectTransform.sizeDelta;
			if (setLayoutWidth)
			{
				layoutElement.preferredWidth = sizeDelta.x;
			}

			if (setLayoutHeight)
			{
				layoutElement.preferredHeight = sizeDelta.y;
			}
		}
	}
}
