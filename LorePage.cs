#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using UnityEngine;
using UnityEngine.UI;

public class LorePage : MonoBehaviour
{
	public LorePageManager manager;

	public JournalPageData pageData;

	[SerializeField]
	public GameObject lockedDisplay;

	[SerializeField]
	public GameObject unlockedDisplay;

	[SerializeField]
	public GameObject newDisplay;

	public Button button;

	public Canvas canvas;

	public bool isUnlocked;

	public bool isNew;

	[SerializeField]
	public TweenUI denyTween;

	[SerializeField]
	public TweenUI newTween;

	public void Awake()
	{
		button.transform.localEulerAngles = new Vector3(0f, 0f, PettyRandom.Range(-1f, 1f) * 2f);
	}

	public void SetUnlocked(JournalPageData pageData, bool value)
	{
		this.pageData = pageData;
		isUnlocked = value;
		button.interactable = value;
		lockedDisplay.SetActive(!value);
		unlockedDisplay.SetActive(value);
	}

	public void SetNew(bool value)
	{
		isNew = value;
		newDisplay.SetActive(value);
		if (value)
		{
			newTween.Fire();
		}
	}

	public void Select()
	{
		if (isUnlocked)
		{
			manager.Select(this);
			return;
		}

		denyTween.Fire();
		SfxSystem.OneShot("event:/sfx/ui/deny");
	}
}
