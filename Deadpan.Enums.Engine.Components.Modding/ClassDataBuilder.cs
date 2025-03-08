using FMODUnity;
using UnityEngine;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class ClassDataBuilder : DataFileBuilder<ClassData, ClassDataBuilder>
	{
		public ClassDataBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public ClassDataBuilder()
		{
		}

		public ClassDataBuilder WithRequiresUnlock(UnlockData requiresUnlock)
		{
			_data.requiresUnlock = requiresUnlock;
			return this;
		}

		public ClassDataBuilder WithStartingInventory(Inventory startingInventory)
		{
			_data.startingInventory = startingInventory;
			return this;
		}

		public ClassDataBuilder WithLeaders(params CardData[] leaders)
		{
			_data.leaders = leaders;
			return this;
		}

		public ClassDataBuilder WithCharacterPrefab(Character characterPrefab)
		{
			_data.characterPrefab = characterPrefab;
			return this;
		}

		public ClassDataBuilder WithRewardPools(params RewardPool[] rewardPools)
		{
			_data.rewardPools = rewardPools;
			return this;
		}

		public ClassDataBuilder WithSelectSfxEvent(EventReference selectSfxEvent)
		{
			_data.selectSfxEvent = selectSfxEvent;
			return this;
		}

		public ClassDataBuilder WithFlag(Sprite flag)
		{
			_data.flag = flag;
			return this;
		}

		public ClassDataBuilder WithFlag(string flag)
		{
			_data.flag = Mod.GetImageSprite(flag);
			return this;
		}
	}
}
