#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ObjectTrackerUIScript : MonoBehaviour
{
	public RectTransform parentCanvas;

	public GameObject objToTrack;

	public Vector3 localOffset;

	public Vector3 targetPos;

	public virtual void Update()
	{
		targetPos = RectTransformUtility.WorldToScreenPoint(Camera.main, objToTrack.transform.position).WithZ(0f);
		GetComponent<RectTransform>().position = localOffset + targetPos;
	}
}
