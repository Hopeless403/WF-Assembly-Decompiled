#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class VersionNumber : MonoBehaviour
{
	[SerializeField]
	public bool showProfile;

	[SerializeField]
	public bool showVersion = true;

	public TMP_Text _t;

	public TMP_Text textElement => _t ?? (_t = GetComponent<TMP_Text>());

	public void OnEnable()
	{
		Events.OnSaveSystemProfileChanged += UpdateText;
		Events.OnModLoaded += ModStateChanged;
		Events.OnModUnloaded += ModStateChanged;
	}

	public void OnDisable()
	{
		Events.OnSaveSystemProfileChanged -= UpdateText;
		Events.OnModLoaded -= ModStateChanged;
		Events.OnModUnloaded -= ModStateChanged;
	}

	public void Start()
	{
		UpdateText();
	}

	public void ModStateChanged(WildfrostMod mod)
	{
		UpdateText();
	}

	public void UpdateText()
	{
		string text = "";
		if (SaveSystem.Enabled && showProfile)
		{
			if (!text.IsNullOrWhitespace())
			{
				text += "\n";
			}

			text = text + "Profile: " + SaveSystem.Profile;
		}

		if (showVersion)
		{
			if (!text.IsNullOrWhitespace())
			{
				text += "\n";
			}

			text += string.Format(Config.data.versionFormat, Config.data.versionNotation);
		}

		WildfrostMod[] array = Bootstrap.Mods.Where((WildfrostMod a) => a.HasLoaded).ToArray();
		if (array.Length != 0)
		{
			text += "<#ff3><size=0.3>";
			WildfrostMod[] array2 = array;
			foreach (WildfrostMod wildfrostMod in array2)
			{
				text = text + "\n" + wildfrostMod.Title;
			}
		}

		textElement.text = text;
	}
}
