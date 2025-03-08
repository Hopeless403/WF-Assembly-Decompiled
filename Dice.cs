#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using NaughtyAttributes;
using UnityEngine;

public class Dice : MonoBehaviour
{
	public int value = -1;

	[SerializeField]
	public Rigidbody2D rb;

	[SerializeField]
	public SpriteRenderer spriteRenderer;

	[SerializeField]
	public Vector2 throwSpeed = new Vector2(20f, 40f);

	[SerializeField]
	public Vector2 spinAmount = new Vector2(4f, 6f);

	[SerializeField]
	public Vector2 drag = new Vector2(5f, 6f);

	[Header("Faces")]
	[SerializeField]
	public Sprite[] faceSprites;

	[SerializeField]
	public int[] faceValues = new int[6] { 1, 2, 3, 4, 5, 6 };

	public void Roll()
	{
		int maxInclusive = faceValues.Length - 1;
		int num = Dead.Random.Range(0, maxInclusive);
		value = faceValues[num];
		Sprite sprite = faceSprites[num];
		spriteRenderer.sprite = sprite;
	}

	[Button(null, EButtonEnableMode.Always)]
	public void Throw()
	{
		Dice[] array = Object.FindObjectsOfType<Dice>();
		foreach (Dice dice in array)
		{
			Vector3 normalized = (Vector3.zero - dice.transform.position).normalized;
			dice.Throw(normalized);
		}
	}

	public void Throw(Vector2 direction)
	{
		rb.drag = drag.Random();
		rb.angularDrag = drag.Random();
		rb.velocity = direction * throwSpeed.Random();
		rb.angularVelocity = rb.velocity.magnitude * spinAmount.Random();
		Roll();
	}

	public void DisableCollisions()
	{
		rb.simulated = false;
	}
}
