﻿using Camino.Service.Projections.Content;
using System.Threading.Tasks;

namespace Camino.Service.Business.Media.Contracts
{
    public interface IPictureBusiness
    {
        PictureProjection Find(long id);
        Task<PictureProjection> GetPicture(long id);
    }
}