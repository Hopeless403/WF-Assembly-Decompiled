#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class BattleLogEntry : MonoBehaviour
{
	[SerializeField]
	public TMP_Text textElement;

	public const string colorFriendly = "5E849A";

	public const string colorEnemy = "804248";

	public const string colorNumber = "EE5D46";

	public async Task SetUp(BattleLog log)
	{
		string text = AddInserts(await log.textKey.GetLocalizedStringAsync().Task, log.args);
		text = FormatLogText(text);
		textElement.text = text;
	}

	public static string AddInserts(string text, object[] inserts)
	{
		if (inserts != null && inserts.Length != 0)
		{
			object[] array = new object[inserts.Length];
			for (int i = 0; i < inserts.Length; i++)
			{
				object obj = inserts[i];
				if (obj is BattleEntity entity)
				{
					string entityName = GetEntityName(entity);
					string text2 = (entity.friendly ? "5E849A" : "804248");
					array[i] = "<#" + text2 + ">" + entityName + "</color>";
				}
				else
				{
					array[i] = obj;
				}
			}

			text = string.Format(text, array);
		}

		return text;
	}

	public static string GetEntityName(BattleEntity entity)
	{
		string text = entity.forceTitle;
		if (text.IsNullOrWhitespace())
		{
			text = entity.titleKey.GetLocalizedString();
		}

		return text;
	}

	public static string FormatLogText(string text)
	{
		int num = text.IndexOf('[');
		if (num >= 0)
		{
			int num2 = text.IndexOf(']');
			if (num2 >= num + 2)
			{
				string text2 = text.Substring(num + 1, num2 - num - 1);
				text = text.Remove(num, num2 - num + 1);
				text = text.Insert(num, "<#EE5D46>" + text2 + "</color>");
				text = FormatLogText(text);
			}
		}

		return text;
	}
}
