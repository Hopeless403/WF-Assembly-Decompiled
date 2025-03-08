#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class UINavigationLayer : MonoBehaviour
{
	public static uint idMax;

	public uint id;

	public bool isOverrideLayer = true;

	public bool setStartingItem = true;

	public bool allowOutsideVisibleSelection;

	public bool allowLayerToBeAppliedAtRuntime = true;

	public bool forceHover = true;

	public void Awake()
	{
		id = idMax++;
	}

	public void OnEnable()
	{
		MonoBehaviourSingleton<UINavigationSystem>.instance.RegisterNavigationLayer(this);
	}

	public void OnDisable()
	{
		MonoBehaviourSingleton<UINavigationSystem>.instance.UnregisterNavigationLayer(this);
	}

	public override bool Equals(object other)
	{
		if (other is UINavigationLayer uINavigationLayer)
		{
			return uINavigationLayer.id == id;
		}

		return false;
	}
}
