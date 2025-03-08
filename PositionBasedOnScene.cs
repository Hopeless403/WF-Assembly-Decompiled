#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PositionBasedOnScene : MonoBehaviour
{
	[Serializable]
	public class ScenePosition
	{
		[SerializeField]
		public string sceneName;

		[SerializeField]
		public Vector3 position;
	}

	[SerializeField]
	public ScenePosition[] positions;

	public bool hasRectTransform;

	public RectTransform rt;

	public Transform t;

	public void OnEnable()
	{
		t = base.transform;
		if (t is RectTransform rectTransform)
		{
			hasRectTransform = true;
			rt = rectTransform;
		}

		Events.OnSceneChanged += ActiveSceneChanged;
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= ActiveSceneChanged;
	}

	public void ActiveSceneChanged(Scene to)
	{
		ScenePosition scenePosition = positions.FirstOrDefault((ScenePosition a) => a.sceneName == to.name);
		if (scenePosition != null)
		{
			SetPosition(scenePosition.position);
		}
	}

	public void SetPosition(Vector3 pos)
	{
		if (hasRectTransform)
		{
			rt.anchoredPosition = pos;
		}
		else
		{
			t.localPosition = pos;
		}
	}
}
