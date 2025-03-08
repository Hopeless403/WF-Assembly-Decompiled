#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class DisableIf : MonoBehaviour
{
	[SerializeField]
	public bool not;

	[SerializeField]
	public bool RELEASE;

	[SerializeField]
	public bool DEMO;

	[SerializeField]
	public bool CHALLENGES;

	[SerializeField]
	public bool BATTLE_LOG;

	[SerializeField]
	public bool ANALYTICS;

	[SerializeField]
	public bool JOURNAL_PAGES;

	[SerializeField]
	public bool HARD_MODE;

	[SerializeField]
	public bool CANNOT_EXIT;

	[SerializeField]
	public bool BETA;

	[SerializeField]
	public bool NEW_FROST_BELLS;

	[SerializeField]
	public bool destroy;

	public void OnEnable()
	{
		bool flag = false;
		if (RELEASE)
		{
			flag = true;
		}

		if (CHALLENGES)
		{
			flag = true;
		}

		if (BATTLE_LOG)
		{
			flag = true;
		}

		if (JOURNAL_PAGES)
		{
			flag = true;
		}

		if (NEW_FROST_BELLS)
		{
			flag = true;
		}

		if ((not && !flag) || (!not && flag))
		{
			Disable();
		}
	}

	public void Disable()
	{
		if (destroy)
		{
			base.gameObject.DestroyImmediate();
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
