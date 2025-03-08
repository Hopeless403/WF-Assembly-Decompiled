#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsEntry : MonoBehaviour
{
	[SerializeField]
	public Color playerBgColor;

	[SerializeField]
	public Graphic background;

	[SerializeField]
	public TMP_Text nameText;

	[SerializeField]
	public TMP_Text rankText;

	[SerializeField]
	public TMP_Text scoreText;

	[SerializeField]
	public TMP_Text timeText;

	public void Set(bool isPlayer, string name, int rank, object score, string timeString)
	{
		if (isPlayer)
		{
			background.color = playerBgColor;
		}

		nameText.text = name;
		rankText.text = $"#{rank}";
		scoreText.text = score.ToString();
		timeText.text = timeString;
	}
}
