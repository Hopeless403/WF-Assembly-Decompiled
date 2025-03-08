#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ConsoleArgsDisplay : MonoBehaviourRect
{
	[SerializeField]
	public TMP_Text textPrefab;

	[SerializeField]
	public int poolStartSize = 20;

	[SerializeField]
	public int maxItems = 30;

	public Queue<TMP_Text> pool = new Queue<TMP_Text>();

	public List<TMP_Text> outOfPool = new List<TMP_Text>();

	public Console.Command[] commands;

	public string[] current;

	public Vector2 targetPos;

	public int Count => current.Length;

	public string TopArgument => current.Last();

	public string TopCommand => commands.Last().id + ((commands.Last().format != commands.Last().id) ? " " : "");

	public void Awake()
	{
		targetPos = base.rectTransform.anchoredPosition;
		for (int i = 0; i < poolStartSize; i++)
		{
			pool.Enqueue(Object.Instantiate(textPrefab, base.transform));
		}
	}

	public void Update()
	{
		base.rectTransform.anchoredPosition = Delta.Lerp(base.rectTransform.anchoredPosition, targetPos, 0.1f, Time.deltaTime);
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void MoveTo(float x)
	{
		targetPos = targetPos.WithX(x);
	}

	public void DisplayCommands(Console.Command[] commands)
	{
		string[] array = commands.Select((Console.Command a) => a.format).ToArray();
		if (current == null || !array.SequenceEqual(current))
		{
			Create(array);
			current = array;
			this.commands = commands;
		}
	}

	public void DisplayArgs(string[] items)
	{
		if (current == null || !items.SequenceEqual(current))
		{
			Create(items);
			current = items;
		}
	}

	public void Clear()
	{
		foreach (TMP_Text item in outOfPool)
		{
			Pool(item);
		}

		outOfPool.Clear();
	}

	public void Create(string[] items)
	{
		Clear();
		int num = items.Length - 1;
		while (num >= 0 && Create(items[num]))
		{
			num--;
		}
	}

	public bool Create(string item)
	{
		bool result = false;
		if (outOfPool.Count < maxItems + 1)
		{
			TMP_Text tMP_Text = ((pool.Count > 0) ? pool.Dequeue() : Object.Instantiate(textPrefab, base.transform));
			if (outOfPool.Count < maxItems)
			{
				tMP_Text.text = item;
				tMP_Text.transform.SetAsLastSibling();
				result = true;
			}
			else
			{
				tMP_Text.text = "...";
				tMP_Text.transform.SetAsLastSibling();
			}

			tMP_Text.gameObject.SetActive(value: true);
			outOfPool.Add(tMP_Text);
		}

		return result;
	}

	public void Pool(TMP_Text inst)
	{
		pool.Enqueue(inst);
		inst.gameObject.SetActive(value: false);
	}
}
