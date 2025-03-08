#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class CardCharmHolder : UpgradeHolder
{
	[SerializeField]
	public float angleAdd = 20f;

	[SerializeField]
	public Vector2 charmPivot = new Vector2(0.5f, 0.9f);

	[SerializeField]
	public Image ropeImage;

	public override void SetPositions()
	{
		int count = list.Count;
		float num = (0f - (float)(count - 1) * 0.5f) * angleAdd;
		for (int i = 0; i < count; i++)
		{
			if (list[i] is CardCharm cardCharm)
			{
				cardCharm.SetAngle(num + (float)i * angleAdd);
				cardCharm.transform.SetSiblingIndex(CalculateSiblingIndex(i, count));
			}
		}

		ropeImage?.gameObject.SetActive(count > 0);
	}

	public static int CalculateSiblingIndex(int listIndex, int listLength)
	{
		float num = (float)(listLength - 1) * 0.5f;
		float num2 = (float)listIndex - num;
		float num3 = Mathf.Sign(num2);
		return Mathf.FloorToInt(num + num2 * (0f - num3)) * 2 + Mathf.Clamp((int)num2, 0, 1);
	}

	public override void Add(UpgradeDisplay upgrade)
	{
		base.Add(upgrade);
		CharmAdded(upgrade);
	}

	public override void Insert(int index, UpgradeDisplay upgrade)
	{
		base.Insert(index, upgrade);
		CharmAdded(upgrade);
	}

	public void CharmAdded(UpgradeDisplay upgrade)
	{
		if (upgrade is CardCharm cardCharm)
		{
			cardCharm.holder = base.transform;
			((RectTransform)cardCharm.transform).pivot = charmPivot;
		}
	}

	public override void Clear()
	{
		base.Clear();
		ropeImage?.gameObject.SetActive(value: false);
	}

	public override void SetInputOverrides()
	{
		for (int i = 0; i < list.Count; i++)
		{
			UpgradeDisplay upgradeDisplay = list[i];
			upgradeDisplay.navigationItem.overrideInputs = true;
			upgradeDisplay.navigationItem.inputLeft = ((i > 0) ? list[i - 1].navigationItem : null);
			upgradeDisplay.navigationItem.inputRight = ((i < list.Count - 1) ? list[i + 1].navigationItem : null);
		}
	}
}
