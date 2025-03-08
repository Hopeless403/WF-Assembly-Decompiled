#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class ItemHolderPetUsed : MonoBehaviour
{
	[SerializeField]
	public Image headImage;

	[SerializeField]
	public Vector2 velocityRangeX = new Vector2(10f, 15f);

	[SerializeField]
	public Vector2 velocityRangeY = new Vector2(11f, 13f);

	[SerializeField]
	public Vector2 velocityRangeZ = new Vector2(-15f, -10f);

	public void Start()
	{
		Vector3 dir = new Vector3(velocityRangeX.PettyRandom().WithRandomSign(), velocityRangeY.PettyRandom(), velocityRangeZ.PettyRandom());
		base.gameObject.GetOrAdd<FlyOffScreen>().Knockback(dir);
	}

	public void SetUp(Sprite headSprite)
	{
		if (headImage != null)
		{
			headImage.sprite = headSprite;
		}
	}
}
