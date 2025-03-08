#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StatusIcon))]
public class StatusIconHealth : MonoBehaviour
{
	[Serializable]
	public class Type
	{
		public string name;

		public GameObject group;

		public TMP_Text textElement;

		public Image fill;

		public void Assign(StatusIcon icon)
		{
			group.SetActive(value: true);
			icon.textElement = textElement;
			icon.fill = fill;
		}
	}

	[SerializeField]
	public Type[] types;

	[SerializeField]
	public GameObject current;

	public StatusIcon _icon;

	public StatusIcon icon => _icon ?? (_icon = GetComponent<StatusIcon>());

	public void SetType()
	{
		if (icon.target == null)
		{
			return;
		}

		string cardTypeName = icon.target.data.cardType.name;
		if (current == null || current.name != cardTypeName)
		{
			Type type = Array.Find(types, (Type a) => a.name == cardTypeName);
			if (type != null)
			{
				current.SetActive(value: false);
				type.Assign(icon);
				current = type.group;
			}
		}
	}
}
