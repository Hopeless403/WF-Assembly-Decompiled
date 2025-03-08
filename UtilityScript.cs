#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public static class UtilityScript
{
	public static string GetLogMessage(string source, string message)
	{
		return "[" + source + "] " + message;
	}

	public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
	{
		if (expression.Body is MethodCallExpression methodCallExpression)
		{
			return methodCallExpression.Method;
		}

		throw new ArgumentException("Expression is not a method", "expression");
	}

	public static Vector3 GenerateSineWave(Vector3 axis, Vector3 currentPosition, float frequency, float magnitude, float timeStep)
	{
		return currentPosition + axis * Mathf.Sin(timeStep * frequency) * magnitude;
	}

	public static float PlanarDistance(this Vector3 a, Vector3 b, Axis negatedAxis)
	{
		if (negatedAxis == Axis.All)
		{
			return new Vector3(0f, 0f, a.z).Distance(new Vector3(0f, 0f, b.z));
		}

		if (negatedAxis == Axis.All)
		{
			return new Vector3(0f, a.y, 0f).Distance(new Vector3(0f, b.y, 0f));
		}

		if (negatedAxis == Axis.All)
		{
			return new Vector3(a.x, 0f, 0f).Distance(new Vector3(b.x, 0f, 0f));
		}

		switch (negatedAxis)
		{
			case Axis.X:
				return new Vector3(0f, a.y, a.z).Distance(new Vector3(0f, b.y, b.z));
			case Axis.Y:
				return new Vector3(a.x, 0f, a.z).Distance(new Vector3(b.x, 0f, b.z));
			case Axis.Z:
				return new Vector3(a.x, a.y, 0f).Distance(new Vector3(b.x, b.y, 0f));
			case Axis.All:
				return Vector3.zero.Distance(Vector3.zero);
			default:
				return float.PositiveInfinity;
		}
	}

	public static Vector2 GetAverage(List<Vector2> positions)
	{
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < positions.Count; i++)
		{
			zero += positions[i];
		}

		return zero / positions.Count;
	}

	public static Vector3 RandomInRange(this Vector3 inVector)
	{
		return new Vector3(UnityEngine.Random.Range((0f - inVector.x) / 2f, inVector.x / 2f), UnityEngine.Random.Range((0f - inVector.y) / 2f, inVector.y / 2f), UnityEngine.Random.Range((0f - inVector.z) / 2f, inVector.z / 2f));
	}

	public static Vector3 PlanarDistanceVec3(this Vector3 a, Vector3 b, Axis negatedAxis)
	{
		if (negatedAxis == Axis.All)
		{
			return new Vector3(0f, 0f, a.z) - new Vector3(0f, 0f, b.z);
		}

		if (negatedAxis == Axis.All)
		{
			return new Vector3(0f, a.y, 0f) - new Vector3(0f, b.y, 0f);
		}

		if (negatedAxis == Axis.All)
		{
			return new Vector3(a.x, 0f, 0f) - new Vector3(b.x, 0f, 0f);
		}

		switch (negatedAxis)
		{
			case Axis.X:
				return new Vector3(0f, a.y, a.z) - new Vector3(0f, b.y, b.z);
			case Axis.Y:
				return new Vector3(a.x, 0f, a.z) - new Vector3(b.x, 0f, b.z);
			case Axis.Z:
				return new Vector3(a.x, a.y, 0f) - new Vector3(b.x, b.y, 0f);
			case Axis.All:
				return Vector3.zero - Vector3.zero;
			default:
				return Vector3.positiveInfinity;
		}
	}

	public static Vector3 GetPlanar(this Vector3 a, Axis negatedAxis)
	{
		switch (negatedAxis)
		{
			case Axis.X:
				return new Vector3(0f, a.y, a.z);
			case Axis.Y:
				return new Vector3(a.x, 0f, a.z);
			case Axis.Z:
				return new Vector3(a.x, a.y, 0f);
			default:
				return new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}
	}

	public static float RoundAwayFromZero(this float f)
	{
		if (!(f >= 0f))
		{
			return Mathf.Floor(f);
		}

		return Mathf.Ceil(f);
	}

	public static float AddToAverage(float average, int size, float value)
	{
		return ((float)size * average + value) / (float)(size + 1);
	}

	public static float SubtractFromAverage(float average, int size, float value)
	{
		return ((float)size * average - value) / (float)(size - 1);
	}

	public static float ReplaceInAverage(float average, int size, float oldValue, float newValue)
	{
		return ((float)size * average - oldValue + newValue) / (float)size;
	}

	public static float AddAveragesTogether(float averageA, int sizeA, float averageB, int sizeB)
	{
		return ((float)sizeA * averageA + (float)sizeB * averageB) / (float)(sizeA + sizeB);
	}

	public static float GetNormalDistance(this Vector3 position, Vector3 startPosition, Vector3 destination)
	{
		float sqrMagnitude = startPosition.PlanarDistanceVec3(destination, Axis.Y).sqrMagnitude;
		return position.PlanarDistanceVec3(destination, Axis.Y).sqrMagnitude.RemapProportion(0f, sqrMagnitude, 0f, 1f);
	}

	public static string GetPathBasedOnOS()
	{
		if (Application.isEditor)
		{
			return Application.persistentDataPath + "/";
		}

		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			return Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/") + "/";
		}

		if (Application.isMobilePlatform || Application.isConsolePlatform)
		{
			return Application.persistentDataPath;
		}

		return Application.persistentDataPath + "/";
	}

	public static string GetIdentifier(string userIdentifier, string uniqueObjectIdentifier)
	{
		return userIdentifier + "-" + uniqueObjectIdentifier;
	}

	public static Vector3 Lerp(this Vector3 a, Vector3 b, float time)
	{
		return Vector3.Lerp(a, b, time);
	}

	public static bool IsEmpty(this string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return string.IsNullOrWhiteSpace(input);
		}

		return false;
	}

	public static double ToEpoch(this DateTime dt)
	{
		return dt.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
	}

	public static DateTime FromEpoch(this double epoch)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch);
	}

	public static double ToEpochMs(this DateTime dt)
	{
		return dt.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
	}

	public static DateTime FromEpochMs(this double epoch)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(epoch);
	}

	public static float SecondsToMs(this float seconds)
	{
		return seconds * 1000f;
	}

	public static Texture2D AddWatermark(Texture2D textureToSave, Texture2D watermark, int startPositionX, int startPositionY, bool hideFadedPixels = false)
	{
		Texture2D texture2D = textureToSave.ToTexture2D();
		for (int i = startPositionX; i < texture2D.width; i++)
		{
			for (int j = startPositionY; j < texture2D.height; j++)
			{
				if (i - startPositionX < watermark.width && j - startPositionY < watermark.height)
				{
					Color pixel = texture2D.GetPixel(i, j);
					Color pixel2 = watermark.GetPixel(i - startPositionX, j - startPositionY);
					Color color = Color.Lerp(pixel, pixel2, pixel2.a / 1f);
					Color color2 = new Color(color.r, color.g, color.b, 1f);
					texture2D.SetPixel(i, j, color2);
				}
			}
		}

		texture2D.Apply();
		return texture2D;
	}

	public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		float num = Vector3.Dot(Vector3.Cross(fwd, targetDir), up);
		if (num > 0f)
		{
			return 1f;
		}

		if (num < 0f)
		{
			return -1f;
		}

		return 0f;
	}

	public static IEnumerable<string> ToCsv<T>(IEnumerable<T> objectlist, string separator = ",", bool header = true)
	{
		FieldInfo[] fields = typeof(T).GetFields();
		PropertyInfo[] properties = typeof(T).GetProperties();
		if (header)
		{
			yield return string.Join(separator, fields.Select((FieldInfo f) => f.Name).Concat(properties.Select((PropertyInfo p) => p.Name)).ToArray());
		}

		foreach (T o in objectlist)
		{
			yield return string.Join(separator, fields.Select((FieldInfo f) => (f.GetValue(o) ?? "").ToString()).Concat(properties.Select((PropertyInfo p) => (p.GetValue(o, null) ?? "").ToString())).ToArray());
		}
	}

	public static float SubtractValueFromMean(float inVal, float currentMean, float numOfValues)
	{
		return (currentMean * numOfValues - inVal) / (numOfValues - 1f);
	}

	public static float AddValueToMean(float inVal, float currentMean, float numOfValues)
	{
		return currentMean + (inVal - currentMean) / numOfValues;
	}

	public static Color RandomColour()
	{
		return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
	}

	public static int RandomIndex<T>(this IEnumerable<T> list)
	{
		return UnityEngine.Random.Range(0, list.Count());
	}

	public static T Next<T>(this IEnumerable<T> list, T anchor)
	{
		int num = list.ToList().IndexOf(anchor);
		if (num >= list.Count())
		{
			return list.ElementAtOrDefault(0);
		}

		if (num <= -1)
		{
			return list.ElementAtOrDefault(list.Count() - 1);
		}

		return list.ElementAtOrDefault(num + 1);
	}

	public static bool CompareArrays(int[,] arrayA, int[,] arrayB)
	{
		try
		{
			return arrayA.Rank == arrayB.Rank && Enumerable.Range(0, arrayA.Rank).All((int dimension) => arrayA.GetLength(dimension) == arrayB.GetLength(dimension)) && arrayA.Cast<int>().SequenceEqual(arrayB.Cast<int>());
		}
		catch
		{
			return false;
		}
	}

	public static float RemapProportion(this float value, float currentMin, float currentMax, float newMin, float newMax)
	{
		return (value - currentMin) / (currentMax - currentMin) * (newMax - newMin) + newMin;
	}

	public static float Distance(this Vector3 thisVec, Vector3 compareVec)
	{
		return Vector3.Distance(thisVec, compareVec);
	}

	public static Vector3 Difference(this Vector3 a, Vector3 b)
	{
		return a - b;
	}

	public static bool InRange(this Vector2 inRange, float inNum, bool isInclusive = true)
	{
		if (isInclusive)
		{
			if (inNum >= inRange.x)
			{
				return inNum <= inRange.y;
			}

			return false;
		}

		if (inNum > inRange.x)
		{
			return inNum < inRange.y;
		}

		return false;
	}

	public static T RandomEnumValue<T>(Likelihood likelihood)
	{
		Array values = Enum.GetValues(typeof(T));
		float[] array = new float[values.Length];
		array[0] = 100f;
		for (int i = 1; i < values.Length; i++)
		{
			array[i] = 100f / (float)Math.Pow(2.0, i);
		}

		if (array[values.Length - 1] > 1f)
		{
			array[values.Length - 1] = 1f;
		}

		array = array.Reverse().ToArray();
		int index = 0;
		switch (likelihood)
		{
			case Likelihood.Balanced:
				index = UnityEngine.Random.Range(0, values.Length);
				break;
			case Likelihood.MostToLeast:
			{
				int num2 = UnityEngine.Random.Range(0, 100);
			for (int k = 0; k < array.Length; k++)
			{
				if ((float)num2 < array[k])
				{
					index = array.Length - 1 - k;
					break;
				}
				}
	
				break;
			}
			case Likelihood.LeastToMost:
			{
				int num = UnityEngine.Random.Range(0, 100);
			for (int j = 0; j < array.Length; j++)
			{
				if ((float)num < array[j])
				{
					index = j;
					break;
				}
				}
	
				break;
			}
		}

		return (T)values.GetValue(index);
	}

	public static float GetPercentageBetweenValues(float inVal, float minVal, float maxVal)
	{
		return Mathf.Clamp01((inVal - minVal) / (maxVal - minVal));
	}

	public static float GetPercentageBetweenValues(this Vector2 inVec, float inVal)
	{
		return Mathf.Clamp01((inVal - inVec.x) / (inVec.y - inVec.x));
	}

	public static float GetValueAtPercentage(this Vector2 inVec, float percentage)
	{
		return inVec.x + (inVec.y - inVec.x) * percentage;
	}

	public static float Clamp(this Vector2 inVec, float inVal)
	{
		return Mathf.Clamp(inVal, inVec.x, inVec.y);
	}

	public static Vector3 ToVec3(this Vector2 inVec)
	{
		return new Vector3(inVec.x, inVec.y, 0f);
	}

	public static bool IsWithinThreshold(float value, float compareTo, float threshold)
	{
		if (value <= compareTo + threshold && value >= compareTo - threshold)
		{
			return true;
		}

		return false;
	}

	public static Texture2D FlipTexture(this Texture2D original)
	{
		Texture2D texture2D = new Texture2D(original.width, original.height);
		int width = original.width;
		int height = original.height;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				texture2D.SetPixel(width - i - 1, j, original.GetPixel(i, j));
			}
		}

		texture2D.Apply();
		return texture2D;
	}

	public static Texture2D ToTexture2D(this Texture texture)
	{
		Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, mipChain: false);
		RenderTexture active = RenderTexture.active;
		RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 32);
		Graphics.Blit(texture, temporary);
		RenderTexture.active = temporary;
		texture2D.ReadPixels(new Rect(0f, 0f, temporary.width, temporary.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}

	public static Vector3 RandomPointInRectTransform(RectTransform rectTransform)
	{
		return new Vector3(UnityEngine.Random.Range(rectTransform.rect.x, rectTransform.rect.x + rectTransform.rect.width), UnityEngine.Random.Range(rectTransform.rect.y, rectTransform.rect.y + rectTransform.rect.height), 0f);
	}

	public static T RandomEnumValue<T>()
	{
		Array values = Enum.GetValues(typeof(T));
		return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
	}

	public static T RandomValueFromList<T>(this List<T> list)
	{
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static T GetEnumValue<T>(string valueName)
	{
		return (T)Enum.Parse(typeof(T), valueName);
	}

	public static Vector2 GetCharPosition(this TextMeshPro text, int index)
	{
		text.GetComponent<Transform>();
		_ = Vector3.zero;
		_ = Vector3.zero;
		TMP_TextInfo textInfo = text.textInfo;
		if (index < 0 || index >= textInfo.characterInfo.Length)
		{
			return Vector2.zero;
		}

		TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[index];
		return (tMP_CharacterInfo.topLeft + tMP_CharacterInfo.topRight + tMP_CharacterInfo.bottomLeft + tMP_CharacterInfo.bottomRight) / 4f;
	}

	public static bool HasReachedDestination(this NavMeshAgent agent)
	{
		float num = 0.2f;
		if (agent.remainingDistance != float.PositiveInfinity && agent.transform.position.PlanarDistance(agent.destination, Axis.Y) < num)
		{
			return true;
		}

		return false;
	}

	public static Vector3 GetRandomPointOnNavMesh(this NavMeshAgent agent, float maxWalkDist)
	{
		NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * maxWalkDist + agent.transform.position, out var hit, maxWalkDist, 1);
		return hit.position;
	}

	public static string SpiltAndCamelCase(this string inString)
	{
		string text = new Regex("\r\n                (?<=[A-Z])(?=[A-Z][a-z]) |\r\n                 (?<=[^A-Z])(?=[A-Z]) |\r\n                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace).Replace(inString, " ");
		return char.ToUpper(text[0]) + text.Substring(1);
	}

	public static Vector3 GetSignedVec3(this Vector3 inVector)
	{
		return new Vector3(GetSignedAngle(inVector.x), GetSignedAngle(inVector.y), GetSignedAngle(inVector.z));
	}

	public static bool IsInCameraView(this Camera cam, Vector3 position, float border = 0f)
	{
		Vector3 vector = cam.WorldToViewportPoint(position);
		if (vector.x >= 0f - border && vector.x <= 1f + border && vector.y >= 0f - border && vector.y <= 1f + border)
		{
			return vector.z > 0f;
		}

		return false;
	}

	public static float GetSignedAngle(float inAngle)
	{
		float num = inAngle;
		bool flag = false;
		do
		{
			flag = true;
			if (num < -180f)
			{
				num += 360f;
			}
			else if (inAngle > 180f)
			{
				num -= 360f;
			}
		}
		while (!flag);
		return num;
	}

	public static T RandomValueFromList<T>(this T[] list)
	{
		return list[UnityEngine.Random.Range(0, list.Length)];
	}

	public static T RandomValueFromList<T>(List<T> listA, List<T> ignoreFromListA)
	{
		List<T> list = listA.Except(ignoreFromListA).ToList();
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static float RoundToNearestMultiple(this float value, float factor)
	{
		return (float)(int)Math.Round((double)value / (double)factor, MidpointRounding.AwayFromZero) * factor;
	}

	public static float Truncate(this float value, int digits)
	{
		double num = Math.Pow(10.0, digits);
		return (float)(Math.Truncate(num * (double)value) / num);
	}

	public static float PingPong(float timeValue, float minValue, float maxValue)
	{
		return Mathf.PingPong(timeValue, maxValue - minValue) + minValue;
	}

	public static IEnumerator ScaleObject(GameObject gameObj, float aValue, float aTime)
	{
		Vector3 currentScale = gameObj.transform.localScale;
		for (float t = 0f; t < 1f; t += Time.deltaTime / aTime)
		{
			Vector3 localScale = new Vector3(Mathf.Lerp(currentScale.x, aValue, t), Mathf.Lerp(currentScale.x, aValue, t), Mathf.Lerp(currentScale.x, aValue, t));
			gameObj.transform.localScale = localScale;
			yield return null;
		}
	}

	public static GameObject FindObject(this GameObject parent, string name)
	{
		Component[] componentsInChildren = parent.GetComponentsInChildren(typeof(Transform), includeInactive: true);
		Transform[] array = new Transform[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array[i] = componentsInChildren[i].transform;
		}

		Transform[] array2 = array;
		foreach (Transform transform in array2)
		{
			if (transform.name == name)
			{
				return transform.gameObject;
			}
		}

		return null;
	}

	public static void Update<TSource>(this IEnumerable<TSource> outer, Action<TSource> updator)
	{
		foreach (TSource item in outer)
		{
			updator(item);
		}
	}

	public static bool Approximately(this Quaternion quatA, Quaternion value, float acceptableRange)
	{
		return 1f - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
	}

	public static T Next<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
		}

		T[] array = (T[])Enum.GetValues(src.GetType());
		int num = Array.IndexOf(array, src) + 1;
		if (array.Length != num)
		{
			return array[num];
		}

		return array[0];
	}

	public static Vector3 Multiply(this Vector3 src, Vector3 vector)
	{
		return new Vector3(src.x * vector.x, src.y * vector.y, src.z * vector.z);
	}

	public static Vector3 ToVector3(this Color src)
	{
		return new Vector3(src.r, src.g, src.b);
	}

	public static Color ToColour(this Vector3 src, float alpha = 1f)
	{
		return new Color(src.x, src.y, src.z, alpha);
	}

	public static Color ColourFromVector3(Vector3 src)
	{
		return new Color(src.x, src.y, src.z);
	}

	public static void CopyFrom(this Transform toEdit, Transform copyFrom)
	{
		toEdit.transform.parent = copyFrom.parent;
		toEdit.transform.localScale = copyFrom.localScale;
		toEdit.transform.localRotation = copyFrom.localRotation;
		toEdit.transform.localPosition = copyFrom.localPosition;
	}

	public static void CopyFrom(this RectTransform objRectTransform, RectTransform rectTransform)
	{
		objRectTransform.transform.parent = rectTransform.parent;
		objRectTransform.anchorMin = rectTransform.anchorMin;
		objRectTransform.anchorMax = rectTransform.anchorMax;
		objRectTransform.anchoredPosition = rectTransform.anchoredPosition;
		objRectTransform.sizeDelta = rectTransform.sizeDelta;
		objRectTransform.localPosition = rectTransform.localPosition;
	}

	public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, Vector3 angle)
	{
		Vector3 vector = point - pivot;
		vector = Quaternion.Euler(angle) * vector;
		point = vector + pivot;
		return point;
	}

	public static Color ScaleToOne(this Color src)
	{
		return new Color((src.r > 1f) ? (src.r / 235f) : src.r, (src.g > 1f) ? (src.g / 235f) : src.g, (src.b > 1f) ? (src.b / 235f) : src.b, (src.a > 1f) ? (src.a / 235f) : src.a);
	}

	public static string ConvertToString(this char[] charArray)
	{
		string text = "";
		for (int i = 0; i < charArray.Length; i++)
		{
			text += charArray[i];
		}

		return text;
	}

	public static void SetLayerRecursively(this GameObject src, int newLayer)
	{
		src.layer = newLayer;
		foreach (Transform item in src.transform)
		{
			item.gameObject.SetLayerRecursively(newLayer);
		}
	}

	public static float SignedAngleBetween(Vector3 vector1, Vector3 vector2, Vector3 normal)
	{
		float num = Vector3.Angle(vector1, vector2);
		float num2 = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(vector1, vector2)));
		return num * num2;
	}

	public static float UnsignedAngleBetween(Vector3 vector1, Vector3 vector2, Vector3 normal)
	{
		float num = Vector3.Angle(vector1, vector2);
		float num2 = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(vector1, vector2)));
		return (num * num2 + 180f) % 360f;
	}

	public static Vector3 GetDirTowardsPoint(this Vector3 ownPos, Vector3 pointTowards)
	{
		return (pointTowards - ownPos).normalized;
	}

	public static Vector3 GetDirAwayFromPoint(this Vector3 ownPos, Vector3 pointAway)
	{
		return (ownPos - pointAway).normalized;
	}

	public static void Reset(this Transform inTransform)
	{
		inTransform.localPosition = Vector3.zero;
		inTransform.localRotation = Quaternion.identity;
		inTransform.localScale = Vector3.one;
	}

	public static void Reset(this RectTransform rectTransform)
	{
		Reset(rectTransform.transform);
		rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0f, 0f);
		rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, 0f);
		rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, 0f);
		rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0f, 0f);
	}

	public static T[] RangeSubset<T>(this T[] array, int startIndex, int length)
	{
		T[] array2 = new T[length];
		Array.Copy(array, startIndex, array2, 0, length);
		return array2;
	}

	public static T[] Subset<T>(this T[] array, params int[] indices)
	{
		T[] array2 = new T[indices.Length];
		for (int i = 0; i < indices.Length; i++)
		{
			array2[i] = array[indices[i]];
		}

		return array2;
	}

	public static bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			return false;
		}

		try
		{
			email = Regex.Replace(email, "(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200.0));
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}

		catch (ArgumentException)
		{
			return false;
		}

		try
		{
			return Regex.IsMatch(email, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250.0));
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}

		string DomainMapper(Match match)
		{
			string ascii = new IdnMapping().GetAscii(match.Groups[2].Value);
			return match.Groups[1].Value + ascii;
		}
	}
}
