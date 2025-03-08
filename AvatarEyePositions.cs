#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public class AvatarEyePositions : MonoBehaviour
{
	[Serializable]
	public class Eye
	{
		public Vector3 pos;

		public Vector3 scale = new Vector3(2.12765956f, 2.12765956f, 1f);
	}

	public Eye[] eyes;
}
