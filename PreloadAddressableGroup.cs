#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class PreloadAddressableGroup : MonoBehaviour
{
	[SerializeField]
	public string[] groups;

	public static bool done;

	public async void Start()
	{
		if (done)
		{
			Object.Destroy(base.gameObject);
			return;
		}

		done = true;
		Object.DontDestroyOnLoad(base.gameObject);
		Debug.Log($"~ ASSET PRELOADER: {groups.Length} group(s) to load");
		string[] array = groups;
		foreach (string group in array)
		{
			Debug.Log("~ ASSET PRELOADER: Loading Group [" + group + "]");
			StopWatch.Start();
			await AddressableLoader.PreLoadGroup(group);
			Debug.Log($"~ ASSET PRELOADER: Group [{group}] Loaded! ({StopWatch.Stop()}ms)");
		}

		Debug.Log("~ ASSET PRELOADER: Finished");
		Object.Destroy(base.gameObject);
	}
}
