#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class LTUtility
{
	public static Vector3[] reverse(Vector3[] arr)
	{
		int num = arr.Length;
		int num2 = 0;
		int num3 = num - 1;
		while (num2 < num3)
		{
			Vector3 vector = arr[num2];
			arr[num2] = arr[num3];
			arr[num3] = vector;
			num2++;
			num3--;
		}

		return arr;
	}
}
