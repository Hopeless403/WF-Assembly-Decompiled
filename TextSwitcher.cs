#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextSwitcher : MonoBehaviour
{
	public bool useExistingTextAsFirst = true;

	public float switchTime = 1f;

	public List<string> texts;

	public int i;

	public float t;

	public TextMeshProUGUI tmp;

	public void Awake()
	{
		tmp = GetComponent<TextMeshProUGUI>();
		if (useExistingTextAsFirst)
		{
			texts.Insert(0, tmp.text);
		}

		t = switchTime;
	}

	public void Update()
	{
		t -= Time.deltaTime;
		while (t <= 0f)
		{
			i = (i + 1) % texts.Count;
			tmp.text = texts[i];
			t += switchTime;
		}
	}
}
