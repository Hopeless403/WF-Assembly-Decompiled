#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class LargeUIScaleUpdater : MonoBehaviour
{
	[SerializeField]
	public Vector3 scaleMin = Vector3.one;

	[SerializeField]
	public Vector3 scaleMax = Vector3.one;

	[SerializeField]
	public Vector3 positionMin;

	[SerializeField]
	public Vector3 positionMax;

	public Transform _transform;

	public Transform t => _transform ?? (_transform = base.transform);

	public void PromptUpdate()
	{
		t.localScale = Vector3.Lerp(scaleMin, scaleMax, 0f);
		t.localPosition = Vector3.Lerp(positionMin, positionMax, 0f);
	}
}
