#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class ConsoleLog : MonoBehaviour
{
	[SerializeField]
	public TMP_Text textAsset;

	[SerializeField]
	public AnimationCurve fadeCurve;

	[SerializeField]
	public float fadeDuration = 4f;

	public float fade;

	public void OnEnable()
	{
		fade = 0f;
	}

	public void Update()
	{
		fade += Time.deltaTime;
		if (fade > fadeDuration)
		{
			base.gameObject.Destroy();
		}
		else
		{
			textAsset.color = textAsset.color.With(-1f, -1f, -1f, fadeCurve.Evaluate(fade / fadeDuration));
		}
	}
}
