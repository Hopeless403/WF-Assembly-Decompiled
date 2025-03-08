#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class TurnOrderDisplay : MonoBehaviour
{
	[SerializeField]
	public Transform group;

	[SerializeField]
	public TurnOrderNumber prefab;

	[SerializeField]
	public CanvasGroup vignette;

	[SerializeField]
	public float musicPitch = 0.9f;

	public bool active;

	public readonly List<TurnOrderNumber> numbers = new List<TurnOrderNumber>();

	public void Toggle()
	{
		if (active)
		{
			Clear();
		}
		else
		{
			Display();
		}
	}

	public void Display()
	{
		if (active)
		{
			return;
		}

		int num = 1;
		foreach (Entity item in Battle.GetCardsOnBoard(References.Battle.enemy))
		{
			if (item.data.counter > 0)
			{
				CreateNumber(item, num++);
			}
		}

		foreach (Entity item2 in Battle.GetCardsOnBoard(References.Battle.player))
		{
			if (item2.data.counter > 0)
			{
				CreateNumber(item2, num++);
			}
		}

		active = true;
		Events.OnEntitySelect += EntitySelect;
		Events.OnRedrawBellHit += RedrawBellHit;
		Events.OnDeckpackOpen += DeckpackOpen;
		Events.OnInspect += Inspect;
		OpenEye();
		vignette.gameObject.SetActive(value: true);
		vignette.alpha = 0f;
		LeanTween.cancel(vignette.gameObject);
		LeanTween.alphaCanvas(vignette, 1f, 1f).setEaseOutQuart();
		Object.FindObjectOfType<BattleMusicSystem>()?.FadePitchTo(musicPitch);
	}

	public void Clear()
	{
		if (!active)
		{
			return;
		}

		foreach (TurnOrderNumber number in numbers)
		{
			if ((bool)number)
			{
				number.gameObject.Destroy();
			}
		}

		numbers.Clear();
		active = false;
		Events.OnEntitySelect -= EntitySelect;
		Events.OnRedrawBellHit -= RedrawBellHit;
		Events.OnDeckpackOpen -= DeckpackOpen;
		Events.OnInspect -= Inspect;
		CloseEye();
		if ((bool)vignette)
		{
			vignette.gameObject.SetActive(value: false);
		}

		Object.FindObjectOfType<BattleMusicSystem>()?.FadePitchTo(1f);
	}

	public static void OpenEye()
	{
		Object.FindObjectOfType<TurnOrderButton>()?.OpenEye();
	}

	public static void CloseEye()
	{
		Object.FindObjectOfType<TurnOrderButton>()?.CloseEye();
	}

	public void CreateNumber(Entity entity, int number)
	{
		Vector3 containerWorldPosition = entity.GetContainerWorldPosition();
		TurnOrderNumber turnOrderNumber = Object.Instantiate(prefab, containerWorldPosition, Quaternion.identity, group);
		turnOrderNumber.Set(entity, number);
		numbers.Add(turnOrderNumber);
	}

	public void EntitySelect(Entity entity)
	{
		Clear();
	}

	public void RedrawBellHit(RedrawBellSystem redrawBell)
	{
		Clear();
	}

	public void DeckpackOpen()
	{
		Clear();
	}

	public void Inspect(Entity entity)
	{
		Clear();
	}
}
