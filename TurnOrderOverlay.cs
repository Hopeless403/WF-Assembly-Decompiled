#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnOrderOverlay : MonoBehaviour
{
	[SerializeField]
	public GameObject overlay;

	[SerializeField]
	public GameObject button;

	[SerializeField]
	public TMP_Text numberPrefab;

	public List<TMP_Text> numbers;

	public void OnEnable()
	{
		Events.OnBattlePhaseStart += BattlePhaseStart;
	}

	public void OnDisable()
	{
		Events.OnBattlePhaseStart -= BattlePhaseStart;
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		switch (phase)
		{
			case Battle.Phase.Battle:
			case Battle.Phase.End:
				button.SetActive(value: false);
				break;
			case Battle.Phase.Play:
				button.SetActive(value: true);
				break;
		}
	}

	public void Toggle()
	{
		if (overlay.activeSelf)
		{
			Deactivate();
		}
		else
		{
			Activate();
		}
	}

	public void Activate()
	{
		overlay.SetActive(value: true);
		HashSet<Entity> allUnits = Battle.GetAllUnits();
		int num = 1;
		if (numbers == null)
		{
			numbers = new List<TMP_Text>();
		}

		foreach (Entity item in allUnits)
		{
			TMP_Text tMP_Text = Object.Instantiate(numberPrefab, overlay.transform);
			tMP_Text.transform.localPosition = item.transform.position;
			tMP_Text.text = num.ToString();
			tMP_Text.gameObject.SetActive(value: true);
			numbers.Add(tMP_Text);
			num++;
		}
	}

	public void Deactivate()
	{
		foreach (TMP_Text number in numbers)
		{
			number.gameObject.Destroy();
		}

		numbers = null;
		overlay.SetActive(value: false);
	}
}
