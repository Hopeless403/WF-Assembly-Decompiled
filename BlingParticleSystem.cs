#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using Dead;
using UnityEngine;

public class BlingParticleSystem : GameSystem
{
	[SerializeField]
	public BlingParticle blingPrefab;

	[SerializeField]
	public int initialPoolSize = 20;

	[SerializeField]
	public Sprite lowValueSprite;

	[SerializeField]
	public Sprite highValueSprite;

	[SerializeField]
	public int highValueAmount = 5;

	public readonly Queue<BlingParticle> pool = new Queue<BlingParticle>();

	public void Start()
	{
		for (int i = 0; i < initialPoolSize; i++)
		{
			Pool(Object.Instantiate(blingPrefab, base.transform));
		}
	}

	public void OnEnable()
	{
		Events.OnDropGold += DropGold;
	}

	public void OnDisable()
	{
		Events.OnDropGold -= DropGold;
	}

	public void DropGold(int amount, string source, Character owner, Vector3 position)
	{
		int num = PettyRandom.Range(3, 5);
		owner.data.inventory.goldOwed += amount;
		while (amount > 0)
		{
			BlingParticle blingParticle = Get();
			blingParticle.transform.position = position;
			blingParticle.gameObject.SetActive(value: true);
			blingParticle.owner = owner;
			if (num > 0 || amount < highValueAmount)
			{
				blingParticle.value = 1;
				blingParticle.sprite = lowValueSprite;
				num--;
			}
			else
			{
				blingParticle.value = highValueAmount;
				blingParticle.sprite = highValueSprite;
			}

			amount -= blingParticle.value;
		}
	}

	public BlingParticle Get()
	{
		if (pool.Count > 0)
		{
			return pool.Dequeue();
		}

		return Object.Instantiate(blingPrefab, base.transform);
	}

	public void Pool(BlingParticle particle)
	{
		particle.gameObject.SetActive(value: false);
		pool.Enqueue(particle);
	}
}
