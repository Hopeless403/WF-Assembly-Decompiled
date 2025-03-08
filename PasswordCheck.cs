#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;

public static class PasswordCheck
{
	public enum PasswordStrength
	{
		Blank,
		VeryWeak,
		Weak,
		Medium,
		Strong,
		VeryStrong
	}

	public struct PasswordOptions
	{
		public int RequiredLength;

		public int RequiredUniqueChars;

		public bool RequireNonAlphanumeric;

		public bool RequireLowercase;

		public bool RequireUppercase;

		public bool RequireDigit;
	}

	public static PasswordStrength GetPasswordStrength(string password)
	{
		int num = 0;
		if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password.Trim()))
		{
			return PasswordStrength.Blank;
		}

		if (HasMinimumLength(password, 5))
		{
			num++;
		}

		if (HasMinimumLength(password, 8))
		{
			num++;
		}

		if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password))
		{
			num++;
		}

		if (HasDigit(password))
		{
			num++;
		}

		if (HasSpecialChar(password))
		{
			num++;
		}

		return (PasswordStrength)num;
	}

	public static bool IsStrongPassword(string password)
	{
		if (HasMinimumLength(password, 8) && HasUpperCaseLetter(password) && HasLowerCaseLetter(password))
		{
			if (!HasDigit(password))
			{
				return HasSpecialChar(password);
			}

			return true;
		}

		return false;
	}

	public static bool IsValidPassword(string password, PasswordOptions opts)
	{
		return IsValidPassword(password, opts.RequiredLength, opts.RequiredUniqueChars, opts.RequireNonAlphanumeric, opts.RequireLowercase, opts.RequireUppercase, opts.RequireDigit);
	}

	public static bool IsValidPassword(string password, int requiredLength, int requiredUniqueChars, bool requireNonAlphanumeric, bool requireLowercase, bool requireUppercase, bool requireDigit)
	{
		if (!HasMinimumLength(password, requiredLength))
		{
			return false;
		}

		if (!HasMinimumUniqueChars(password, requiredUniqueChars))
		{
			return false;
		}

		if (requireNonAlphanumeric && !HasSpecialChar(password))
		{
			return false;
		}

		if (requireLowercase && !HasLowerCaseLetter(password))
		{
			return false;
		}

		if (requireUppercase && !HasUpperCaseLetter(password))
		{
			return false;
		}

		if (requireDigit && !HasDigit(password))
		{
			return false;
		}

		return true;
	}

	public static bool HasMinimumLength(string password, int minLength)
	{
		return password.Length >= minLength;
	}

	public static bool HasMinimumUniqueChars(string password, int minUniqueChars)
	{
		return password.Distinct().Count() >= minUniqueChars;
	}

	public static bool HasDigit(string password)
	{
		return password.Any((char c) => char.IsDigit(c));
	}

	public static bool HasSpecialChar(string password)
	{
		return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
	}

	public static bool HasUpperCaseLetter(string password)
	{
		return password.Any((char c) => char.IsUpper(c));
	}

	public static bool HasLowerCaseLetter(string password)
	{
		return password.Any((char c) => char.IsLower(c));
	}
}
