#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class DemoEndSequence : MonoBehaviour
{
	[SerializeField]
	public GameObject[] pages;

	[SerializeField]
	public Image[] pageBlips;

	[SerializeField]
	public Sprite blipActive;

	[SerializeField]
	public Sprite blipInactive;

	[SerializeField]
	public OpenURL openURL;

	[SerializeField]
	public TweenUI nextPageTween;

	[SerializeField]
	public TweenUI previousPageTween;

	public int currentPage;

	public bool active = true;

	public void Awake()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeSelf)
			{
				currentPage = i;
				break;
			}
		}

		if ((bool)MonoBehaviourSingleton<Deckpack>.instance && Deckpack.IsOpen)
		{
			Deckpack.Close();
		}
	}

	public void NextPage()
	{
		if (active)
		{
			ClosePage(currentPage);
			currentPage++;
			if (currentPage >= pages.Length)
			{
				currentPage = 0;
			}

			OpenPage(currentPage);
			nextPageTween.Fire();
		}
	}

	public void PreviousPage()
	{
		if (active)
		{
			ClosePage(currentPage);
			currentPage--;
			if (currentPage < 0)
			{
				currentPage = pages.Length - 1;
			}

			OpenPage(currentPage);
			previousPageTween.Fire();
		}
	}

	public void Wishlist()
	{
		if (active)
		{
			openURL.Open();
		}
	}

	public void Close()
	{
		if (active)
		{
			active = false;
			new Routine(SceneManager.Unload("DemoEnd"));
		}
	}

	public void OpenPage(int number)
	{
		pages[number].SetActive(value: true);
		pageBlips[number].sprite = blipActive;
	}

	public void ClosePage(int number)
	{
		pages[currentPage].SetActive(value: false);
		pageBlips[currentPage].sprite = blipInactive;
	}
}
