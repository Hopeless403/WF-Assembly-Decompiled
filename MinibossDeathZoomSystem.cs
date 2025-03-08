#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MinibossDeathZoomSystem : GameSystem
{
	[SerializeField]
	public Transform container;

	[SerializeField]
	public float zoomAmount = 4f;

	[SerializeField]
	public Image fade;

	[SerializeField]
	public Color fadeColor;

	[SerializeField]
	public Color flashColor;

	[SerializeField]
	public Image flash;

	[SerializeField]
	public AnimationCurve flashCurve;

	[SerializeField]
	public float slowmo = 0.1f;

	[SerializeField]
	public float duration = 0.3f;

	[SerializeField]
	public Vector3 limit = new Vector3(5.5f, 2.5f, 5f);

	public Entity target;

	public Transform targetPreviousParent;

	public bool pauseBlocked;

	public bool slowmoActive;

	public void OnEnable()
	{
		Events.OnEntityKilled += EntityKilled;
	}

	public void OnDisable()
	{
		Events.OnEntityKilled -= EntityKilled;
		UnblockPause();
		EndSlowmo();
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if ((bool)entity && (bool)entity.data)
		{
			CardType cardType = entity.data.cardType;
			if ((object)cardType != null && cardType.miniboss && References.Battle.minibosses.Count((Entity a) => a != entity && a.owner == entity.owner) <= 0)
			{
				Run(entity);
			}
		}
	}

	public void Run(Entity target)
	{
		StartCoroutine(Routine(target));
	}

	public void BlockPause()
	{
		if (!pauseBlocked)
		{
			pauseBlocked = true;
			PauseMenu.Block();
			DeckpackBlocker.Block();
			if (Deckpack.IsOpen && References.Player.entity.display is CharacterDisplay characterDisplay)
			{
				characterDisplay.CloseInventory();
			}
		}
	}

	public void UnblockPause()
	{
		if (pauseBlocked)
		{
			pauseBlocked = false;
			DeckpackBlocker.Unblock();
			PauseMenu.Unblock();
		}
	}

	public void Slowmo()
	{
		if (!slowmoActive)
		{
			slowmoActive = true;
			LeanTween.value(Time.timeScale, slowmo, 0.05f).setEase(LeanTweenType.linear).setOnUpdate(Events.InvokeTimeScaleChange);
		}
	}

	public void EndSlowmo()
	{
		if (slowmoActive)
		{
			slowmoActive = false;
			LeanTween.value(Time.timeScale, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(Events.InvokeTimeScaleChange);
		}
	}

	public IEnumerator Routine(Entity target)
	{
		BlockPause();
		if ((bool)this.target)
		{
			this.target.transform.SetParent(targetPreviousParent);
		}

		yield return null;
		this.target = target;
		targetPreviousParent = target.transform.parent;
		target.transform.SetParent(container);
		target.ResetDrawOrder();
		HitFlashSystem.Remove(target);
		FlyOffScreen component = target.GetComponent<FlyOffScreen>();
		if ((bool)component)
		{
			component.velocity.z = 0f;
			component.velocity = component.velocity.normalized * 20f;
			component.velocity.z = -3f;
		}

		ScreenFlashSystem.SetDrawOrder("Inspect", 1);
		ScreenFlashSystem.SetColour(flashColor);
		ScreenFlashSystem.Run(0.1f);
		Slowmo();
		LeanTween.value(base.gameObject, fade.color.a, fadeColor.a, duration).setEase(LeanTweenType.easeInOutQuint).setOnUpdate(delegate(float a)
		{
			fade.color = fade.color.With(-1f, -1f, -1f, a);
		});
		Vector3 v = target.transform.position.WithZ(zoomAmount) - References.MinibossCameraMover.position;
		v = v.Clamp(-limit, limit);
		LeanTween.cancel(References.MinibossCameraMover.gameObject);
		LeanTween.moveLocal(References.MinibossCameraMover.gameObject, v, Mathf.Min(duration, 0.4f)).setEase(LeanTweenType.easeOutBack);
		yield return Sequences.Wait(duration);
		EndSlowmo();
		LeanTween.moveLocal(References.MinibossCameraMover.gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeOutBack);
		LeanTween.value(base.gameObject, fade.color.a, 0f, 0.25f).setEase(LeanTweenType.easeInOutQuart).setOnUpdate(delegate(float a)
		{
			fade.color = fade.color.With(-1f, -1f, -1f, a);
		});
		yield return Sequences.Wait(1f);
		if ((bool)this.target)
		{
			this.target.transform.SetParent(targetPreviousParent);
		}

		this.target = null;
		UnblockPause();
	}
}
