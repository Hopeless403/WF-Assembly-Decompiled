#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SwayerColour : MonoBehaviour
{
	[SerializeField]
	public bool randomStart;

	[HideIf("sprite")]
	public Graphic graphic;

	[HideIf("ui")]
	public SpriteRenderer renderer;

	public Gradient gradient;

	public float speed = 1f;

	public AnimationCurve curve;

	public float t;

	public bool ui => graphic != null;

	public bool sprite => renderer != null;

	public void Awake()
	{
		if (randomStart)
		{
			t = PettyRandom.Range(0f, 10f);
		}
	}

	public void Update()
	{
		t = (t + Time.deltaTime * speed) % 1f;
		float time = curve.Evaluate(t);
		Color color = gradient.Evaluate(time);
		if (ui)
		{
			graphic.color = color;
		}

		if (sprite)
		{
			renderer.color = color;
		}
	}
}
