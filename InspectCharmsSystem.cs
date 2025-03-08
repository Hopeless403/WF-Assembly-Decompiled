#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InspectCharmsSystem : GameSystem
{
	[SerializeField]
	public InspectSystem inspectSystem;

	[SerializeField]
	public CardCharm charmPrefab;

	[SerializeField]
	public GridLayoutGroup grid;

	[SerializeField]
	public string[] closeInputs = new string[3] { "Select", "Back", "Inspect" };

	public readonly List<CardCharm> charms = new List<CardCharm>();

	public const int maxCharms = 30;

	public const int maxColumns = 6;

	public float wait;

	public void Update()
	{
		if (wait <= 0f && !MonoBehaviourSingleton<Cursor3d>.instance.usingMouse && closeInputs.Any((string i) => InputSystem.IsButtonPressed(i)))
		{
			Hide();
		}

		if (wait > 0f)
		{
			wait -= Time.deltaTime;
		}
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		Create(inspectSystem.inspect.data.upgrades.Where((CardUpgradeData a) => a.type == CardUpgradeData.Type.Charm).ToArray());
		wait = 0.15f;
	}

	public void Create(CardUpgradeData[] cardUpgrades)
	{
		int num = Mathf.Min(30, cardUpgrades.Length);
		SetGridSize(num);
		for (int i = 0; i < num; i++)
		{
			CardCharm cardCharm = Object.Instantiate(charmPrefab, grid.transform);
			cardCharm.SetData(cardUpgrades[i]);
			cardCharm.holder = cardCharm.transform;
			charms.Add(cardCharm);
		}
	}

	public void SetGridSize(int count)
	{
		int num = Mathf.Min(6, count);
		int num2 = Mathf.CeilToInt((float)count / 6f);
		if (grid.transform is RectTransform rectTransform)
		{
			rectTransform.sizeDelta = new Vector2((float)num * grid.cellSize.x, (float)num2 * grid.cellSize.y);
		}
	}

	public void TryHide()
	{
		if (wait <= 0f)
		{
			Hide();
		}
	}

	public void Hide()
	{
		foreach (CardCharm charm in charms)
		{
			charm.gameObject.Destroy();
		}

		charms.Clear();
		base.gameObject.SetActive(value: false);
	}
}
