#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Area", fileName = "Area")]
public class AreaData : ScriptableObject
{
	public EventReference battleMusicEvent;

	public EventReference minibossMusicEvent;

	public EventReference bossMusicEvent;

	public EventReference ambienceEvent;

	public AssetReferenceGameObject battleBackgroundPrefabRef;

	public Sprite battleBaseSprite;
}
