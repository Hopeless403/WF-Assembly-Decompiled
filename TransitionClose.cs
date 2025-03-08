#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class TransitionClose : TransitionType
{
	[SerializeField]
	public Transform rotator;

	[SerializeField]
	public GameObject borderTop;

	[SerializeField]
	public GameObject borderBottom;

	[SerializeField]
	public CanvasGroup fade;

	[SerializeField]
	public Vector2 angleRange = new Vector2(0f, 40f);

	[Header("Tweens")]
	[SerializeField]
	public LeanTweenType easeIn = LeanTweenType.easeOutBounce;

	[SerializeField]
	public float easeInDur = 0.8f;

	[SerializeField]
	public LeanTweenType easeOut = LeanTweenType.easeInQuart;

	[SerializeField]
	public float easeOutDur = 0.5f;

	public Vector3 borderTopStartPos;

	public Vector3 borderBottomStartPos;

	public void Start()
	{
		borderTopStartPos = borderTop.transform.localPosition;
		borderBottomStartPos = borderBottom.transform.localPosition;
	}

	public override IEnumerator In()
	{
		base.IsRunning = true;
		fade.blocksRaycasts = true;
		fade.alpha = 0.01f;
		rotator.SetLocalRotationZ(angleRange.PettyRandom().WithRandomSign());
		LeanTween.moveLocal(borderTop, Vector3.zero, easeInDur).setEase(easeIn);
		LeanTween.moveLocal(borderBottom, Vector3.zero, easeInDur).setEase(easeIn);
		yield return new WaitForSeconds(easeInDur);
		base.IsRunning = false;
	}

	public override IEnumerator Out()
	{
		base.IsRunning = true;
		fade.blocksRaycasts = false;
		fade.alpha = 0f;
		LeanTween.moveLocal(borderTop, borderTopStartPos, easeOutDur).setEase(easeOut);
		LeanTween.moveLocal(borderBottom, borderBottomStartPos, easeOutDur).setEase(easeOut);
		yield return new WaitForSeconds(easeOutDur);
		base.IsRunning = false;
		base.gameObject.Destroy();
	}
}
