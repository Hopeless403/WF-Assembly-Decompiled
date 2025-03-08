#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatusIconReaction : StatusIcon
{
	public Sprite snowSprite;

	public Image _image;

	public Sprite baseSprite;

	public int snowPre;

	public Image image
	{
		get
		{
			if (_image == null)
			{
				_image = GetComponent<Image>();
				baseSprite = _image.sprite;
			}

			return _image;
		}
	}

	public void UpdateDisplay()
	{
		int num = (base.target ? base.target.SnowAmount() : 0);
		if (num > 0 && (bool)snowSprite)
		{
			image.sprite = snowSprite;
		}
		else
		{
			image.sprite = baseSprite;
		}

		snowPre = num;
	}

	public override void CheckRemove()
	{
		if (!base.target.statusEffects.Any((StatusEffectData a) => a.isReaction))
		{
			SetValue(default(Stat));
			base.target.display.reactionIcon = null;
		}
	}
}
