#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
	[SerializeField]
	public Vector2 x;

	[SerializeField]
	public Vector2 y;

	public void OnEnable()
	{
		Randomize();
	}

	[Button(null, EButtonEnableMode.Always)]
	public void Randomize()
	{
		base.transform.localPosition = new Vector2(x.Random(), y.Random());
	}
}
