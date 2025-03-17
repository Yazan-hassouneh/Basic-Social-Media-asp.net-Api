
namespace BasicSocialMedia.Core.Consts
{
	public static class FileSettings
	{
		//Profile paths
		public const string ProfileImagesPath = "/Uploads/ProfileImgs";
		//Comments paths
		public const string CommentsImagesPath = "/Uploads/Comments/Images/";
		public const string CommentsVideosPath = "/Uploads/Comments/Video/";	
		// post paths
		public const string PostsImagesPath = "/Uploads/Posts/Images/";
		public const string PostsVideosPath = "/Uploads/Posts/Video/";

		public const string ImagesAllowedExtension = ".jpeg,.jpg,.png";
		public const int ImagesMaxSizeInMB = 1;
		public const int ImagesMaxSizeInBytes = ImagesMaxSizeInMB * 1024 * 1024;
	}
}
