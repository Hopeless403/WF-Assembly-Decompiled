#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public static class NavigationState
{
	public static readonly Stack<INavigationState> stateHistory = new Stack<INavigationState>();

	public static INavigationState PeekCurrentState()
	{
		if (stateHistory.Count <= 0)
		{
			return null;
		}

		return stateHistory.Peek();
	}

	public static INavigationState PopCurrentState()
	{
		if (stateHistory.Count <= 0)
		{
			return null;
		}

		return stateHistory.Pop();
	}

	public static void Start(INavigationState state)
	{
		INavigationState navigationState = PeekCurrentState();
		if (navigationState != state)
		{
			navigationState?.End();
			if (state != null)
			{
				state.Begin();
				stateHistory.Push(state);
			}
		}
	}

	public static void BackToPreviousState()
	{
		if (stateHistory.Count > 0)
		{
			PopCurrentState()?.End();
			PeekCurrentState()?.Begin();
		}
	}

	public static void Reset()
	{
		stateHistory.Clear();
		Start(new NavigationStateIdle());
	}
}
