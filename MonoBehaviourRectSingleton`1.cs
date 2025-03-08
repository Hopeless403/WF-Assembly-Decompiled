#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class MonoBehaviourRectSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T _instance;

	public RectTransform _rectTransform;

	public static T instance
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				_instance = Object.FindObjectOfType<T>();
			}

			return _instance;
		}
	}

	public RectTransform rectTransform
	{
		get
		{
			if (_rectTransform == null)
			{
				_rectTransform = (RectTransform)base.transform;
			}

			return _rectTransform;
		}
	}
}
