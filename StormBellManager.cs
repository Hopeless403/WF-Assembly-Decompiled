#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class StormBellManager : MonoBehaviour
{
	public const int TrueFinalBossPointThreshold = 10;

	public const int PointLimit = 10;

	public const int MaxPointStart = 5;

	public const int MaxPointIncrease = 1;

	[SerializeField]
	public GameObject openButton;

	[SerializeField]
	public Transform[] bellGroups;

	[SerializeField]
	public ModifierIcon bellPrefab;

	[SerializeField]
	public ModifierIconMultiple[] stormBellDisplays;

	[SerializeField]
	public GameObject stormBellUnlockDisplay;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public GameObject overcranker;

	[SerializeField]
	public GameObject pointLimitDisplay;

	[SerializeField]
	public GameObject overcrankDisplay;

	[SerializeField]
	public GameObject overcrankUnlockDisplay;

	[SerializeField]
	public ColourFader backgroundFader;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString stormStrengthAddString;

	[SerializeField]
	public GameObject additionalButtons;

	[Header("Smoke")]
	[SerializeField]
	public ParticleSystem normalSmoke;

	[SerializeField]
	public ParticleSystem overcrankSmoke;

	[Header("Storm Strength Increase Popup")]
	[SerializeField]
	public GameObject stormLimitAdd;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString stormLimitAddString;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString goldFramesRemainingString;

	[SerializeField]
	public TMP_Text stormLimitAddText;

	[Header("SFX")]
	[SerializeField]
	public EventReference stormBellActivateSfx;

	[SerializeField]
	public EventReference stormBellActivate10Sfx;

	[SerializeField]
	public EventReference stormBellRandomizeSfx;

	[SerializeField]
	public EventReference stormBellClearSfx;

	public List<string> activeNames;

	public List<string> golden;

	public List<string> newGolden;

	public List<HardModeModifierData> active;

	public readonly Dictionary<ModifierIcon, HardModeModifierData> modifierIcons = new Dictionary<ModifierIcon, HardModeModifierData>();

	public bool unlocked;

	public int points;

	public int maxPoints;

	public bool overcrank;

	public string stormStrengthAddText;

	public EventInstance bellActivateSfxInstance;

	public EventInstance bellActivate10SfxInstance;

	public EventInstance bellRandomizeSfxInstance;

	public void Awake()
	{
		overcrank = SaveSystem.LoadProgressData("stormPointOvercrank", defaultValue: false);
		maxPoints = Mathf.Min(SaveSystem.LoadProgressData("maxStormPoints", 5), 10);
		unlocked = SaveSystem.LoadProgressData("stormBellsUnlocked", defaultValue: false);
		if (!unlocked && SaveSystem.LoadProgressData("hardModeModifiersUnlocked", 0) > 0)
		{
			unlocked = true;
			SaveSystem.SaveProgressData("stormBellsUnlocked", value: true);
		}

		if (!unlocked)
		{
			openButton.SetActive(value: false);
			return;
		}

		golden = SaveSystem.LoadProgressData<List<string>>("goldHardModeModifiers") ?? new List<string>();
		newGolden = SaveSystem.LoadProgressData<List<string>>("goldHardModeModifiersNew");
		if (!SaveSystem.LoadProgressData("stormBellsUnlockedScreenShown", defaultValue: false))
		{
			stormBellUnlockDisplay.SetActive(value: true);
			SaveSystem.SaveProgressData("stormBellsUnlockedScreenShown", value: true);
		}

		List<string> list = newGolden;
		if (list != null && list.Count > 0)
		{
			int num = MonoBehaviourSingleton<References>.instance.hardModeModifiers.Count((HardModeModifierData a) => golden.Contains(a.name));
			if (num > 0)
			{
				stormLimitAdd.SetActive(value: true);
				stormLimitAddText.text = goldFramesRemainingString.GetLocalizedString(num);
			}
		}

		int num2 = SaveSystem.LoadProgressData("maxStormPointIncrease", 0);
		if (num2 > 0)
		{
			stormLimitAdd.SetActive(value: true);
			stormLimitAddText.text = stormLimitAddString.GetLocalizedString(num2);
			SaveSystem.SaveProgressData("maxStormPointIncrease", 0);
		}

		activeNames = SaveSystem.LoadProgressData<List<string>>("activeHardModeModifiers") ?? new List<string>();
		bool flag = false;
		for (int num3 = activeNames.Count - 1; num3 >= 0; num3--)
		{
			string bellName = activeNames[num3];
			if (MonoBehaviourSingleton<References>.instance.hardModeModifiers.All((HardModeModifierData a) => a.name != bellName))
			{
				activeNames.RemoveAt(num3);
				flag = true;
			}
		}

		if (active == null)
		{
			active = new List<HardModeModifierData>();
		}

		HardModeModifierData[] hardModeModifiers = MonoBehaviourSingleton<References>.instance.hardModeModifiers;
		foreach (HardModeModifierData hardModeModifierData in hardModeModifiers)
		{
			if (activeNames.Contains(hardModeModifierData.name))
			{
				active.Add(hardModeModifierData);
			}
		}

		if (!overcrank)
		{
			int num4 = 0;
			for (int num5 = active.Count - 1; num5 >= 0; num5--)
			{
				HardModeModifierData hardModeModifierData2 = active[num5];
				if (num4 + hardModeModifierData2.stormPoints > maxPoints)
				{
					active.RemoveAt(num5);
					activeNames.Remove(hardModeModifierData2.name);
					flag = true;
				}
				else
				{
					num4 += hardModeModifierData2.stormPoints;
				}
			}
		}

		if (flag)
		{
			SaveSystem.SaveProgressData("activeHardModeModifiers", activeNames);
		}

		stormStrengthAddText = stormStrengthAddString.GetLocalizedString();
		ModifierIcon modifierIcon = null;
		for (int j = 0; j < MonoBehaviourSingleton<References>.instance.hardModeModifiers.Length; j++)
		{
			HardModeModifierData modifier = MonoBehaviourSingleton<References>.instance.hardModeModifiers[j];
			ModifierIcon modifierIcon2 = CreateBell(modifier, j);
			if (modifierIcon != null && j != 5 && j != 10)
			{
				ConnectBellInputLeft(modifierIcon2, modifierIcon);
				ConnectBellInputRight(modifierIcon, modifierIcon2);
			}

			modifierIcon = modifierIcon2;
		}

		UpdateBells();
		if (overcrank)
		{
			foreach (KeyValuePair<ModifierIcon, HardModeModifierData> modifierIcon3 in modifierIcons)
			{
				modifierIcon3.Key.GetComponent<ModifierToggle>().canToggle = false;
			}
		}

		if (overcrank)
		{
			overcrankSmoke.Play();
		}
		else if (points >= 10)
		{
			normalSmoke.Play();
		}

		additionalButtons.SetActive(!overcrank);
		pointLimitDisplay.SetActive(!overcrank);
		overcrankDisplay.SetActive(overcrank);
		overcranker.SetActive(!MonoBehaviourSingleton<References>.instance.hardModeModifiers.Select((HardModeModifierData a) => a.name).Except(golden).Any());
		if (overcranker.activeSelf && !SaveSystem.LoadProgressData("overcrankUnlockedScreenShown", defaultValue: false))
		{
			overcrankUnlockDisplay.SetActive(value: true);
			SaveSystem.SaveProgressData("overcrankUnlockedScreenShown", value: true);
		}

		SetGlobalStormBellCountParam();
	}

	public void OnEnable()
	{
		UpdatePointsDisplays();
	}

	public void CheckStartGoldFrameRoutine()
	{
		foreach (KeyValuePair<ModifierIcon, HardModeModifierData> modifierIcon in modifierIcons)
		{
			if (golden.Contains(modifierIcon.Value.name) && (newGolden == null || !newGolden.Contains(modifierIcon.Value.name)))
			{
				modifierIcon.Key.GetComponent<UnityEngine.Animator>()?.SetBool("Gold", value: true);
			}
		}

		List<string> list = newGolden;
		if (list != null && list.Count > 0)
		{
			string[] modifierNames = newGolden.ToArray();
			newGolden = null;
			SaveSystem.DeleteProgressData("goldHardModeModifiersNew");
			StartCoroutine(GoldFrameRoutine(modifierNames));
		}
	}

	public void UpdateBells()
	{
		foreach (KeyValuePair<ModifierIcon, HardModeModifierData> modifierIcon in modifierIcons)
		{
			ModifierToggle component = modifierIcon.Key.GetComponent<ModifierToggle>();
			if ((object)component != null && !component.IsActive)
			{
				component.canToggle = maxPoints - points - modifierIcon.Value.stormPoints >= 0;
				component.UpdateArt();
			}
		}
	}

	public ModifierIcon CreateBell(HardModeModifierData modifier, int index)
	{
		ModifierIcon modifierIcon = Object.Instantiate(bellPrefab, bellGroups[index % bellGroups.Length]);
		modifierIcon.Set(modifier.modifierData, Vector2.left);
		modifierIcon.AddText(stormStrengthAddText.Format(modifier.stormPoints));
		ModifierToggle component = modifierIcon.GetComponent<ModifierToggle>();
		if (activeNames.Contains(modifier.name))
		{
			points += modifier.stormPoints;
			ModifierSystem.AddModifier(Campaign.Data, modifier.modifierData);
		}
		else if ((bool)component)
		{
			component.Toggle();
		}

		if ((bool)component)
		{
			component.onToggle.AddListener(delegate
			{
				Toggle(modifier);
			});
		}

		modifierIcons.Add(modifierIcon, modifier);
		return modifierIcon;
	}

	public static void ConnectBellInputLeft(ModifierIcon from, ModifierIcon to)
	{
		UINavigationItem component = from.GetComponent<UINavigationItem>();
		if ((object)component != null)
		{
			UINavigationItem component2 = to.GetComponent<UINavigationItem>();
			if ((object)component2 != null)
			{
				component.overrideInputs = true;
				component.inputLeft = component2;
			}
		}
	}

	public static void ConnectBellInputRight(ModifierIcon from, ModifierIcon to)
	{
		UINavigationItem component = from.GetComponent<UINavigationItem>();
		if ((object)component != null)
		{
			UINavigationItem component2 = to.GetComponent<UINavigationItem>();
			if ((object)component2 != null)
			{
				component.overrideInputs = true;
				component.inputRight = component2;
			}
		}
	}

	public void Toggle(HardModeModifierData modifier)
	{
		int num = points;
		ToggleEffect(modifier);
		UpdatePointsDisplays();
		UpdateBells();
		SaveSystem.SaveProgressData("activeHardModeModifiers", activeNames);
		animator.SetBool("Angry", points >= 10);
		animator.SetTrigger("Blip");
		SetGlobalStormBellCountParam();
		if (activeNames.Contains(modifier.name))
		{
			bellActivateSfxInstance = SfxSystem.OneShot(stormBellActivateSfx);
			SfxSystem.SetParam(bellActivateSfxInstance, "count", activeNames.Count);
		}

		if (num < 10 && points >= 10)
		{
			backgroundFader.FadeToColour("Purple");
			bellActivate10SfxInstance = SfxSystem.OneShot(stormBellActivate10Sfx);
			normalSmoke.Play();
		}
		else if (num >= 10 && points < 10)
		{
			backgroundFader.FadeToColour("Normal");
			normalSmoke.Stop();
		}
	}

	public void SetGlobalStormBellCountParam()
	{
		int num = ((points >= 10) ? Mathf.Max(10, active.Count) : active.Count);
		SfxSystem.SetGlobalParam("stormbell_count", num);
	}

	public void ToggleEffect(HardModeModifierData modifier)
	{
		if (activeNames.Contains(modifier.name))
		{
			activeNames.Remove(modifier.name);
			active.Remove(modifier);
			points -= modifier.stormPoints;
			ModifierSystem.RemoveModifier(Campaign.Data, modifier.modifierData);
		}
		else
		{
			activeNames.Add(modifier.name);
			active.Add(modifier);
			points += modifier.stormPoints;
			ModifierSystem.AddModifier(Campaign.Data, modifier.modifierData);
		}
	}

	public void ForceEnableAllBells()
	{
		foreach (KeyValuePair<ModifierIcon, HardModeModifierData> modifierIcon in modifierIcons)
		{
			if (!activeNames.Contains(modifierIcon.Value.name))
			{
				ToggleEffect(modifierIcon.Value);
				modifierIcon.Key.Ding();
				modifierIcon.Key.GetComponent<ModifierToggle>().SetActive(value: true);
			}
		}

		UpdatePointsDisplays();
		UpdateBells();
		foreach (KeyValuePair<ModifierIcon, HardModeModifierData> modifierIcon2 in modifierIcons)
		{
			modifierIcon2.Key.GetComponent<ModifierToggle>().canToggle = false;
		}

		SaveSystem.SaveProgressData("activeHardModeModifiers", activeNames);
		animator.SetBool("Angry", points >= 10);
		animator.SetTrigger("Blip");
	}

	public void DisableForcedBells()
	{
		List<string> list = SaveSystem.LoadProgressData<List<string>>("activeHardModeModifiersBeforeOvercrank") ?? new List<string>();
		foreach (KeyValuePair<ModifierIcon, HardModeModifierData> modifierIcon in modifierIcons)
		{
			if (!list.Contains(modifierIcon.Value.name))
			{
				ToggleEffect(modifierIcon.Value);
				modifierIcon.Key.GetComponent<ModifierToggle>().SetActive(value: false);
			}
		}

		UpdatePointsDisplays();
		UpdateBells();
		foreach (KeyValuePair<ModifierIcon, HardModeModifierData> modifierIcon2 in modifierIcons)
		{
			modifierIcon2.Key.GetComponent<ModifierToggle>().canToggle = activeNames.Contains(modifierIcon2.Value.name) || points + modifierIcon2.Value.stormPoints <= maxPoints;
		}

		SaveSystem.SaveProgressData("activeHardModeModifiers", activeNames);
		animator.SetBool("Angry", points >= 10);
		animator.SetTrigger("Blip");
	}

	public void UpdatePointsDisplays()
	{
		ModifierIconMultiple[] array = stormBellDisplays;
		foreach (ModifierIconMultiple modifierIconMultiple in array)
		{
			modifierIconMultiple.Clear();
			if (active == null)
			{
				continue;
			}

			foreach (HardModeModifierData item in active)
			{
				if ((bool)item)
				{
					modifierIconMultiple.Add(item.modifierData);
				}
			}
		}
	}

	public static List<string> GetActiveStormBells()
	{
		return SaveSystem.LoadProgressData<List<string>>("activeHardModeModifiers");
	}

	public static int GetCurrentStormPoints(List<string> active)
	{
		int num = 0;
		if (active != null)
		{
			HardModeModifierData[] hardModeModifiers = MonoBehaviourSingleton<References>.instance.hardModeModifiers;
			foreach (HardModeModifierData hardModeModifierData in hardModeModifiers)
			{
				if (active.Contains(hardModeModifierData.name))
				{
					num += hardModeModifierData.stormPoints;
				}
			}
		}

		return num;
	}

	public void UpdateAnimator()
	{
		animator.SetBool("Angry", points >= 10);
		animator.SetBool("Overcrank", overcrank);
		backgroundFader.FadeToColour(overcrank ? "Red" : ((points >= 10) ? "Purple" : "Normal"));
		if (overcrank)
		{
			overcrankSmoke.Play();
			normalSmoke.Stop();
			return;
		}

		overcrankSmoke.Stop();
		if (points >= 10)
		{
			normalSmoke.Play();
		}
		else
		{
			normalSmoke.Stop();
		}
	}

	public void ToggleOvercrank()
	{
		overcrank = !overcrank;
		animator.SetBool("Overcrank", overcrank);
		SaveSystem.SaveProgressData("stormPointOvercrank", overcrank);
		if (overcrank)
		{
			SaveSystem.SaveProgressData("activeHardModeModifiersBeforeOvercrank", activeNames);
			ForceEnableAllBells();
			backgroundFader.FadeToColour("Red");
			SfxSystem.OneShot("event:/sfx/town/stormbell_overcrank");
			overcrankSmoke.Play();
			normalSmoke.Stop();
		}
		else
		{
			DisableForcedBells();
			backgroundFader.FadeToColour((points >= 10) ? "Purple" : "Normal");
			SfxSystem.OneShot("event:/sfx/town/stormbell_overcrank_deactivate");
			overcrankSmoke.Stop();
			if (points >= 10)
			{
				normalSmoke.Play();
			}
			else
			{
				normalSmoke.Stop();
			}
		}

		pointLimitDisplay.SetActive(!overcrank);
		overcrankDisplay.SetActive(overcrank);
		additionalButtons.SetActive(!overcrank);
		SetGlobalStormBellCountParam();
	}

	public static bool TrueFinalBossPointThresholdReached()
	{
		return GetCurrentStormPoints(GetActiveStormBells()) >= 10;
	}

	public IEnumerator GoldFrameRoutine(string[] modifierNames)
	{
		foreach (string modifierName in modifierNames)
		{
			KeyValuePair<ModifierIcon, HardModeModifierData> keyValuePair = modifierIcons.FirstOrDefault((KeyValuePair<ModifierIcon, HardModeModifierData> p) => p.Value.name == modifierName);
			if ((bool)keyValuePair.Key)
			{
				UnityEngine.Animator component = keyValuePair.Key.GetComponent<UnityEngine.Animator>();
				if ((object)component != null)
				{
					component.SetBool("Gold", value: true);
					component.SetBool("BecomeGold", value: true);
					yield return new WaitForSeconds(0.15f);
				}
			}
		}
	}

	public void Randomize()
	{
		Clear();
		int num = 0;
		List<HardModeModifierData> list = new List<HardModeModifierData>();
		while (num != maxPoints)
		{
			list.Clear();
			num = 0;
			List<HardModeModifierData> list2 = MonoBehaviourSingleton<References>.instance.hardModeModifiers.ToList();
			while (num < maxPoints && list2.Count > 0)
			{
				HardModeModifierData hardModeModifierData = list2.TakeRandom();
				if (num + hardModeModifierData.stormPoints <= maxPoints)
				{
					list.Add(hardModeModifierData);
					num += hardModeModifierData.stormPoints;
				}
			}
		}

		foreach (HardModeModifierData modifier in list)
		{
			ToggleEffect(modifier);
			ModifierIcon key = modifierIcons.FirstOrDefault((KeyValuePair<ModifierIcon, HardModeModifierData> p) => p.Value == modifier).Key;
			if ((bool)key)
			{
				key.Ding();
				key.GetComponent<ModifierToggle>().SetActive(value: true);
			}
		}

		SaveSystem.SaveProgressData("activeHardModeModifiers", activeNames);
		UpdatePointsDisplays();
		UpdateBells();
		UpdateAnimator();
		SetGlobalStormBellCountParam();
		SfxSystem.Stop(bellRandomizeSfxInstance);
		bellRandomizeSfxInstance = SfxSystem.OneShot(stormBellRandomizeSfx);
	}

	public void Clear()
	{
		foreach (KeyValuePair<ModifierIcon, HardModeModifierData> modifierIcon in modifierIcons)
		{
			if (activeNames.Contains(modifierIcon.Value.name))
			{
				modifierIcon.Key.GetComponent<ModifierToggle>().Toggle();
			}
		}
	}

	public void ClearAndPlaySfx()
	{
		if (active.Count > 0)
		{
			SfxSystem.OneShot(stormBellClearSfx);
			SfxSystem.Stop(bellRandomizeSfxInstance);
			SfxSystem.Stop(bellActivateSfxInstance);
			SfxSystem.Stop(bellActivate10SfxInstance);
			Clear();
		}
	}
}
