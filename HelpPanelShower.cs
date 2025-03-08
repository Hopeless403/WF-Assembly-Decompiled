#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class HelpPanelShower : MonoBehaviour
{
	[SerializeField]
	public UnityEngine.Localization.LocalizedString key;

	[SerializeField]
	public Prompt.Emote.Type emote = Prompt.Emote.Type.Explain;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString[] buttonTextKeys;

	[SerializeField]
	public string[] buttonHotKeys;

	[SerializeField]
	public bool canGoBack = true;

	public void SetKey(UnityEngine.Localization.LocalizedString key, Prompt.Emote.Type? emoteType = null)
	{
		this.key = key;
		if (emoteType.HasValue)
		{
			emote = emoteType.Value;
		}
	}

	public void Show()
	{
		HelpPanelSystem.Show(key);
		HelpPanelSystem.SetEmote(emote);
		HelpPanelSystem.SetBackButtonActive(canGoBack);
	}

	public void AddButton(int index, HelpPanelSystem.ButtonType type, UnityAction action)
	{
		HelpPanelSystem.AddButton(type, buttonTextKeys[index], buttonHotKeys[index], action);
	}
}
