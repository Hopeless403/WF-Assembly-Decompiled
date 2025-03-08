#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ScreenFlashSystem : GameSystem
{
	public static ScreenFlashSystem instance;

	[SerializeField]
	public SpriteRenderer renderer;

	[SerializeField]
	public AnimationCurve curve;

	[SerializeField]
	public Material basicMaterial;

	[SerializeField]
	public Material additiveMaterial;

	public Color color;

	public float current;

	public float duration;

	public float amount = 1f;

	public void Awake()
	{
		instance = this;
	}

	public void Update()
	{
		if (renderer.enabled)
		{
			current += Time.deltaTime;
			float time = Mathf.Min(1f, current / duration);
			float alpha = curve.Evaluate(time) * amount;
			renderer.color = color.WithAlpha(alpha);
			if (current >= duration)
			{
				renderer.enabled = false;
				renderer.material = basicMaterial;
			}
		}
	}

	public static void SetDrawOrder(string sortingLayer, int orderInLayer)
	{
		instance._SetDrawOrder(sortingLayer, orderInLayer);
	}

	public static void SetColour(Color color)
	{
		instance.color = color;
	}

	public static void SetMaterialAdditive()
	{
		instance.renderer.material = instance.additiveMaterial;
	}

	public static void Run(float duration)
	{
		instance._Run(duration);
	}

	public void _SetDrawOrder(string sortingLayer, int orderInLayer)
	{
		renderer.sortingLayerName = sortingLayer;
		renderer.sortingOrder = orderInLayer;
	}

	public void _Run(float duration)
	{
		current = 0f;
		renderer.enabled = true;
		this.duration = duration;
		amount = Mathf.Max(0.2f, Settings.Load("ScreenFlash", 1f));
	}
}
