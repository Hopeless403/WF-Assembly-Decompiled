#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class WaveDot : MonoBehaviour
{
	[SerializeField]
	public Image onImage;

	[SerializeField]
	public Image offImage;

	public void TurnOn()
	{
		onImage.enabled = true;
		offImage.enabled = false;
	}

	public void TurnOff()
	{
		onImage.enabled = false;
		offImage.enabled = true;
	}
}
