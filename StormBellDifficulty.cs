#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class StormBellDifficulty : MonoBehaviour
{
	[Serializable]
	public struct Level
	{
		public int threshold;

		public UnityEngine.Localization.LocalizedString name;

		public Color colour;
	}

	[SerializeField]
	public bool setOnAwake = true;

	[SerializeField]
	public LocalizeStringEvent locEvent;

	[SerializeField]
	public TMP_Text text;

	[SerializeField]
	public Level[] levels;

	public int _points;

	public int points
	{
		get
		{
			return _points;
		}
		set
		{
			_points = value;
			locEvent.RefreshString();
		}
	}

	public void UpdateText(string text)
	{
		Level[] array = levels;
		for (int i = 0; i < array.Length; i++)
		{
			Level level = array[i];
			if (points >= level.threshold)
			{
				this.text.text = StringExt.Format(text, level.name.GetLocalizedString());
				this.text.color = level.colour;
				break;
			}
		}
	}

	public void Awake()
	{
		if (!setOnAwake)
		{
			return;
		}

		int num = 0;
		List<string> list = SaveSystem.LoadProgressData<List<string>>("activeHardModeModifiers");
		HardModeModifierData[] hardModeModifiers = MonoBehaviourSingleton<References>.instance.hardModeModifiers;
		foreach (HardModeModifierData hardModeModifierData in hardModeModifiers)
		{
			if (list.Contains(hardModeModifierData.name))
			{
				num += hardModeModifierData.stormPoints;
			}
		}

		points = num;
	}
}
