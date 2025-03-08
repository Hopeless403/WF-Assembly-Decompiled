#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSetter : MonoBehaviour
{
	[SerializeField]
	public Image image;

	[SerializeField]
	public Profile[] sprites;

	public void Set(string @in)
	{
		Profile profile = sprites.FirstOrDefault((Profile a) => a.@string == @in);
		if (profile != null)
		{
			image.sprite = profile.sprite;
		}
	}
}
