#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

public class CinemaBarShower : MonoBehaviour
{
	[SerializeField]
	public bool showOnEnable;

	[Header("Sorting Order")]
	[SerializeField]
	public bool setSortingOrder;

	[SerializeField]
	[ShowIf("setSortingOrder")]
	public string sortingLayer = "CinemaBars";

	[SerializeField]
	[ShowIf("setSortingOrder")]
	public int orderInLayer;

	[Header("Top Bar")]
	[SerializeField]
	public bool topText;

	[SerializeField]
	[ShowIf("topText")]
	public UnityEngine.Localization.LocalizedString topScript;

	[SerializeField]
	[ShowIf("topText")]
	public UnityEngine.Localization.LocalizedString topPrompt;

	[SerializeField]
	[ShowIf("topText")]
	public string topAction;

	[Header("Bottom Bar")]
	[SerializeField]
	public bool bottomText;

	[SerializeField]
	[ShowIf("bottomText")]
	public UnityEngine.Localization.LocalizedString bottomScript;

	[SerializeField]
	[ShowIf("bottomText")]
	public UnityEngine.Localization.LocalizedString bottomPrompt;

	[SerializeField]
	[ShowIf("bottomText")]
	public string bottomAction;

	public void OnEnable()
	{
		if (showOnEnable)
		{
			Show();
		}
	}

	public void Show()
	{
		CinemaBarSystem.In();
		CinemaBarSystem.SetSortingLayer(sortingLayer, orderInLayer);
		if (topText)
		{
			if (!topScript.IsEmpty)
			{
				CinemaBarSystem.Top.SetScript(topScript.GetLocalizedString());
			}
			else if (!topPrompt.IsEmpty)
			{
				CinemaBarSystem.Top.SetPrompt(topPrompt.GetLocalizedString(), topAction);
			}
		}

		if (bottomText)
		{
			if (!bottomScript.IsEmpty)
			{
				CinemaBarSystem.Bottom.SetScript(bottomScript.GetLocalizedString());
			}
			else if (!bottomPrompt.IsEmpty)
			{
				CinemaBarSystem.Bottom.SetPrompt(bottomPrompt.GetLocalizedString(), bottomAction);
			}
		}
	}

	public void Hide()
	{
		CinemaBarSystem.Clear();
		CinemaBarSystem.Out();
	}
}
