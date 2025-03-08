#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Dead;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HackedTextEffect : MonoBehaviour
{
	public TMP_Text _tmp;

	public char[] extraChars;

	public string[] extraCharColourHexes;

	public Vector2 delayBetweenRoutines = new Vector2(1f, 2f);

	public TMP_Text tmp => _tmp ?? (_tmp = GetComponent<TMP_Text>());

	public void SetText(string text)
	{
		StopAllCoroutines();
		StartRoutine(text);
	}

	public void StartRoutine(string text)
	{
		StartCoroutine(Routine(text));
	}

	public void OnDisable()
	{
		StopAllCoroutines();
	}

	public IEnumerator Routine(string text)
	{
		yield return Appear(text);
		while (true)
		{
			yield return new WaitForSeconds(delayBetweenRoutines.Random());
			yield return Dead.Random.Choose<IEnumerator>(LoopChars(text), AddCharsRandom(text), AppendChars(text), AddChar(text), ReplaceChar(text), ColourChar(text), Appear(text));
		}
	}

	public IEnumerator Appear(string text)
	{
		for (int i = 1; i < text.Length; i++)
		{
			char c = Dead.Random.Choose<char>(extraChars.RandomItem(), text[Mathf.Max(0, i - 1)]);
			tmp.text = text.Remove(i, 1).Insert(i, $"<#{extraCharColourHexes.RandomItem()}>{c}</color>");
			tmp.maxVisibleCharacters = i + 1;
			yield return new WaitForSeconds(0.05f);
		}

		tmp.text = text;
		tmp.maxVisibleCharacters = 1000;
		yield return new WaitForSeconds(1f);
	}

	public IEnumerator LoopChars(string text)
	{
		int count = Dead.Random.Choose<int>(text.Length, Dead.Random.Range(0, text.Length));
		for (int i = 0; i < count; i++)
		{
			char c = Dead.Random.Choose<char>(extraChars.RandomItem(), text[i]);
			tmp.text = text.Remove(i, 1).Insert(i, $"<#{extraCharColourHexes.RandomItem()}>{c}</color>");
			yield return new WaitForSeconds(PettyRandom.Range(0.025f, 0.05f));
			tmp.text = text;
			yield return new WaitForSeconds(PettyRandom.Range(0.025f, 0.05f));
		}
	}

	public IEnumerator AddCharsRandom(string text)
	{
		int count = PettyRandom.Range(5, 15);
		for (int i = 0; i < count; i++)
		{
			int num = text.RandomIndex();
			char c = Dead.Random.Choose<char>(extraChars.RandomItem(), text[num]);
			tmp.text = text.Remove(num, 1).Insert(num, $"<#{extraCharColourHexes.RandomItem()}>{c}</color>");
			yield return new WaitForSeconds(PettyRandom.Range(0.05f, 0.1f));
			tmp.text = text;
			yield return new WaitForSeconds(0.1f);
		}
	}

	public IEnumerator AppendChars(string text)
	{
		int count = PettyRandom.Range(1, 3);
		int index = text.Length - 1;
		char finalChar = text[index];
		for (int i = 0; i < count; i++)
		{
			tmp.text += $"<#{extraCharColourHexes.RandomItem()}>{finalChar}</color>";
			yield return new WaitForSeconds(PettyRandom.Range(0.1f, 0.2f));
		}

		yield return new WaitForSeconds(0.3f);
		tmp.text = text;
	}

	public IEnumerator AddChar(string text)
	{
		int index = text.RandomIndex();
		char c = Dead.Random.Choose<char>(extraChars.RandomItem(), text[index]);
		int count = Dead.Random.Choose<int>(1, 2, 2);
		for (int i = 0; i < count; i++)
		{
			tmp.text = tmp.text.Insert(index, $"<#{extraCharColourHexes.RandomItem()}>{c}</color>");
			yield return new WaitForSeconds(PettyRandom.Range(1f, 2f));
		}

		tmp.text = text;
	}

	public IEnumerator ReplaceChar(string text)
	{
		int index = text.RandomIndex();
		char c = extraChars.RandomItem();
		tmp.text = text.Remove(index, 1).Insert(index, $"<#{extraCharColourHexes.RandomItem()}>{c}</color>");
		yield return new WaitForSeconds(PettyRandom.Range(1f, 2f));
		tmp.text = text.Remove(index, 1).Insert(index, " ");
		yield return new WaitForSeconds(1f);
	}

	public IEnumerator ColourChar(string text)
	{
		int index = text.RandomIndex();
		char c = text[index];
		string col = extraCharColourHexes.RandomItem();
		tmp.text = text.Remove(index, 1).Insert(index, $"<#{col}>{c}</color>");
		yield return new WaitForSeconds(PettyRandom.Range(2f, 3f));
		int count = Dead.Random.Range(6, 10) * Dead.Random.Choose<int>(1, 1, 1, 1, 1, 3);
		for (int i = 0; i < count; i++)
		{
			tmp.text = text.Remove(index, 1).Insert(index, $"<#{col}>{extraChars.RandomItem()}</color>");
			yield return new WaitForSeconds(PettyRandom.Range(0.025f, 0.05f));
		}

		yield return new WaitForSeconds(1f);
		tmp.text = text;
	}
}
