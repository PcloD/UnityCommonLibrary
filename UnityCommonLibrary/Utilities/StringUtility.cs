using System.Text;

namespace UnityCommonLibrary.Utility
{
	public static class StringUtility
	{
		public static bool IsNullOrWhitespace(string str)
		{
			if(str == null)
			{
				return true;
			}
			for(int i = 0; i < str.Length; i++)
			{
				if(!char.IsWhiteSpace(str[i]))
				{
					return false;
				}
			}
			return true;
		}
		public static bool ContainsIgnoreCase(this string str, string other)
		{
			return str.IndexOf(other, System.StringComparison.OrdinalIgnoreCase) >= 0;
		}
		public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object[] args)
		{
			return builder.AppendLine(string.Format(format, args));
		}
	}
}