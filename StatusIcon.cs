#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviourRect
{
	public string type;

	public TMP_Text textElement;

	public bool persistent;

	[SerializeField]
	public bool alterTextColours = true;

	[ShowIf("alterTextColours")]
	public Color textColour = Color.black;

	[ShowIf("alterTextColours")]
	public Color textColourAboveMax = Color.white;

	[ShowIf("alterTextColours")]
	public Color textColourBelowMax = Color.white;

	[ShowIf("alterTextColours")]
	public Material textMaterialAboveMax;

	public Image fill;

	public Stat value;

	[SerializeField]
	public string textFormat = "{0}";

	[SerializeField]
	public int textAdd;

	public Material normalMaterial;

	public bool currentMaterialIsNormal = true;

	public UnityEvent onCreate;

	public UnityEventStatStat onValueDown;

	public UnityEventStatStat onValueUp;

	public UnityEvent afterUpdate;

	public UnityEvent onDestroy;

	public Entity target { get; set; }

	public virtual Stat GetValue()
	{
		return value;
	}

	public void SetValue(Stat value, bool doPing = true)
	{
		if (doPing)
		{
			UpdateEvent(this.value, value);
		}

		this.value = value;
		afterUpdate.Invoke();
		if ((bool)fill)
		{
			fill.type = Image.Type.Filled;
			fill.fillAmount = Mathf.Clamp((float)value.current / (float)value.max, 0f, 1f);
		}
	}

	public virtual void Assign(Entity entity)
	{
		target = entity;
	}

	public void CreateEvent()
	{
		onCreate.Invoke();
	}

	public void UpdateEvent(Stat previousValue, Stat newValue)
	{
		if (newValue.current < previousValue.current)
		{
			onValueDown.Invoke(previousValue, newValue);
		}
		else if (newValue.current > previousValue.current)
		{
			onValueUp.Invoke(previousValue, newValue);
		}

		Events.InvokeStatusIconChanged(this, previousValue, newValue);
	}

	public void DestroyEvent()
	{
		onDestroy.Invoke();
	}

	public void SetText()
	{
		if (!textElement)
		{
			return;
		}

		Stat stat = GetValue();
		textElement.text = string.Format(textFormat, stat.current + textAdd);
		if (!alterTextColours)
		{
			return;
		}

		textElement.color = ((stat.current > stat.max) ? textColourAboveMax : ((stat.current < stat.max) ? textColourBelowMax : textColour));
		if (stat.current > stat.max)
		{
			if (currentMaterialIsNormal && (bool)textMaterialAboveMax)
			{
				normalMaterial = textElement.fontSharedMaterial;
				currentMaterialIsNormal = false;
				textElement.fontSharedMaterial = textMaterialAboveMax;
			}
		}
		else if (!currentMaterialIsNormal && (bool)normalMaterial)
		{
			currentMaterialIsNormal = true;
			textElement.fontSharedMaterial = normalMaterial;
		}
	}

	public void Ping()
	{
		if ((bool)base.rectTransform)
		{
			base.rectTransform.LeanCancel();
			base.rectTransform.LeanScale(Vector3.one, 1f).setEaseOutElastic().setFrom(new Vector3(0f, 0f, 1f));
		}
	}

	public virtual void CheckRemove()
	{
		if (!persistent && !target.statusEffects.Find((StatusEffectData a) => a.type == type))
		{
			SetValue(default(Stat));
		}
	}

	public void CheckDestroy(Stat previousValue, Stat newValue)
	{
		if (newValue.current <= 0 && newValue.max <= 0 && !persistent)
		{
			Destroy();
		}
	}

	public void Destroy()
	{
		DestroyEvent();
		base.gameObject.Destroy();
	}
}
