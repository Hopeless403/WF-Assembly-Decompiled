#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization.Components;

public class BlessingSelect : MonoBehaviour
{
	[SerializeField]
	public InputAction inputAction;

	[SerializeField]
	public ImageSprite bellImage;

	[SerializeField]
	public ImageSprite dingerImage;

	[SerializeField]
	public LocalizeStringEvent titleString;

	[SerializeField]
	public LocalizeStringEvent descString;

	public void SetUp(BlessingData blessingData, GainBlessingSequence gainBlessingSequence)
	{
		inputAction.action.AddListener(delegate
		{
			gainBlessingSequence.SelectBlessing(blessingData);
		});
		bellImage.SetSprite(blessingData.modifierToAdd.bellSprite);
		dingerImage.SetSprite(blessingData.modifierToAdd.dingerSprite);
		titleString.StringReference = blessingData.modifierToAdd.titleKey;
		descString.StringReference = blessingData.modifierToAdd.descriptionKey;
	}
}
