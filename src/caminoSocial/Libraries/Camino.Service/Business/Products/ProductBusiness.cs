﻿using AutoMapper;
using Camino.Service.Projections.Filters;
using Camino.Data.Contracts;
using LinqToDB;
using System;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Products.Contracts;
using Camino.DAL.Entities;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;
using Camino.Data.Enums;
using Camino.Core.Utils;
using System.Collections.Generic;
using Camino.Service.Projections.Media;
using Camino.Service.Projections.Product;

namespace Camino.Service.Business.Products
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<FarmProduct> _farmProductRepository;
        private readonly IRepository<Farm> _farmRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<UserPhoto> _userPhotoRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<ProductPrice> _productPriceRepository;
        private readonly IRepository<ProductCategoryRelation> _productCategoryRelationRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ProductBusiness(IMapper mapper, IRepository<Product> productRepository,
            IRepository<ProductCategoryRelation> productCategoryRelationRepository, IRepository<User> userRepository,
            IRepository<Picture> pictureRepository, IRepository<ProductPicture> productPictureRepository,
            IRepository<UserPhoto> userPhotoRepository, IRepository<ProductPrice> productPriceRepository,
            IRepository<FarmProduct> farmProductRepository, IRepository<Farm> farmRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _productCategoryRelationRepository = productCategoryRelationRepository;
            _userRepository = userRepository;
            _pictureRepository = pictureRepository;
            _productPictureRepository = productPictureRepository;
            _userPhotoRepository = userPhotoRepository;
            _productPriceRepository = productPriceRepository;
            _farmProductRepository = farmProductRepository;
            _farmRepository = farmRepository;
        }

        public ProductProjection Find(long id)
        {
            var exist = (from product in _productRepository.Table
                         where product.Id == id
                         select new ProductProjection
                         {
                             CreatedDate = product.CreatedDate,
                             CreatedById = product.CreatedById,
                             Id = product.Id,
                             Name = product.Name,
                             UpdatedById = product.UpdatedById,
                             UpdatedDate = product.UpdatedDate,
                         }).FirstOrDefault();

            if (exist == null)
            {
                return null;
            }

            return exist;
        }

        public async Task<ProductProjection> FindDetailAsync(long id)
        {
            var farmQuery = from farm in _farmRepository.Table
                            join farmProduct in _farmProductRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var product = await (from p in _productRepository.Table
                                 join pr in _productPriceRepository.Get(x => x.IsCurrent)
                                 on p.Id equals pr.ProductId into prices
                                 from price in prices.DefaultIfEmpty()

                                 join productPic in _productPictureRepository.Table
                                 on p.Id equals productPic.ProductId into pics

                                 join fp in farmQuery
                                 on p.Id equals fp.ProductId into farmProducts
                                 where p.Id == id
                                 select new ProductProjection
                                 {
                                     Description = p.Description,
                                     CreatedDate = p.CreatedDate,
                                     CreatedById = p.CreatedById,
                                     Id = p.Id,
                                     Name = p.Name,
                                     UpdatedById = p.UpdatedById,
                                     UpdatedDate = p.UpdatedDate,
                                     Price = price.Price,
                                     ProductFarms = farmProducts.Select(x => new ProductFarmProjection()
                                     {
                                         Id = x.Id,
                                         FarmId = x.FarmId,
                                         FarmName = x.Name
                                     }),
                                     Thumbnails = pics.Select(x => new PictureRequestProjection
                                     {
                                         Id = x.PictureId
                                     }),
                                 }).FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
            }

            var createdByUserName = await _userRepository.Get(x => x.Id == product.CreatedById).Select(x => x.DisplayName).FirstOrDefaultAsync();
            product.CreatedBy = createdByUserName;

            return product;
        }

        public ProductProjection FindByName(string name)
        {
            var exist = _productRepository.Get(x => x.Name == name)
                .FirstOrDefault();

            var product = _mapper.Map<ProductProjection>(exist);

            return product;
        }

        public async Task<BasePageList<ProductProjection>> GetAsync(ProductFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var productQuery = _productRepository.Table;
            if (!string.IsNullOrEmpty(search))
            {
                productQuery = productQuery.Where(user => user.Name.ToLower().Contains(search)
                         || user.Description.ToLower().Contains(search));
            }

            if (filter.CreatedById.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedById == filter.CreatedById);
            }

            if (filter.UpdatedById.HasValue)
            {
                productQuery = productQuery.Where(x => x.UpdatedById == filter.UpdatedById);
            }

            if (filter.CategoryId.HasValue)
            {
                productQuery = productQuery.Where(x => x.ProductCategories.Any(c => c.ProductCategoryId == filter.CategoryId));
            }

            if (filter.FarmId.HasValue)
            {
                productQuery = productQuery.Where(x => x.ProductFarms.Any(c => c.FarmId == filter.FarmId));
            }

            // Filter by register date/ created date
            if (filter.CreatedDateFrom.HasValue && filter.CreatedDateTo.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateTo.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedDate <= filter.CreatedDateTo);
            }
            else if (filter.CreatedDateFrom.HasValue)
            {
                productQuery = productQuery.Where(x => x.CreatedDate >= filter.CreatedDateFrom && x.CreatedDate <= DateTime.UtcNow);
            }

            var filteredNumber = productQuery.Select(x => x.Id).Count();

            var farmQuery = from farm in _farmRepository.Table
                            join farmProduct in _farmProductRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var thumbnailTypeId = (byte)ProductPictureType.Thumbnail;
            var query = from product in productQuery
                        join productPic in _productPictureRepository.Get(x => x.PictureType == thumbnailTypeId)
                        on product.Id equals productPic.ProductId into pics
                        join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                        on product.CreatedById equals pho.CreatedById into photos
                        from userPhoto in photos.DefaultIfEmpty()
                        join pr in _productPriceRepository.Get(x => x.IsCurrent)
                        on product.Id equals pr.ProductId into prices
                        from price in prices.DefaultIfEmpty()
                        join fp in farmQuery
                        on product.Id equals fp.ProductId into farmProducts
                        select new ProductProjection
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Price = price != null ? price.Price : 0,
                            CreatedById = product.CreatedById,
                            CreatedDate = product.CreatedDate,
                            Description = product.Description,
                            UpdatedById = product.UpdatedById,
                            UpdatedDate = product.UpdatedDate,
                            CreatedByPhotoCode = userPhoto.Code,
                            Thumbnails = pics.Select(x => new PictureRequestProjection
                            {
                                Id = x.PictureId
                            }),
                            ProductFarms = farmProducts.Select(x => new ProductFarmProjection
                            {
                                Id = x.Id,
                                FarmId = x.FarmId,
                                FarmName = x.Name
                            })
                        };

            var products = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var createdByIds = products.Select(x => x.CreatedById).ToArray();
            var updatedByIds = products.Select(x => x.UpdatedById).ToArray();

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

            foreach (var product in products)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == product.CreatedById);
                product.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == product.UpdatedById);
                product.UpdatedBy = updatedBy.DisplayName;
            }

            var result = new BasePageList<ProductProjection>(products)
            {
                TotalResult = filteredNumber,
                TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize)
            };
            return result;
        }

        public async Task<IList<ProductProjection>> GetRelevantsAsync(long id, ProductFilter filter)
        {
            var exist = (from pr in _productRepository.Get(x => x.Id == id)
                         join fp in _farmProductRepository.Table
                         on pr.Id equals fp.ProductId into farmProducts
                         join productCategoryRelation in _productCategoryRelationRepository.Table
                         on pr.Id equals productCategoryRelation.ProductId into categoriesRelation
                         select new ProductProjection
                         {
                             Id = pr.Id,
                             CreatedById = pr.CreatedById,
                             UpdatedById = pr.UpdatedById,
                             ProductCategories = categoriesRelation.Select(x => new ProductCategoryProjection()
                             {
                                 Id = x.ProductCategoryId
                             }),
                             ProductFarms = farmProducts.Select(x => new ProductFarmProjection()
                             {
                                 FarmId = x.FarmId
                             })
                         }).FirstOrDefault();

            var farmIds = exist.ProductFarms.Select(x => x.FarmId);
            var categoryIds = exist.ProductCategories.Select(x => x.Id);

            var farmQuery = from farm in _farmRepository.Get(x => farmIds.Contains(x.Id))
                            join farmProduct in _farmProductRepository.Table
                            on farm.Id equals farmProduct.FarmId
                            select new
                            {
                                Id = farmProduct.Id,
                                FarmId = farm.Id,
                                ProductId = farmProduct.ProductId,
                                Name = farm.Name
                            };

            var avatarTypeId = (byte)UserPhotoKind.Avatar;
            var thumbnailTypeId = (byte)ProductPictureType.Thumbnail;
            var relevantProductQuery = (from pr in _productRepository.Get(x => x.Id != exist.Id)
                                        join fp in farmQuery
                                        on pr.Id equals fp.ProductId into farmProducts

                                        join productCategoryRelation in _productCategoryRelationRepository.Table
                                        on pr.Id equals productCategoryRelation.ProductId into categoriesRelation
                                        from categoryRelation in categoriesRelation.DefaultIfEmpty()

                                        join productPic in _productPictureRepository.Get(x => x.PictureType == thumbnailTypeId)
                                        on pr.Id equals productPic.ProductId into pics
                                        join pho in _userPhotoRepository.Get(x => x.TypeId == avatarTypeId)
                                        on pr.CreatedById equals pho.CreatedById into photos
                                        from userPhoto in photos.DefaultIfEmpty()

                                        join prc in _productPriceRepository.Get(x => x.IsCurrent)
                                        on pr.Id equals prc.ProductId into prices
                                        from price in prices.DefaultIfEmpty()

                                        where pr.CreatedById == exist.CreatedById
                                        || categoryIds.Contains(categoryRelation.ProductCategoryId)
                                        || pr.UpdatedById == exist.UpdatedById
                                        select new ProductProjection
                                        {
                                            Id = pr.Id,
                                            Name = pr.Name,
                                            Price = price != null ? price.Price : 0,
                                            CreatedById = pr.CreatedById,
                                            CreatedDate = pr.CreatedDate,
                                            Description = pr.Description,
                                            UpdatedById = pr.UpdatedById,
                                            UpdatedDate = pr.UpdatedDate,
                                            CreatedByPhotoCode = userPhoto.Code,
                                            Thumbnails = pics.Select(x => new PictureRequestProjection
                                            {
                                                Id = x.PictureId
                                            }),
                                            ProductFarms = farmProducts.Select(x => new ProductFarmProjection
                                            {
                                                Id = x.Id,
                                                FarmId = x.FarmId,
                                                FarmName = x.Name
                                            })
                                        });

            var relevantProducts = await relevantProductQuery
                .OrderByDescending(x => x.CreatedDate)
                .Skip(filter.PageSize * (filter.Page - 1))
                .Take(filter.PageSize).ToListAsync();

            var createdByIds = relevantProducts.Select(x => x.CreatedById).ToArray();
            var updatedByIds = relevantProducts.Select(x => x.UpdatedById).ToArray();

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

            foreach (var product in relevantProducts)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == product.CreatedById);
                product.CreatedBy = createdBy.DisplayName;

                var updatedBy = updatedByUsers.FirstOrDefault(x => x.Id == product.UpdatedById);
                product.UpdatedBy = updatedBy.DisplayName;
            }

            return relevantProducts;
        }

        public async Task<long> CreateAsync(ProductProjection product)
        {
            var modifiedDate = DateTimeOffset.UtcNow;
            var newProduct = new Product()
            {
                CreatedById = product.CreatedById,
                UpdatedById = product.UpdatedById,
                CreatedDate = modifiedDate,
                UpdatedDate = modifiedDate,
                Description = product.Description,
                Name = product.Name,
            };

            var id = await _productRepository.AddWithInt64EntityAsync(newProduct);
            if (id > 0)
            {
                foreach (var category in product.ProductCategories)
                {
                    _productCategoryRelationRepository.Add(new ProductCategoryRelation()
                    {
                        ProductCategoryId = category.Id,
                        ProductId = id
                    });
                }

                foreach (var farm in product.ProductFarms)
                {
                    _farmProductRepository.Add(new FarmProduct()
                    {
                        FarmId = farm.FarmId,
                        ProductId = id,
                        IsLinked = true,
                        LinkedById = product.CreatedById,
                        LinkedDate = modifiedDate
                    });
                }

                _productPriceRepository.Add(new ProductPrice()
                {
                    PricedDate = modifiedDate,
                    ProductId = id,
                    Price = product.Price,
                    IsCurrent = true
                });

                var index = 0;
                foreach (var picture in product.Thumbnails)
                {
                    var thumbnail = ImageUtil.EncodeJavascriptBase64(picture.Base64Data);
                    var pictureData = Convert.FromBase64String(thumbnail);
                    var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                    {
                        CreatedById = product.UpdatedById,
                        CreatedDate = modifiedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = product.UpdatedById,
                        UpdatedDate = modifiedDate,
                        BinaryData = pictureData
                    });

                    var productPictureType = index == 0 ? (int)ProductPictureType.Thumbnail : (int)ProductPictureType.Secondary;
                    _productPictureRepository.Add(new ProductPicture()
                    {
                        ProductId = id,
                        PictureId = pictureId,
                        PictureType = productPictureType
                    });
                    index += 1;
                }
            }

            return id;
        }

        public async Task<ProductProjection> UpdateAsync(ProductProjection request)
        {
            var updatedDate = DateTimeOffset.UtcNow;
            var product = _productRepository.FirstOrDefault(x => x.Id == request.Id);
            product.Description = request.Description;
            product.Name = request.Name;
            product.UpdatedById = request.UpdatedById;
            product.UpdatedDate = updatedDate;

            var index = 0;
            foreach (var picture in request.Thumbnails)
            {
                if (!string.IsNullOrEmpty(picture.Base64Data))
                {
                    var productThumbnails = _productPictureRepository
                        .Get(x => x.ProductId == request.Id)
                        .AsEnumerable();

                    if (productThumbnails.Any())
                    {
                        var pictureIds = productThumbnails.Select(x => x.PictureId).ToList();
                        await _productPictureRepository.DeleteAsync(productThumbnails.AsQueryable());

                        var currentThumbnails = _pictureRepository.Get(x => pictureIds.Contains(x.Id));
                        await _pictureRepository.DeleteAsync(currentThumbnails);
                    }

                    var pictureData = Convert.FromBase64String(picture.Base64Data);
                    var pictureId = _pictureRepository.AddWithInt64Entity(new Picture()
                    {
                        CreatedById = request.UpdatedById,
                        CreatedDate = updatedDate,
                        FileName = picture.FileName,
                        MimeType = picture.ContentType,
                        UpdatedById = request.UpdatedById,
                        UpdatedDate = updatedDate,
                        BinaryData = pictureData
                    });

                    var productPictureType = index == 0 ? (int)ProductPictureType.Thumbnail : (int)ProductPictureType.Secondary;
                    _productPictureRepository.Add(new ProductPicture()
                    {
                        ProductId = product.Id,
                        PictureId = pictureId,
                        PictureType = productPictureType
                    });
                    index += 1;
                }

                _productRepository.Update(product);
            }

            request.UpdatedDate = product.UpdatedDate;
            return request;
        }
    }
}