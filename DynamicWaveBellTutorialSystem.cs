#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization;

public class DynamicWaveBellTutorialSystem : GameSystem
{
	public UnityEngine.Localization.LocalizedString helpKey;

	public Prompt.Emote.Type emote = Prompt.Emote.Type.Explain;

	public UnityEngine.Localization.LocalizedString buttonKey;

	public Sprite helpSprite;

	public void OnEnable()
	{
		Events.OnWaveDeployerPostCountDown += WaveDeployerCountDown;
		if (SaveSystem.LoadProgressData("dynamicWaveBellTutorial", defaultValue: false))
		{
			base.enabled = false;
		}
	}

	public void OnDisable()
	{
		Events.OnWaveDeployerPostCountDown -= WaveDeployerCountDown;
	}

	public void WaveDeployerCountDown(int counter)
	{
		if (counter > 0 && Battle.GetCardsOnBoard(References.Battle.enemy).Count == 0)
		{
			ShowHelp();
		}
	}

	public void ShowHelp()
	{
		HelpPanelSystem.Show(helpKey);
		HelpPanelSystem.SetEmote(emote);
		HelpPanelSystem.SetImage(2f, 2f, helpSprite);
		HelpPanelSystem.SetBackButtonActive(active: false);
		HelpPanelSystem.AddButton(HelpPanelSystem.ButtonType.Positive, buttonKey, "Select", End);
	}

	public void End()
	{
		SaveSystem.SaveProgressData("dynamicWaveBellTutorial", value: true);
		base.enabled = false;
	}
}
