#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

public class PromptShower : MonoBehaviour
{
	[SerializeField]
	public bool showOnEnable = true;

	[SerializeField]
	public bool showOnce = true;

	[SerializeField]
	[ShowIf("showOnce")]
	public string saveDataString;

	[SerializeField]
	public Prompt.Anchor anchor;

	[SerializeField]
	public Vector2 position;

	[SerializeField]
	public float width;

	[SerializeField]
	public Prompt.Emote.Type emote;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString localizedString;

	public void OnEnable()
	{
		if (showOnEnable)
		{
			Show(null);
		}
	}

	public void Show(object insert)
	{
		if (showOnce)
		{
			if (SaveSystem.LoadProgressData(saveDataString, defaultValue: false))
			{
				return;
			}

			SaveSystem.SaveProgressData(saveDataString, value: true);
		}

		PromptSystem.Hide();
		PromptSystem.Create(anchor, position.x, position.y, width, emote);
		if (insert != null)
		{
			PromptSystem.SetTextAction(() => string.Format(localizedString.GetLocalizedString(), insert));
		}
		else
		{
			PromptSystem.SetTextAction(() => localizedString.GetLocalizedString());
		}
	}

	public void Hide()
	{
		PromptSystem.Hide();
	}
}
