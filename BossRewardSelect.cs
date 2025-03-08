#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class BossRewardSelect : MonoBehaviourRect
{
	[SerializeField]
	public InputAction inputAction;

	[SerializeField]
	public KeywordData popUpKeyword;

	public string title;

	public string body;

	public string popUpName;

	public bool poppedUp;

	public static readonly Vector2 popUpOffset = new Vector2(1f, 0f);

	public virtual void SetUp(BossRewardData.Data rewardData, GainBlessingSequence2 gainBlessingSequence)
	{
		inputAction.action.AddListener(delegate
		{
			gainBlessingSequence.Select(rewardData);
		});
		inputAction.action.AddListener(Destroy);
	}

	public void Pop()
	{
		if (!poppedUp)
		{
			CardPopUp.AssignTo(base.rectTransform, popUpOffset.x, popUpOffset.y);
			if ((bool)popUpKeyword)
			{
				CardPopUp.AddPanel(popUpKeyword);
			}
			else
			{
				CardPopUp.AddPanel(popUpName, title, body);
			}

			poppedUp = true;
		}
	}

	public void UnPop()
	{
		if (poppedUp)
		{
			CardPopUp.RemovePanel(popUpKeyword ? popUpKeyword.name : popUpName);
			poppedUp = false;
		}
	}

	public void Destroy()
	{
		UnPop();
		Object.Destroy(base.gameObject);
	}
}
