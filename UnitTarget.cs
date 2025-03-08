#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class UnitTarget : MonoBehaviour
{
	[SerializeField]
	public GameObject aimlessOverlay;

	[SerializeField]
	public GameObject frenzyUnderlay;

	public void SetAimless(bool aimless)
	{
		aimlessOverlay.SetActive(aimless);
	}

	public void SetFrenzy(bool frenzy)
	{
		frenzyUnderlay.SetActive(frenzy);
	}
}
