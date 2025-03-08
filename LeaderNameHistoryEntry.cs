#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class LeaderNameHistoryEntry : MonoBehaviour
{
	[SerializeField]
	public Transform offset;

	[SerializeField]
	public TMP_Text textElement;

	[SerializeField]
	public AnimationCurve scaleCurve;

	[SerializeField]
	public Gradient colour;

	[SerializeField]
	public Gradient missingColour;

	public void Assign(JournalNameHistory.Name name)
	{
		offset.localPosition = name.offset;
		offset.localEulerAngles = Vector2.zero.WithZ(name.angle);
		offset.localScale = Vector3.one * scaleCurve.Evaluate(name.opacity);
		textElement.text = name.text;
		if (name.killed)
		{
			textElement.fontStyle = FontStyles.Strikethrough;
		}

		if (name.missing)
		{
			textElement.text += " ?";
			textElement.color = missingColour.Evaluate(name.opacity);
		}
		else
		{
			textElement.color = colour.Evaluate(name.opacity);
		}
	}
}
