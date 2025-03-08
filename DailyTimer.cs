#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DailyTimer : MonoBehaviour
{
	[SerializeField]
	public TMP_Text textElement;

	[SerializeField]
	public UnityEvent action;

	public int secondsRemaining;

	public float secondTimer;

	public void OnEnable()
	{
		DateTime dateTime = DailyFetcher.GetDateTime();
		float num = (float)(DailyFetcher.GetNextDateTime() - dateTime).TotalSeconds;
		secondsRemaining = Mathf.FloorToInt(num);
		secondTimer = num - (float)secondsRemaining;
		UpdateText();
	}

	public void Update()
	{
		secondTimer -= Time.deltaTime;
		if (secondTimer < 0f)
		{
			while (secondTimer < 0f)
			{
				secondsRemaining--;
				secondTimer += 1f;
			}

			if (secondsRemaining <= 0)
			{
				action?.Invoke();
			}

			UpdateText();
		}
	}

	public void UpdateText()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.Max(0, secondsRemaining));
		textElement.text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
	}
}
