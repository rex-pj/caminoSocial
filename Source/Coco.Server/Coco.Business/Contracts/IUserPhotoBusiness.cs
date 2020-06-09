﻿using Coco.Entities.Enums;
using Coco.Entities.Dtos;
using Coco.Entities.Dtos.General;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Coco.Business.Contracts
{
    public interface IUserPhotoBusiness
    {
        Task<UserPhotoUpdateDto> UpdateUserPhotoAsync(UserPhotoUpdateDto model, long userId);
        Task DeleteUserPhotoAsync(long userId, UserPhotoTypeEnum userPhotoType);
        Task<UserPhotoDto> GetUserPhotoByCodeAsync(string code, UserPhotoTypeEnum type);
        UserPhotoDto GetUserPhotoByUserId(long userId, UserPhotoTypeEnum type);
        Task<IEnumerable<UserPhotoDto>> GetUserPhotosAsync(long userId);
    }
}
