#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ChallengeEntry : MonoBehaviour
{
	[SerializeField]
	public LocalizeStringEvent text;

	[SerializeField]
	public TMP_Text progressText;

	[SerializeField]
	public LocalizeStringEvent rewardText;

	[SerializeField]
	public Image background;

	[SerializeField]
	public Image rewardIcon;

	public ChallengeData challenge;

	public void Assign(ChallengeData challenge, bool completed)
	{
		this.challenge = challenge;
		text.StringReference = challenge.textKey;
		if (completed && (bool)rewardText)
		{
			rewardText.StringReference = challenge.rewardKey;
		}

		if ((bool)rewardIcon)
		{
			rewardIcon.sprite = challenge.icon;
		}
	}

	public void SetProgress(int progress)
	{
		progressText.text = $"{progress}/{challenge.goal}";
	}

	public void SetText(string str)
	{
		TMP_Text component = text.GetComponent<TMP_Text>();
		if ((object)component != null)
		{
			component.text = str.Format(challenge.listener.target);
		}
	}
}
