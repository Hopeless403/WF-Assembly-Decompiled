#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class FontSetter : MonoBehaviour
{
	public TMP_Text _textElement;

	public bool registered;

	public TMP_Text textElement => _textElement ?? (_textElement = GetComponent<TMP_Text>());

	public void OnEnable()
	{
		FontSetterSystem.Register(this);
		registered = true;
	}

	public void OnDisable()
	{
		if (registered)
		{
			FontSetterSystem.Unregister(this);
			registered = false;
		}
	}

	public void SetFont(TMP_FontAsset font)
	{
		textElement.font = font;
	}
}
