#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class BuildingPlot : MonoBehaviour
{
	[SerializeField]
	public string sortingLayerName;

	[SerializeField]
	public int sortingOrder;

	public void Start()
	{
		Building componentInChildren = GetComponentInChildren<Building>(includeInactive: true);
		if (componentInChildren != null)
		{
			SpriteRenderer[] componentsInChildren = componentInChildren.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
			foreach (SpriteRenderer obj in componentsInChildren)
			{
				obj.sortingLayerName = sortingLayerName;
				obj.sortingOrder += sortingOrder;
			}
		}

		Image component = GetComponent<Image>();
		if ((object)component != null)
		{
			component.enabled = false;
		}
	}
}
