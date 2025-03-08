using UnityEngine;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class CampaignNodeTypeBuilder : DataFileBuilder<CampaignNodeType, CampaignNodeTypeBuilder>
	{
		public CampaignNodeTypeBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public CampaignNodeTypeBuilder()
		{
		}

		public CampaignNodeTypeBuilder WithLetter(string letter)
		{
			_data.letter = letter;
			return this;
		}

		public CampaignNodeTypeBuilder WithZoneName(string zoneName)
		{
			_data.zoneName = zoneName;
			return this;
		}

		public CampaignNodeTypeBuilder WithMustClear(bool mustClear)
		{
			_data.mustClear = mustClear;
			return this;
		}

		public CampaignNodeTypeBuilder WithCanSkip(bool canSkip)
		{
			_data.canSkip = canSkip;
			return this;
		}

		public CampaignNodeTypeBuilder WithCanEnter(bool canEnter)
		{
			_data.canEnter = canEnter;
			return this;
		}

		public CampaignNodeTypeBuilder WithIsBattle(bool isBattle)
		{
			_data.isBattle = isBattle;
			return this;
		}

		public CampaignNodeTypeBuilder WithIsBoss(bool isBoss)
		{
			_data.isBoss = isBoss;
			return this;
		}

		public CampaignNodeTypeBuilder WithModifierReward(bool modifierReward)
		{
			_data.modifierReward = modifierReward;
			return this;
		}

		public CampaignNodeTypeBuilder WithInteractable(bool interactable)
		{
			_data.interactable = interactable;
			return this;
		}

		public CampaignNodeTypeBuilder WithStartRevealed(bool startRevealed)
		{
			_data.startRevealed = startRevealed;
			return this;
		}

		public CampaignNodeTypeBuilder WithFinalNode(bool finalNode)
		{
			_data.finalNode = finalNode;
			return this;
		}

		public CampaignNodeTypeBuilder WithMapNodePrefab(MapNode mapNodePrefab)
		{
			_data.mapNodePrefab = mapNodePrefab;
			return this;
		}

		public CampaignNodeTypeBuilder WithMapNodeSprite(Sprite mapNodeSprite)
		{
			_data.mapNodeSprite = mapNodeSprite;
			return this;
		}

		public CampaignNodeTypeBuilder WithMapNodeSprite(string mapNodeSprite)
		{
			_data.mapNodeSprite = Mod.GetImageSprite(mapNodeSprite);
			return this;
		}

		public CampaignNodeTypeBuilder WithSize(float size = 1f)
		{
			_data.size = size;
			return this;
		}

		public CampaignNodeTypeBuilder WithCanLink(bool canLink)
		{
			_data.canLink = canLink;
			return this;
		}
	}
}
