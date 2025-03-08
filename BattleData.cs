#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "BattleData", menuName = "Battle")]
public class BattleData : DataFile
{
	public string title;

	public float pointFactor = 1f;

	public int waveCounter = 4;

	public BattleWavePoolData[] pools;

	public CardData[] bonusUnitPool;

	public Vector2Int bonusUnitRange;

	public CardData[] goldGiverPool;

	public int goldGivers = 1;

	public BattleGenerationScript generationScript;

	public Script setUpScript;

	public Sprite sprite;

	public UnityEngine.Localization.LocalizedString nameRef;
}
