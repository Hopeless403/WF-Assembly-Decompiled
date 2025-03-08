#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using Rewired.Data.Mapping;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Joystick Button Style", fileName = "Joystick Button Style")]
public class JoystickButtonStyle : ScriptableObject
{
	[Serializable]
	public class ElementButton
	{
		public string elementName;

		public Sprite buttonSprite;

		public UnityEngine.Localization.LocalizedString textKey;

		public bool hasSprite => buttonSprite;

		public string text => textKey.GetLocalizedString();
	}

	public HardwareJoystickMap[] hardwareMaps;

	public string hardwareIdentifier;

	public int templateId;

	public string tag;

	[SerializeField]
	public ElementButton[] elements;

	[SerializeField]
	public ControllerType type = ControllerType.Joystick;

	public IEnumerable<Guid> guids => hardwareMaps.Select((HardwareJoystickMap a) => a.Guid);

	public ElementButton GetElement(Player player, string actionName)
	{
		ActionElementMap firstElementMapWithAction = player.controllers.maps.GetFirstElementMapWithAction(type, actionName, skipDisabledMaps: true);
		if (firstElementMapWithAction != null)
		{
			return elements[firstElementMapWithAction.elementIndex];
		}

		return null;
	}
}
