#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EyeData", menuName = "EyeData")]
public class EyeData : DataFile
{
	[Serializable]
	public struct Eye
	{
		public Vector2 position;

		public Vector2 scale;

		public float rotation;

		public Eye(Transform transform)
		{
			position = transform.localPosition;
			scale = transform.localScale;
			rotation = transform.localEulerAngles.z;
		}
	}

	public string cardData;

	public Eye[] eyes;

	public void Set(params EyePositionSaver[] eyePositions)
	{
		eyes = new Eye[eyePositions.Length];
		int num = 0;
		foreach (EyePositionSaver eyePositionSaver in eyePositions)
		{
			eyes[num++] = new Eye(eyePositionSaver.transform);
		}
	}
}
