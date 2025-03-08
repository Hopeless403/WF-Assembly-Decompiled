#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class TutorialBattleSystem : TutorialParentSystem
{
	public override void OnEnable()
	{
		base.OnEnable();
		Events.OnSceneChanged += SceneChanged;
		Events.OnBattlePhaseStart += BattlePhaseStart;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		Events.OnSceneChanged -= SceneChanged;
		Events.OnBattlePhaseStart -= BattlePhaseStart;
		BattleEnd();
	}

	public void SceneChanged(Scene scene)
	{
		Object.Destroy(this);
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		if (phase == Battle.Phase.Init)
		{
			BattleStart();
		}
	}

	public virtual void BattleStart()
	{
	}

	public virtual void BattleEnd()
	{
	}

	public TutorialBattleSystem()
	{
	}
}
