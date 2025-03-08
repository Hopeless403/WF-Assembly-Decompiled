#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CharacterAvatarMoveToCard : MonoBehaviour
{
	[SerializeField]
	public Vector3 inCardPosition;

	[SerializeField]
	public Vector3 inCardRotation;

	[SerializeField]
	public Vector3 inCardScale;

	public void MoveToCard(Card card)
	{
		base.transform.SetParent(card.mainImage.transform);
		base.transform.localPosition = inCardPosition;
		base.transform.localEulerAngles = inCardRotation;
		base.transform.localScale = inCardScale;
	}
}
