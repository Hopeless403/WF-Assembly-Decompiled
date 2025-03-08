#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ChallengeListDisplay : MonoBehaviour
{
	[SerializeField]
	public ChallengeListDisplayBuilder builder;

	[SerializeField]
	public GameObject loadingWidget;

	public bool loadingWidgetActive;

	public void OnEnable()
	{
		loadingWidget.SetActive(builder.running);
		loadingWidgetActive = loadingWidget.activeSelf;
	}

	public void Update()
	{
		if (loadingWidgetActive && !builder.running)
		{
			loadingWidget.SetActive(value: false);
			loadingWidgetActive = false;
		}
	}
}
