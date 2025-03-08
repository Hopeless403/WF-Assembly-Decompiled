#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class InputSwitcher : MonoBehaviour
{
	public static int justSwitchedCount = 3;

	public const int maxJustSwitchedCount = 3;

	public static bool justSwitched;

	public int currentIndex = -1;

	[SerializeField]
	public BaseInputSwitcher startingInput;

	[SerializeReference]
	public BaseInputSwitcher[] switchers;

	public void Awake()
	{
		BaseInputSwitcher[] array = switchers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(value: false);
		}
	}

	public void Start()
	{
		int num = switchers.Length;
		for (int i = 0; i < num; i++)
		{
			if (switchers[i] == startingInput)
			{
				SwitchTo(i);
				break;
			}
		}
	}

	public void Update()
	{
		int num = switchers.Length;
		for (int i = 0; i < num; i++)
		{
			if (switchers[i].CheckSwitchTo())
			{
				if (i != currentIndex)
				{
					SwitchTo(i);
				}

				break;
			}
		}

		if (justSwitched && --justSwitchedCount <= 0)
		{
			justSwitched = false;
		}
	}

	public void SwitchTo(int profileIndex)
	{
		if (currentIndex >= 0)
		{
			switchers[currentIndex].gameObject.SetActive(value: false);
		}

		switchers[profileIndex].SwitchTo();
		currentIndex = profileIndex;
		justSwitched = true;
		justSwitchedCount = 3;
	}
}
