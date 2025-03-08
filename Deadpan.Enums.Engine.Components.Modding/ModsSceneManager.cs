using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public class ModsSceneManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject Content;

		[SerializeField]
		private GameObject ModPrefab;

		private List<WildfrostMod> instantiatedMods = new List<WildfrostMod>();

		private IEnumerator Start()
		{
			foreach (WildfrostMod mod in Bootstrap.Mods)
			{
				if (!instantiatedMods.Contains(mod))
				{
					GameObject gameObject = ModPrefab.InstantiateKeepName();
					gameObject.transform.SetParent(Content.transform);
					gameObject.transform.SetLocalZ(0f);
					gameObject.transform.SetLocalPositionAndRotation(gameObject.transform.localPosition, Quaternion.identity);
					ModHolder componentInChildren = gameObject.GetComponentInChildren<ModHolder>();
					componentInChildren.Mod = mod;
					componentInChildren.UpdateInfo();
					instantiatedMods.Add(mod);
				}
			}
			Time.timeScale = 1f;
			yield break;
		}
	}
}
