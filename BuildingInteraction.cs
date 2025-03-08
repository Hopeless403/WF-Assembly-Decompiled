#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Building))]
public class BuildingInteraction : MonoBehaviour
{
	[SerializeField]
	public GameObject[] outlines;

	public Building _building;

	public bool hover;

	public Building building => _building ?? (_building = GetComponent<Building>());

	public void Hover()
	{
		if (!hover)
		{
			Debug.Log("[" + base.name + " Hover]");
			hover = true;
			GameObject[] array = outlines;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: true);
			}

			HoverTween();
			building.SetIcon("Select");
			Events.InvokeButtonHover(ButtonType.Building);
		}
	}

	public void UnHover()
	{
		if (hover)
		{
			hover = false;
			Debug.Log("[" + base.name + " UnHover]");
			GameObject[] array = outlines;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}

			UnHoverTween();
			building.SetSuitableIcon();
		}
	}

	public void Select(BaseEventData eventData)
	{
		if (!(eventData is PointerEventData pointerEventData) || pointerEventData.button == PointerEventData.InputButton.Left)
		{
			Town.SelectBuilding(building);
			Events.InvokeButtonPress(ButtonType.Building);
		}
	}

	public void HoverTween()
	{
		LeanTween.cancel(base.gameObject);
		base.transform.localScale = new Vector3(0.99502486f, 0.99502486f, 1f);
		LeanTween.scale(base.gameObject, new Vector3(1.005f, 1.005f, 1f), 1.2f).setEaseOutElastic();
	}

	public void UnHoverTween()
	{
		LeanTween.cancel(base.gameObject);
		LeanTween.scale(base.gameObject, Vector3.one, 0.1f).setEaseOutQuart();
	}
}
