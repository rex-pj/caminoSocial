﻿using Camino.Core.Utils;
using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.Data.Enums;
using Camino.IdentityDAL.Entities;
using Camino.Service.Business.Farms.Contracts;
using Camino.Service.Projections.Farm;
using Camino.Service.Projections.Filters;
using Camino.Service.Projections.Media;
using Camino.Service.Projections.PageList;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Camino.Service.Business.Farms
{
    public class FarmBusiness : IFarmBusiness
    {
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<FarmType> _farmTypeRepository;
        private readonly IRepository<FarmPicture> _farmPictureRepository;
        private readonly IRepository<FarmProduct> _farmProductRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<ProductPrice> _productPriceRepository;
        private readonly IRepository<ProductCategoryRelation> _productCategoryRelationRepository;

        public FarmBusiness(IRepository<Farm> farmRepository, IRepository<FarmType> farmTypeRepository,
            IRepository<FarmPicture> farmPictureRepository, IRepository<User> userRepository,
            IRepository<Picture> pictureRepository, IRepository<UserPhoto> userPhotoRepository,
            IRepository<FarmProduct> farmProductRepository, IRepository<Product> productRepository,
            IRepository<ProductPicture> productPictureRepository, IRepository<ProductPrice> productPriceRepository,
            IRepository<ProductCategoryRelation> productCategoryRelationRepository)
        {
            _farmRepository = farmRepository;
            _farmTypeRepository = farmTypeRepository;
            _farmPictureRepository = farmPictureRepository;
            _userRepository = userRepository;
            _pictureRepository = pictureRepository;
            _userPhotoRepository = userPhotoRepository;
            _farmProductRepository = farmProductRepository;
            _productRepository = productRepository;
            _productPictureRepository = productPictureRepository;
            _productPriceRepository = productPriceRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
        }

        public async Task<FarmProjection> FindAsync(long id)
        {
            var exist = await (from farm in _farmRepository.Get(x => x.Id == id && !x.IsDeleted)
                               join farmType in _farmTypeRepository.Table
                               on farm.FarmTypeId equals farmType.Id
                               select new FarmProjection
                               {
                                   CreatedDate = farm.CreatedDate,
                                   CreatedById = farm.CreatedById,
                                   Id = farm.Id,
                                   Name = farm.Name,
                                   Address = farm.Address,
                                   UpdatedById = farm.UpdatedById,
                                   UpdatedDate = farm.UpdatedDate,
                                   Description = farm.Description,
                                   FarmTypeId = farm.FarmTypeId,
                                   FarmTypeName = farmType.Name
                               }).FirstOrDefaultAsync();

            return exist;
        }

        public async Task<FarmProjection> FindDetailAsync(long id)
        {
            var farmPictureType = (int)FarmPictureType.Thumbnail;
            var farmPictures = from farmPic in _farmPictureRepository.Get(x => x.PictureTypeId == farmPictureType)
                               join picture in _pictureRepository.Get(x => !x.IsDeleted)
                               on farmPic.PictureId equals picture.Id
                               select farmPic;

            var exist = await (from farm in _farmRepository.Get(x => x.Id == id && !x.IsDeleted)
                               join farmType in _farmTypeRepository.Table
                               on farm.FarmTypeId equals farmType.Id
                               join farmPic in _farmPictureRepository.Table
                               on farm.Id equals farmPic.FarmId into pics
                               select new FarmProjection
                               {
                                   Id = farm.Id,
                                   Name = farm.Name,
                                   Address = farm.Address,
                                   Description = farm.Description,
                                   CreatedDate = farm.CreatedDate,
                                   CreatedById = farm.CreatedById,
                                   UpdatedById = farm.UpdatedById,
                                   UpdatedDate = farm.UpdatedDate,
                                   FarmTypeName = farmType.Name,
                                   FarmTypeId = farm.FarmTypeId,
                                   Pictures = pics.Select(x => new PictureRequestProjection
                                   {
                                       Id = x.PictureId
                                   }),
                               }).FirstOrDefaultAsync();

            if (exist == null)
            {
                return null;
            }

            var createdByUserName = await _userRepository.Get(x => x.Id == exist.CreatedById).Select(x => x.DisplayName).FirstOrDefaultAsync();
            exist.CreatedBy = createdByUserName;

            var updatedByUserName = await _userRepository.Get(x => x.Id == exist.UpdatedById).Select(x => x.DisplayName).FirstOrDefaultAsync();
            exist.UpdatedBy = updatedByUserName;

            return exist;
        }

        public async Task<IList<FarmProjection>> SelectAsync(SelectFilter filter, int page = 1, int pageSize = 10)
        {
            var query = _farmRepository.Get(x => !x.IsDeleted)
                .Select(c => new FarmProjection
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                });

            if (filter.CreatedById > 0)
            {
                query = query.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.CurrentIds.Any())
            {
                query = query.Where(x => !filter.CurrentIds.Contains(x.Id));
            }

            filter.Search = filter.Search.ToLower();
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filter.Search) || x.Description.ToLower().Contains(filter.Search));
            }

            if (pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var farms = await query
                .Select(x => new FarmProjection()
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return farms;
        }

        public FarmProjection FindByName(string name)
        {
            var exist = _farmRepository.Get(x => x.Name == name && !x.IsDeleted)
                .Select(x => new FarmProjection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    FarmTypeId = x.FarmTypeId,
                    UpdatedById = x.UpdatedById,
                    CreatedById = x.CreatedById,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .FirstOrDefault();

            return exist;
        }

        public async Task<BasePageList<FarmProjection>> GetAsync(FarmFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var farmQuery = _farmRepository.Get(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(search))
            {
                farmQuery = farmQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.ExclusiveCreatedById.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedById != filter.ExclusiveCreatedById);
            }

            if (filter.CreatedById.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.FarmTypeId.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.FarmTypeId == filter.FarmTypeId);
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                farmQuery = farmQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTimeOffset.UtcNow);
            }

            var filteredNumber = farmQuery.Select(x => x.Id).Count();

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var thumbnailTypeId = (int)FarmPictureType.Thumbnail;

            var farmPictures = from farmPic in _farmPictureRepository.Get(x => x.PictureTypeId == thumbnailTypeId)
                               join picture in _pictureRepository.Get(x => !x.IsDeleted)
                               on farmPic.PictureId equals picture.Id
                               select farmPic;

            var query = from farm in farmQuery
                        join pic in farmPictures
                        on farm.Id equals pic.FarmId into pics
                        join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                        on farm.CreatedById equals pho.CreatedById into photos
                        from userPhoto in photos.DefaultIfEmpty()
                        select new FarmProjection
                        {
                            Id = farm.Id,
                            Name = farm.Name,
                            Address = farm.Address,
                            CreatedById = farm.CreatedById,
                            CreatedDate = farm.CreatedDate,
                            Description = farm.Description,
                            UpdatedById = farm.UpdatedById,
                            UpdatedDate = farm.UpdatedDate,
                            CreatedByPhotoCode = userPhoto.Code,
                            Pictures = pics.Select(x => new PictureRequestProjection
                            {
                                Id = x.PictureId
                            })
                        };

            var farms = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var createdByIds = farms.Select(x => x.CreatedById).ToArray();
            var updatedByIds = farms.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).Select(x => new
            {
                x.DisplayName,
                x.Id
            }).ToList();

            foreach (var article in farms)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == article.CreatedById);
                article.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == article.UpdatedById);
                article.UpdatedBy = updatedBy.DisplayName;
            }

            var result = new BasePageList<FarmProjection>(farms)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<long> CreateAsync(FarmProjection farm)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var newFarm = new Farm()
            {
                FarmTypeId = farm.FarmTypeId,
                Name = farm.Name,
                Address = farm.Address,
                UpdatedById = farm.UpdatedById,
                CreatedById = farm.CreatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Description = farm.Description,
                IsPublished = true
            };

            var id = await _farmRepository.AddWithInt64EntityAsync(newFarm);
            if (id > 0)
            {
                var index = 0;
                foreach (var picture in farm.Pictures)
                {
                    var thumbnail = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                    var pictureData = Convert.FromBase64String(thumbnail);
                    var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                    {
                        CreatedById = farm.UpdatedById,
                        CreatedDate = modifiedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = farm.UpdatedById,
                        UpdatedDate = modifiedDate,
                        BinaryData = pictureData,
                        IsPublished = true
                    });

                    var farmPictureType = index == 0 ? (int)FarmPictureType.Thumbnail : (int)FarmPictureType.Secondary;
                    _farmPictureRepository.Add(new FarmPicture()
                    {
                        FarmId = id,
                        PictureId = pictureId,
                        PictureTypeId = farmPictureType
                    });
                    index += 1;
                }
            }

            return id;
        }

        public async Task<FarmProjection> UpdateAsync(FarmProjection request)
        {
            var updatedDate = DateTimeOffset.UtcNow;
            var farm = _farmRepository.FirstOrDefault(x => x.Id == request.Id);
            farm.Description = request.Description;
            farm.Name = request.Name;
            farm.FarmTypeId = request.FarmTypeId;
            farm.UpdatedById = request.UpdatedById;
            farm.UpdatedDate = updatedDate;
            farm.Address = request.Address;

            var pictureIds = request.Pictures.Select(x => x.Id);
            var deleteFarmPictures = _farmPictureRepository
                        .Get(x => x.FarmId == request.Id && !pictureIds.Contains(x.PictureId));

            var deletePictureIds = deleteFarmPictures.Select(x => x.PictureId).ToList();
            if (deletePictureIds.Any())
            {
                await deleteFarmPictures.DeleteAsync();

                await _pictureRepository.Get(x => deletePictureIds.Contains(x.Id))
                    .DeleteAsync();
            }

            var thumbnailType = (int)FarmPictureType.Thumbnail;
            var shouldAddThumbnail = true;
            var hasThumbnail = _farmPictureRepository.Get(x => x.FarmId == request.Id && x.PictureTypeId == thumbnailType).Any();
            if (hasThumbnail)
            {
                shouldAddThumbnail = false;
            }

            foreach (var picture in request.Pictures)
            {
                if (!string.IsNullOrEmpty(picture.Base64Data))
                {
                    var base64Data = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                    var pictureData = Convert.FromBase64String(base64Data);
                    var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                    {
                        CreatedById = request.UpdatedById,
                        CreatedDate = updatedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = request.UpdatedById,
                        UpdatedDate = updatedDate,
                        BinaryData = pictureData,
                        IsPublished = true
                    });

                    var farmPictureType = shouldAddThumbnail ? thumbnailType : (int)FarmPictureType.Secondary;
                    _farmPictureRepository.Add(new FarmPicture()
                    {
                        FarmId = farm.Id,
                        PictureId = pictureId,
                        PictureTypeId = farmPictureType
                    });
                    shouldAddThumbnail = false;
                }
            }

            var firstRestPicture = await _farmPictureRepository.FirstOrDefaultAsync(x => x.FarmId == request.Id && x.PictureTypeId != thumbnailType);
            if (firstRestPicture != null)
            {
                firstRestPicture.PictureTypeId = thumbnailType;
                await _farmPictureRepository.UpdateAsync(firstRestPicture);
            }

            _farmRepository.Update(farm);

            return request;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // Delete farm pictures
            var farmPictures = _farmPictureRepository.Get(x => x.FarmId == id);
            var pictureIds = farmPictures.Select(x => x.PictureId).ToList();
            await farmPictures.DeleteAsync();

            await _pictureRepository.Get(x => pictureIds.Contains(x.Id))
                .DeleteAsync();

            // Delete farm products
            await DeleteProductByFarmIdAsync(id);

            // Delete farm
            await _farmRepository.Get(x => x.Id == id)
                .DeleteAsync();

            return true;
        }

        /// <summary>
        /// Delete products by farm id
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        private async Task DeleteProductByFarmIdAsync(long farmId)
        {
            var farmProducts = _farmProductRepository.Get(x => x.FarmId == farmId);
            var productIds = farmProducts.Select(x => x.ProductId).ToList();

            var productPictures = _productPictureRepository.Get(x => productIds.Contains(x.ProductId));
            var pictureIds = productPictures.Select(x => x.PictureId).ToList();
            await productPictures.DeleteAsync();

            await _pictureRepository.Get(x => pictureIds.Contains(x.Id))
                .DeleteAsync();

            await farmProducts.DeleteAsync();

            await _productPriceRepository.Get(x => productIds.Contains(x.ProductId))
                .DeleteAsync();

            await _productCategoryRelationRepository
                .Get(x => productIds.Contains(x.ProductId))
                .DeleteAsync();

            var products = _productRepository.Get(x => productIds.Contains(x.Id))
                .DeleteAsync();
        }

        public async Task<bool> SoftDeleteAsync(long id)
        {
            // Delete farm pictures
            await (from farmPicture in _farmPictureRepository.Get(x => x.FarmId == id)
                   join picture in _pictureRepository.Table
                   on farmPicture.PictureId equals picture.Id
                   select picture)
                    .Set(x => x.IsDeleted, true)
                    .UpdateAsync();

            // Delete farm products
            await SoftDeleteProductByFarmIdAsync(id);

            // Delete farm
            await _farmRepository.Get(x => x.Id == id)
                .Set(x => x.IsDeleted, true)
                .UpdateAsync();

            return true;
        }

        private async Task SoftDeleteProductByFarmIdAsync(long farmId)
        {
            var productIds = _farmProductRepository.Get(x => x.FarmId == farmId)
                .Select(x => x.ProductId);

            await (from productPicture in _productPictureRepository.Get(x => productIds.Contains(x.ProductId))
                   join picture in _pictureRepository.Table
                   on productPicture.PictureId equals picture.Id
                   select picture)
                .Set(x => x.IsDeleted, true)
                .UpdateAsync();

            await _productRepository.Get(x => productIds.Contains(x.Id))
                .Set(x => x.IsDeleted, true)
                .UpdateAsync();
        }
    }
}
