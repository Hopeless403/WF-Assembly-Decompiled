#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using UnityEngine;

public class CrownHolder : UpgradeHolder
{
	[SerializeField]
	public float gap = 0.3f;

	[SerializeField]
	public float angleRange = 1f;

	[SerializeField]
	public float xRange = 0.02f;

	[SerializeField]
	public float xMax = 0.1f;

	public int seed;

	public void Awake()
	{
		seed = PettyRandom.Range(10000, 9999999);
	}

	public override void SetPositions()
	{
		UnityEngine.Random.State state = UnityEngine.Random.state;
		UnityEngine.Random.InitState(seed);
		Vector2 zero = Vector2.zero;
		Vector3 zero2 = Vector3.zero;
		foreach (RectTransform item in base.transform)
		{
			item.anchoredPosition = zero;
			item.localEulerAngles = zero2;
			float num = UnityEngine.Random.Range(-1f, 1f);
			float num2 = zero.x + num * xRange;
			if (num2 > xMax || num2 < 0f - xMax)
			{
				num *= -1f;
			}

			zero += new Vector2(num * xRange, gap);
			zero2.z += num * angleRange;
		}

		UnityEngine.Random.state = state;
	}

	public override void SetInputOverrides()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			UpgradeDisplay component = base.transform.GetChild(i).GetComponent<UpgradeDisplay>();
			component.navigationItem.overrideInputs = true;
			component.navigationItem.inputDown = ((i > 0) ? base.transform.GetChild(i - 1).GetComponent<UpgradeDisplay>().navigationItem : null);
			component.navigationItem.inputUp = ((i < list.Count - 1) ? base.transform.GetChild(i + 1).GetComponent<UpgradeDisplay>().navigationItem : null);
		}
	}
}
