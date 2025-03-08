#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class LeanAudioStream
{
	public int position;

	public AudioClip audioClip;

	public float[] audioArr;

	public LeanAudioStream(float[] audioArr)
	{
		this.audioArr = audioArr;
	}

	public void OnAudioRead(float[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = audioArr[position];
			position++;
		}
	}

	public void OnAudioSetPosition(int newPosition)
	{
		position = newPosition;
	}
}
