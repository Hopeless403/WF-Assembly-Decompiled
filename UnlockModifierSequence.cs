#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class UnlockModifierSequence : MonoBehaviour
{
	[SerializeField]
	public ModifierIcon icon;

	public void Run(GameModifierData modifierData)
	{
		icon.Set(modifierData, Vector2.zero);
		base.gameObject.SetActive(value: true);
	}
}
