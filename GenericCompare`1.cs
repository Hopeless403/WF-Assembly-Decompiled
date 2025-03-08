#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;

public class GenericCompare<T> : IComparer<T>
{
	public Func<T, T, int> ComparerFunction { get; set; }

	public GenericCompare(Func<T, T, int> comparerFunction)
	{
		ComparerFunction = comparerFunction;
	}

	public int Compare(T x, T y)
	{
		if (x == null || y == null)
		{
			if (y == null && x == null)
			{
				return 0;
			}

			if (y == null)
			{
				return 1;
			}

			if (x == null)
			{
				return -1;
			}
		}

		try
		{
			return ComparerFunction(x, y);
		}
		catch (Exception)
		{
		}

		return 0;
	}
}
