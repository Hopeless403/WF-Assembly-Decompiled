#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class TutorialParentSystem : GameSystem
{
	public abstract class Phase
	{
		public bool _enabled;

		public bool enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				bool flag = _enabled;
				_enabled = value;
				if (_enabled)
				{
					if (!flag)
					{
						OnEnable();
					}
				}
				else if (flag)
				{
					OnDisable();
				}
			}
		}

		public virtual float delay => 0f;

		public virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
		}

		public static bool FreeMoveAction(PlayAction action)
		{
			if (action is ActionMove actionMove && Battle.IsOnBoard(actionMove.entity))
			{
				return Battle.IsOnBoard(actionMove.toContainers);
			}

			return false;
		}

		public static bool InspectAction(PlayAction action)
		{
			return action is ActionInspect;
		}

		public Phase()
		{
		}
	}

	public float delay;

	public const float delayBetween = 0f;

	public Phase currentPhase;

	public List<Phase> phases;

	public virtual void OnEnable()
	{
	}

	public virtual void OnDisable()
	{
		if (currentPhase != null)
		{
			currentPhase.enabled = false;
			currentPhase = null;
		}
	}

	public virtual void Update()
	{
		if (delay > 0f)
		{
			delay -= Time.deltaTime;
		}
		else
		{
			if (phases == null || phases.Count <= 0)
			{
				return;
			}

			if (currentPhase == null)
			{
				currentPhase = phases[0];
				phases.RemoveAt(0);
				currentPhase.enabled = true;
			}
			else if (!currentPhase.enabled)
			{
				currentPhase = null;
				delay = 0f;
				if (phases.Count > 0)
				{
					delay += phases[0].delay;
				}
			}
		}
	}
}
