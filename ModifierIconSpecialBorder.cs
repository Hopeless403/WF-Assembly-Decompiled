#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class ModifierIconSpecialBorder : MonoBehaviour
{
	[SerializeField]
	public Image border;

	[SerializeField]
	public Sprite specialBorder;

	public void SetSpecialBorder()
	{
		border.sprite = specialBorder;
	}
}
