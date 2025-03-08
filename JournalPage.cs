#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[ExecuteInEditMode]
public class JournalPage : MonoBehaviour
{
	[SerializeField]
	public Journal journal;

	[SerializeField]
	public Sprite pageSprite;

	[SerializeField]
	public JournalPage alsoOpen;

	public void Open()
	{
		if (base.gameObject.activeSelf)
		{
			return;
		}

		foreach (Transform item in base.transform.parent)
		{
			item.gameObject.SetActive(value: false);
		}

		base.gameObject.SetActive(value: true);
		journal.PagedOpened(this);
		if ((bool)pageSprite)
		{
			journal.SetPageImage(pageSprite);
		}

		if (alsoOpen != null)
		{
			alsoOpen.Open();
		}
	}

	public void Close()
	{
		base.gameObject.SetActive(value: false);
	}
}
