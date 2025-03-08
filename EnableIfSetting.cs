#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class EnableIfSetting : MonoBehaviour
{
	public enum Type
	{
		Bool,
		String,
		Int,
		FloatMin,
		FloatMax
	}

	[SerializeField]
	public string key;

	[SerializeField]
	public Type type;

	[SerializeField]
	[ShowIf("TypeBool")]
	public bool expectedBool = true;

	[SerializeField]
	[ShowIf("TypeBool")]
	public bool defaultBool;

	[SerializeField]
	[ShowIf("TypeString")]
	public string expectedString;

	[SerializeField]
	[ShowIf("TypeString")]
	public string defaultString;

	[SerializeField]
	[ShowIf("TypeInt")]
	public string expectedInt;

	[SerializeField]
	[ShowIf("TypeInt")]
	public string defaultInt;

	[SerializeField]
	[ShowIf("TypeFloatMin")]
	public float minFloat;

	[SerializeField]
	[ShowIf("TypeFloatMax")]
	public float maxFloat;

	[SerializeField]
	[ShowIf("TypeFloat")]
	public float defaultFloat;

	public bool TypeBool => type == Type.Bool;

	public bool TypeString => type == Type.String;

	public bool TypeInt => type == Type.Int;

	public bool TypeFloat
	{
		get
		{
			Type type = this.type;
			return type == Type.FloatMin || type == Type.FloatMax;
		}
	}

	public bool TypeFloatMin => type == Type.FloatMin;

	public bool TypeFloatMax => type == Type.FloatMax;

	public void Awake()
	{
		switch (type)
		{
			case Type.Bool:
				base.gameObject.SetActive(Settings.Load(key, defaultBool) == expectedBool);
				break;
			case Type.String:
				base.gameObject.SetActive(Settings.Load(key, defaultString) == expectedString);
				break;
			case Type.Int:
				base.gameObject.SetActive(Settings.Load(key, defaultInt) == expectedInt);
				break;
			case Type.FloatMin:
				base.gameObject.SetActive(Settings.Load(key, defaultFloat) >= minFloat);
				break;
			case Type.FloatMax:
				base.gameObject.SetActive(Settings.Load(key, defaultFloat) <= maxFloat);
				break;
		}
	}
}
