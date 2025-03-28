using BasicSocialMedia.Core.DTOs.M2MDTOs;

namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices
{
	public interface IBlockService
	{
		Task<bool> IsUsersBlocked(string userId1, string userId2);
		Task<IEnumerable<GetBlockUserDto>> GetBlockListAsync(string userId);
		Task<bool> BlockUserAsync(BlockUserDto requestDto);
		Task<bool> UnBlockUserAsync(int blockingId);
	}
}
