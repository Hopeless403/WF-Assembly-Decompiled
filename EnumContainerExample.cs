#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using BetaJester.EnumGenerator;
using UnityEngine;

public class EnumContainerExample : MonoBehaviour, IEnumContainer
{
	public List<ObjectInfo> objectInfos = new List<ObjectInfo>();

	public EnumInfo[] GetEnums()
	{
		return new EnumInfo[1]
		{
			new EnumInfo
			{
				_name = "ObjectType",
				_values = objectInfos.Select((ObjectInfo x) => x.objectName).ToArray()
			}
		};
	}
}
