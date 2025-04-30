using AutoMapper;
using BasicSocialMedia.Core.DTOs.M2MDTOs;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.M2MServices;
using BasicSocialMedia.Core.Interfaces.UnitOfWork;
using BasicSocialMedia.Core.Models.M2MRelations;

namespace BasicSocialMedia.Application.Services.M2MServices
{
	public class BlockService(IUnitOfWork unitOfWork, IMapper mapper) : IBlockService
	{
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IMapper _mapper = mapper;

		public async Task<bool> IsUsersBlocked(string userId1, string userId2)
		{
			return await _unitOfWork.Blocking.IsBlockingAsync(userId1, userId2);
		}
		public async Task<bool> BlockUserAsync(BlockUserDto requestDto)
		{
			Block block = new()
			{
				BlockedId = requestDto.BlockedId,
				BlockerId = requestDto.BlockerId,
			};
			await _unitOfWork.Blocking.AddAsync(block);
			await _unitOfWork.Blocking.Save();
			return true;
		}
		public async Task<IEnumerable<GetBlockUserDto>> GetBlockListAsync(string userId)
		{
			var blockList = await _unitOfWork.Blocking.GetBlockListAsync(userId);
			return _mapper.Map<IEnumerable<GetBlockUserDto>>(blockList);
		}
		public async Task<bool> UnBlockUserAsync(int blockingId)
		{
			var blockEntity = await _unitOfWork.Blocking.GetByIdAsync(blockingId);
			if (blockEntity == null) return false;

			_unitOfWork.Blocking.Delete(blockEntity);
			await _unitOfWork.Blocking.Save();
			return true;
		}		
		public async Task<bool> DeleteBlockListByUserIdAsync(string userId)
		{
			IEnumerable<Block?> blockEntity = await _unitOfWork.Blocking.FindAllWithTrackingAsync(blockEntity => blockEntity.BlockedId == userId || blockEntity.BlockerId == userId);
			IEnumerable<Block> nonNullableBlockEntity = blockEntity.Where(blockEntity => blockEntity != null).Select(blockEntity => blockEntity!);

			_unitOfWork.Blocking.DeleteRange(nonNullableBlockEntity);
			await _unitOfWork.Blocking.Save();
			return true;
		}
		public async Task<bool> SetUserIdToNull(string userId)
		{
			var blocked = await _unitOfWork.Blocking.FindAllWithTrackingAsync(block => block.BlockerId == userId || block.BlockedId == userId);
			if (blocked == null || !blocked.Any()) return true;

			if (blocked.Any())
			{
				foreach (var block in blocked)
				{
					bool updated = false;

					if (block!.BlockerId == userId)
					{
						block.BlockerId = null;
						updated = true;
					}

					if (block.BlockedId == userId)
					{
						block.BlockedId = null;
						updated = true;
					}

					if (updated) _unitOfWork.Blocking.Update(block);
				}

				await _unitOfWork.Blocking.Save();
				return true;
			}
			return true;
		}
	}
}
