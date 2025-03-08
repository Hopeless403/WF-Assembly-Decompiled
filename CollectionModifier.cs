#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectionModifier", menuName = "Character/Collection Modifier")]
public class CollectionModifier : ScriptableObject
{
	[Serializable]
	public struct Modify
	{
		public int index;

		public float addWeight;
	}

	public Modify[] list;
}
