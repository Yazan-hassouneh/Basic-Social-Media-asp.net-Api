using AutoMapper;

namespace BasicSocialMedia.Application.Utils
{
	public class Base64StringToByteArrayConverter : IValueConverter<string, byte[]>
	{
		public byte[] Convert(string sourceMember, ResolutionContext context)
		{
			if (string.IsNullOrEmpty(sourceMember))
			{
				return null;  // Handle null or empty strings, if needed
			}

			return System.Convert.FromBase64String(sourceMember);  // Convert Base64 string back to byte array
		}
	}
}
