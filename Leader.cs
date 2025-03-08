#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class Leader : ScriptableCardImage
{
	[SerializeField]
	public CharacterAvatar avatar;

	public override void AssignEvent()
	{
		avatar.UpdateDisplay(entity.data.customData.Get<CharacterData>("CharacterData"));
	}
}
