#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
	[SerializeField]
	public CardData data;

	public Card _card;

	public Card card => _card ?? (_card = GetComponent<Card>());

	public void Awake()
	{
		base.transform.localScale = Vector3.zero;
	}

	public IEnumerator Start()
	{
		card.entity.data = data;
		yield return card.UpdateData();
		LeanTween.scale(base.gameObject, Vector3.one, 0.3f).setEaseOutBack();
	}
}
