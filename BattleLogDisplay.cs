#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class BattleLogDisplay : MonoBehaviour
{
	[SerializeField]
	public BattleLogDisplayBuilder builder;

	[SerializeField]
	public ScrollRect scroll;

	[SerializeField]
	public GameObject loadingWidget;

	public bool promptScrollToBottom;

	public bool loadingWidgetActive;

	public void OnEnable()
	{
		CheckScrollToBottom();
		loadingWidget.SetActive(builder.running);
		loadingWidgetActive = loadingWidget.activeSelf;
	}

	public void Update()
	{
		CheckScrollToBottom();
		if (loadingWidgetActive && !builder.running)
		{
			loadingWidget.SetActive(value: false);
			loadingWidgetActive = false;
		}
	}

	public void CheckScrollToBottom()
	{
		if (promptScrollToBottom && !builder.running)
		{
			scroll.normalizedPosition = Vector2.zero;
			promptScrollToBottom = false;
		}
	}

	public void PromptScrollToBottom()
	{
		promptScrollToBottom = true;
	}
}
