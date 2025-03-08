#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class UpgradeDisplay : MonoBehaviour
{
	[SerializeField]
	public Image image;

	public UINavigationItem navigationItem;

	public Image _raycast;

	public CardUpgradeData data { get; set; }

	public Image raycast => _raycast ?? (_raycast = GetComponent<Image>());

	public bool CanRaycast
	{
		set
		{
			raycast.raycastTarget = value;
		}
	}

	public virtual void SetData(CardUpgradeData data)
	{
		this.data = data;
		image.sprite = data.image;
	}
}
