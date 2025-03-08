#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeProgressDisplay : MonoBehaviour
{
	public TMP_Text text;

	public TMP_Text progressText;

	public ImageSprite icon;

	public ImageSprite back;

	public Image fill;

	public UnityEngine.Animator animator;

	public ChallengeData challengeData;

	public void Assign(ChallengeData challengeData)
	{
		this.challengeData = challengeData;
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		text.text = string.Format(challengeData.text, challengeData.goal);
		icon.SetSprite(challengeData.icon);
	}

	public void SetRewardText(ChallengeData challengeData)
	{
		text.text = challengeData.rewardText;
	}

	public void SetFill(float current, int target)
	{
		fill.fillAmount = current / (float)target;
		int num = Mathf.RoundToInt(current);
		progressText.text = $"{num}/{target}";
	}

	public void SetRemainingText(ChallengeData challengeData, float current)
	{
		int num = Mathf.RoundToInt(current);
		text.text = string.Format(challengeData.text, challengeData.goal - num);
	}

	public void SetColor(Color color)
	{
		text.color = color;
		progressText.color = color;
		fill.color = color;
	}
}
