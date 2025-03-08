#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class MapNodeSpriteSetterItem : MapNodeSpriteSetter
{
	[SerializeField]
	public Sprite normalSprite;

	[SerializeField]
	public Sprite bigSprite;

	[SerializeField]
	public Sprite clearedSprite;

	public override void Set(MapNode mapNode)
	{
		object value;
		if (mapNode.campaignNode.cleared)
		{
			mapNode.SetSprite(clearedSprite);
		}
		else if (mapNode.campaignNode.data.TryGetValue("cards", out value) && value is SaveCollection<string> saveCollection && saveCollection.Count > 3)
		{
			mapNode.SetSprite(bigSprite);
		}
		else
		{
			mapNode.SetSprite(normalSprite);
		}
	}
}
