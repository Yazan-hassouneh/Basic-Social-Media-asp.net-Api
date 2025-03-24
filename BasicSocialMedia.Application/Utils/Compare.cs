using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSocialMedia.Application.Utils
{
	internal static class Compare
	{
		public static bool ByteArrayCompare(byte[] a1, byte[] a2)
		{
			if (a1 == null || a2 == null)
				return a1 == a2;

			if (a1.Length != a2.Length)
				return false;

			for (int i = 0; i < a1.Length; i++)
			{
				if (a1[i] != a2[i])
					return false;
			}

			return true;
		}
	}
}
