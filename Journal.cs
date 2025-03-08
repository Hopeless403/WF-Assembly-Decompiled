#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Journal : MonoBehaviour
{
	[SerializeField]
	public JournalTab openOnEnable;

	[SerializeField]
	public Transform leftPageGroup;

	[SerializeField]
	public Transform rightPageGroup;

	[SerializeField]
	public Image page;

	[SerializeField]
	public EventReference closeSfxRef;

	public void OnEnable()
	{
		if (Application.isPlaying)
		{
			if ((bool)openOnEnable)
			{
				openOnEnable.Select();
			}

			SfxSystem.OneShot("event:/sfx/ui/journal_open");
		}
	}

	public void OnDisable()
	{
		if ((bool)openOnEnable)
		{
			openOnEnable.Select();
		}
	}

	public void PagedOpened(JournalPage page)
	{
		if (!(page.transform.parent == leftPageGroup))
		{
			return;
		}

		foreach (Transform item in rightPageGroup)
		{
			JournalPage component = item.GetComponent<JournalPage>();
			if ((object)component != null && component.gameObject.activeSelf)
			{
				component.Close();
			}
		}
	}

	public void SetPageImage(Sprite sprite)
	{
		page.sprite = sprite;
	}

	public void Close()
	{
		SfxSystem.OneShot(closeSfxRef);
	}
}
