#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportCards : MonoBehaviour
{
	[SerializeField]
	public string folder = "ExportCards";

	public Camera _camera;

	public Camera camera => _camera ?? (_camera = Camera.main);

	public IEnumerator Start()
	{
		yield return new WaitUntil(() => CardManager.init);
		List<CardData> group = AddressableLoader.GetGroup<CardData>("CardData");
		foreach (CardData cardData in group)
		{
			if (!(cardData.mainSprite == null) && !(cardData.mainSprite.name == "Nothing"))
			{
				Card card = CardManager.Get(cardData, null, null, inPlay: false, isPlayerCard: false);
				yield return card.UpdateData();
				card.transform.position = Vector3.zero;
				yield return null;
				Screenshot(Application.dataPath + "/../" + folder + "/" + cardData.cardType.name, card.titleText.text + " (" + card.name + ").png");
				yield return null;
				CardManager.ReturnToPool(card);
			}
		}
	}

	public void Screenshot(string directory, string fileName)
	{
		string text = directory + "/" + fileName;
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, mipChain: false);
		RenderTexture renderTexture = new RenderTexture(texture2D.width, texture2D.height, 24);
		camera.targetTexture = renderTexture;
		camera.Render();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, texture2D.width, texture2D.height), 0, 0);
		texture2D.Apply();
		byte[] bytes = texture2D.EncodeToPNG();
		Directory.CreateDirectory(directory);
		File.WriteAllBytes(text, bytes);
		Debug.Log(text);
	}
}
