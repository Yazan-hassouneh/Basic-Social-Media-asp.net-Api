using AutoMapper;

namespace BasicSocialMedia.Application.Utils
{
	internal class ByteArrayToBase64StringConverter : ITypeConverter<byte[], string?>
	{
		public string? Convert(byte[] source, string? destination, ResolutionContext context)
		{
			if (source == null)
			{
				return null;
			}
			return System.Convert.ToBase64String(source);
		}
	}
}
