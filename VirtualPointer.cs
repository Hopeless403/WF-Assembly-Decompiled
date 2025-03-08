#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class VirtualPointer : MonoBehaviourSingleton<VirtualPointer>
{
	public static void Hide()
	{
		if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			MonoBehaviourSingleton<VirtualPointer>.instance.gameObject.SetActive(value: false);
		}
	}

	public static void Show()
	{
		if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			MonoBehaviourSingleton<VirtualPointer>.instance.gameObject.SetActive(value: true);
		}
	}
}
