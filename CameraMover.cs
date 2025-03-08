#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMover : GameSystem
{
	[Serializable]
	public class Translation
	{
		public string name;

		public Vector3 position;

		public Vector3 rotation;

		public LeanTweenType ease;

		public float dur;
	}

	[SerializeField]
	public Translation[] battlePhasePositions;

	public void OnEnable()
	{
		Events.OnBattlePhaseStart += BattlePhaseStart;
		Events.OnSceneChanged += SceneChange;
	}

	public void OnDisable()
	{
		Events.OnBattlePhaseStart -= BattlePhaseStart;
		Events.OnSceneChanged -= SceneChange;
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		Translation translation = battlePhasePositions.FirstOrDefault((Translation a) => a.name == phase.ToString().ToLower());
		if (translation != null)
		{
			LeanTween.cancel(base.gameObject);
			LeanTween.moveLocal(base.gameObject, translation.position, translation.dur).setEase(translation.ease);
			LeanTween.rotateLocal(base.gameObject, translation.rotation, translation.dur).setEase(translation.ease);
		}
	}

	public void SceneChange(Scene scene)
	{
		LeanTween.cancel(base.gameObject);
		base.transform.localPosition = Vector3.zero;
		base.transform.localEulerAngles = Vector3.zero;
	}
}
