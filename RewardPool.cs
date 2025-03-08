#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reward Pool", fileName = "RewardPool")]
public class RewardPool : ScriptableObject
{
	public enum Type
	{
		Items,
		Units,
		Charms
	}

	public string type;

	public int copies = 1;

	public List<DataFile> list;

	public bool isGeneralPool;
}
