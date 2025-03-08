#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using Dead;
using UnityEngine;

[Serializable]
public struct Stat
{
	public static readonly Stat Default = new Stat(1, 0);

	[SerializeField]
	public SafeInt safeCurrent;

	[SerializeField]
	public SafeInt safeMax;

	public int current
	{
		get
		{
			return safeCurrent.Value;
		}
		set
		{
			safeCurrent = new SafeInt(value);
		}
	}

	public int max
	{
		get
		{
			return safeMax.Value;
		}
		set
		{
			safeMax = new SafeInt(value);
		}
	}

	public Stat(int value)
	{
		safeCurrent = new SafeInt(value);
		safeMax = new SafeInt(value);
	}

	public Stat(int current, int max)
	{
		safeCurrent = new SafeInt(current);
		safeMax = new SafeInt(max);
	}

	public void Max()
	{
		safeCurrent.Value = safeMax.Value;
	}

	public static Stat operator +(Stat a, int b)
	{
		return new Stat(a.safeCurrent.Value + b, a.safeMax.Value);
	}

	public static Stat operator -(Stat a, int b)
	{
		return new Stat(a.safeCurrent.Value - b, a.safeMax.Value);
	}

	public static Stat operator *(Stat a, int b)
	{
		return new Stat(a.safeCurrent.Value * b, a.safeMax.Value);
	}

	public static Stat operator +(Stat a, SafeInt b)
	{
		return new Stat(a.safeCurrent.Value + b.Value, a.safeMax.Value);
	}

	public static Stat operator -(Stat a, SafeInt b)
	{
		return new Stat(a.safeCurrent.Value - b.Value, a.safeMax.Value);
	}

	public static Stat operator *(Stat a, SafeInt b)
	{
		return new Stat(a.safeCurrent.Value * b.Value, a.safeMax.Value);
	}

	public static bool operator ==(Stat a, int b)
	{
		return a.safeCurrent.Value == b;
	}

	public static bool operator !=(Stat a, int b)
	{
		return a.safeCurrent.Value != b;
	}

	public static bool operator >(Stat a, int b)
	{
		return a.safeCurrent.Value > b;
	}

	public static bool operator >=(Stat a, int b)
	{
		return a.safeCurrent.Value >= b;
	}

	public static bool operator <(Stat a, int b)
	{
		return a.safeCurrent.Value < b;
	}

	public static bool operator <=(Stat a, int b)
	{
		return a.safeCurrent.Value <= b;
	}

	public static bool operator ==(int a, Stat b)
	{
		return a == b.safeCurrent.Value;
	}

	public static bool operator !=(int a, Stat b)
	{
		return a != b.safeCurrent.Value;
	}

	public static bool operator >(int a, Stat b)
	{
		return a > b.safeCurrent.Value;
	}

	public static bool operator >=(int a, Stat b)
	{
		return a >= b.safeCurrent.Value;
	}

	public static bool operator <(int a, Stat b)
	{
		return a < b.safeCurrent.Value;
	}

	public static bool operator <=(int a, Stat b)
	{
		return a <= b.safeCurrent.Value;
	}

	public override string ToString()
	{
		return $"{safeCurrent.Value}/{safeMax.Value}";
	}

	public override bool Equals(object obj)
	{
		if (obj is int num)
		{
			return safeCurrent.Value == num;
		}

		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
