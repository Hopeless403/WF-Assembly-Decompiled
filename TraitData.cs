#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "New Trait", menuName = "Trait")]
public class TraitData : DataFile
{
	public KeywordData keyword;

	public StatusEffectData[] effects;

	public TraitData[] overrides;

	public bool isReaction;
}
