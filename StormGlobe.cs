#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class StormGlobe : ScriptableCardImage
{
	[SerializeField]
	public Image image;

	[SerializeField]
	public Sprite[] sprites;

	[SerializeField]
	public AnimationCurve spriteIndexCurve;

	public int effectBonus;

	public override void UpdateEvent()
	{
		if (entity.effectBonus != effectBonus)
		{
			effectBonus = entity.effectBonus;
			int num = Mathf.Clamp(Mathf.RoundToInt(spriteIndexCurve.Evaluate(effectBonus)), 0, sprites.Length - 1);
			image.sprite = sprites[num];
		}
	}
}
