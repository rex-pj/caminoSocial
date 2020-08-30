﻿using Camino.DAL.Entities;
using Camino.Data.Contracts;
using Camino.Service.Business.Media.Contracts;
using Camino.Service.Data.Content;
using System.Linq;

namespace Camino.Service.Business.Media
{
    public class PictureBusiness : IPictureBusiness
    {
        private readonly IRepository<Picture> _pictureRepository;

        public PictureBusiness(IRepository<Picture> pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public PictureProjection Find(long id)
        {
            var picture = _pictureRepository.Get(x => x.Id == id).Select(pic => new PictureProjection
            {
                CreatedDate = pic.CreatedDate,
                CreatedById = pic.CreatedById,
                Id = pic.Id,
                FileName = pic.FileName,
                UpdatedById = pic.UpdatedById,
                UpdatedDate = pic.UpdatedDate,
                Alt = pic.Alt,
                BinaryData = pic.BinaryData,
                MimeType = pic.MimeType,
                RelativePath = pic.RelativePath,
                Title = pic.Title
            }).FirstOrDefault();

            if (picture == null)
            {
                return null;
            }

            return picture;
        }
    }
}
