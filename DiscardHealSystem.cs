#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization;

public class DiscardHealSystem : GameSystem
{
	[SerializeField]
	public int healAmount = 5;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString healTextKey;

	[SerializeField]
	public Vector3 healPopupOffset = Vector3.up;

	public void OnEnable()
	{
		Events.OnDiscard += Discard;
	}

	public void OnDisable()
	{
		Events.OnDiscard -= Discard;
	}

	public void Discard(Entity entity)
	{
		ActionDiscardEffect action = new ActionDiscardEffect(entity, healAmount);
		if (Events.CheckAction(action))
		{
			ActionQueue.Add(action);
			if (entity.data.hasHealth)
			{
				Vector3 position = entity.transform.position + healPopupOffset;
				string text = "<size=0.5>" + string.Format(healTextKey.GetLocalizedString(), healAmount);
				FloatingText.Create(position).SetText(text).SetSortingLayer("PopUp", 10)
					.Animate("Spring")
					.Fade("Smooth");
			}
		}
	}
}
