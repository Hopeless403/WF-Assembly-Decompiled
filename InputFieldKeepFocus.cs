#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldKeepFocus : MonoBehaviour
{
	public TMP_InputField _i;

	public PointerEventData _e;

	public bool focused;

	public TMP_InputField i => _i ?? (_i = GetComponent<TMP_InputField>());

	public PointerEventData e => _e ?? (_e = new PointerEventData(EventSystem.current));

	public void Update()
	{
		if (!focused)
		{
			if (!i.isFocused)
			{
				Debug.Log("[" + base.name + "] is not focused! Attempting to select");
				i.OnPointerClick(e);
			}

			if (i.isFocused)
			{
				focused = true;
			}
		}
	}

	public void OnDisable()
	{
		focused = false;
	}
}
