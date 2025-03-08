#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class Deckpack : MonoBehaviourSingleton<Deckpack>
{
	public UnityEngine.Animator animator;

	[SerializeField]
	public Button button;

	public bool open;

	public static bool IsOpen => MonoBehaviourSingleton<Deckpack>.instance.open;

	public static void Open()
	{
		Events.InvokeDeckpackOpen();
		MonoBehaviourSingleton<Deckpack>.instance.animator.SetBool("Open", value: true);
		MonoBehaviourSingleton<Deckpack>.instance.open = true;
	}

	public static void Close()
	{
		Events.InvokeDeckpackClose();
		MonoBehaviourSingleton<Deckpack>.instance.animator.SetBool("Open", value: false);
		MonoBehaviourSingleton<Deckpack>.instance.open = false;
	}

	public void Ping()
	{
		animator.SetTrigger("Ping");
	}
}
