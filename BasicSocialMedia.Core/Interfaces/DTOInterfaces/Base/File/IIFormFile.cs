using Microsoft.AspNetCore.Http;

namespace BasicSocialMedia.Core.Interfaces.DTOInterfaces.Base.File
{
	public interface IIFormFile
	{
		public List<IFormFile> Files { get; set; }
	}
}
