
namespace BasicSocialMedia.Core.Consts
{
	public static class FileSettings
	{
		//Profile paths
		public const string ProfileImagesPath = "/Uploads/ProfileImgs";
		//Comments paths
		public const string CommentsImagesPath = "/Uploads/Comments/Images/";
		public const string CommentsVideosPath = "/Uploads/Comments/Video/";
		//Message paths
		public const string MessagesImagesPath = "/Uploads/Messages/Images/";
		public const string MessagesVideosPath = "/Uploads/Messages/Video/";
		// post paths
		public const string PostsImagesPath = "/Uploads/Posts/Images/";
		public const string PostsVideosPath = "/Uploads/Posts/Video/";

		public const string ImagesAllowedExtension = ".jpeg,.jpg,.png";
		public const int ImagesMaxSizeInMB = 3;
		public const int ImagesMaxSizeInBytes = ImagesMaxSizeInMB * 1024 * 1024;

		public const string VideoAllowedExtension = ".mp4,.mov,.avi";
		public const int VideoMaxSizeInMB = 300;
		public const int VideoMaxSizeInBytes = ImagesMaxSizeInMB * 1024 * 1024;
	}
}
