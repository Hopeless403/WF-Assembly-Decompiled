#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LayoutFixer : MonoBehaviourRect
{
	public enum Prompt
	{
		None,
		All,
		Horizontal,
		Vertical
	}

	[SerializeField]
	public bool unlimitedDepth = true;

	[SerializeField]
	[HideIf("unlimitedDepth")]
	public int depth;

	public Prompt prompt;

	public void Update()
	{
		if (prompt != 0)
		{
			switch (prompt)
			{
				case Prompt.All:
					DoAll();
					break;
				case Prompt.Horizontal:
					DoHorizontal();
					break;
				case Prompt.Vertical:
					DoVertical();
					break;
			}

			prompt = Prompt.None;
		}
	}

	public void Fix()
	{
		prompt = Prompt.All;
	}

	public void FixHorizontal()
	{
		prompt = Prompt.Horizontal;
	}

	public void FixVertical()
	{
		prompt = Prompt.Vertical;
	}

	public void DoAll()
	{
		if (unlimitedDepth)
		{
			base.rectTransform.FixLayoutGroups();
		}
		else
		{
			base.rectTransform.FixLayoutGroups(depth);
		}
	}

	public void DoHorizontal()
	{
		StopAllCoroutines();
		StartCoroutine(FixHorizontalRoutine());
	}

	public void DoVertical()
	{
		StopAllCoroutines();
		StartCoroutine(FixVerticalRoutine());
	}

	public IEnumerator FixHorizontalRoutine()
	{
		if (depth <= 0 && !unlimitedDepth)
		{
			ContentSizeFitter fitter = GetComponent<ContentSizeFitter>();
			if (fitter != null)
			{
				ContentSizeFitter.FitMode pre = fitter.horizontalFit;
				fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
				yield return null;
				fitter.horizontalFit = pre;
			}
		}
		else
		{
			base.rectTransform.DisableContentFitters();
			yield return null;
			base.rectTransform.EnableContentFitters();
		}
	}

	public IEnumerator FixVerticalRoutine()
	{
		if (depth <= 0 && !unlimitedDepth)
		{
			ContentSizeFitter fitter = GetComponent<ContentSizeFitter>();
			if (fitter != null)
			{
				ContentSizeFitter.FitMode pre = fitter.verticalFit;
				fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
				yield return null;
				fitter.verticalFit = pre;
			}
		}
		else
		{
			base.rectTransform.DisableContentFitters();
			yield return null;
			base.rectTransform.EnableContentFitters();
		}
	}
}
