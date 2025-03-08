#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class ModifierIcon : MonoBehaviourRect
{
	[SerializeField]
	public LayoutElement layoutElement;

	[SerializeField]
	public RectTransform bellHolder;

	public ImageSprite bellImage;

	[SerializeField]
	public ImageSprite dingerImage;

	[SerializeField]
	public GameObject activeIcon;

	[SerializeField]
	public GameObject inactiveIcon;

	[SerializeField]
	public TweenUI[] dingTweens;

	public bool playDingSfx = true;

	public string title;

	public string body;

	[SerializeField]
	public GameModifierData modifier;

	public string popUpName;

	[SerializeField]
	public bool dontPop;

	public bool poppedUp;

	public Vector2 popUpOffset;

	public virtual void Set(GameModifierData modifier, Vector2 popUpOffset)
	{
		this.modifier = modifier;
		this.popUpOffset = popUpOffset;
		if ((bool)dingerImage)
		{
			dingerImage.SetSprite(this.modifier.dingerSprite);
		}

		if ((bool)bellImage)
		{
			bellImage.SetSprite(modifier.bellSprite);
			float num = (Mathf.Max(1f, bellImage.rectTransform.sizeDelta.y) - 1f) * 0.5f;
			bellHolder.anchoredPosition = new Vector2(0f, num);
			if ((bool)layoutElement)
			{
				layoutElement.preferredHeight = 1f + num;
			}
		}

		popUpName = modifier.name;
		title = modifier.titleKey.GetLocalizedString();
		body = modifier.descriptionKey.GetLocalizedString();
	}

	public void UpdateText()
	{
		title = modifier.titleKey.GetLocalizedString();
		body = modifier.descriptionKey.GetLocalizedString();
	}

	public void SetText(string title, string body)
	{
		this.title = title;
		this.body = body;
	}

	public void AddText(string toBody)
	{
		body += toBody;
	}

	public virtual void Pop()
	{
		if (!dontPop && !poppedUp)
		{
			CardPopUp.AssignTo(base.rectTransform, popUpOffset.x, popUpOffset.y);
			CardPopUp.AddPanel(popUpName, title, body);
			poppedUp = true;
		}
	}

	public virtual void UnPop()
	{
		if (!dontPop && poppedUp)
		{
			CardPopUp.RemovePanel(popUpName);
			poppedUp = false;
		}
	}

	public void OnDisable()
	{
		UnPop();
	}

	[Button(null, EButtonEnableMode.Always)]
	public void Ding()
	{
		TweenUI[] array = dingTweens;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Fire();
		}

		if (playDingSfx)
		{
			modifier.PlayRingSfx();
		}
	}
}
