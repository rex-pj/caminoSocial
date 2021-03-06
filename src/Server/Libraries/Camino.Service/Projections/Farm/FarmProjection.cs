﻿using Camino.Service.Projections.Media;
using System;
using System.Collections.Generic;

namespace Camino.Service.Projections.Farm
{
    public class FarmProjection
    {
        public FarmProjection()
        {
            Pictures = new List<PictureRequestProjection>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public long FarmTypeId { get; set; }
        public string FarmTypeName { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string Address { get; set; }
        public IEnumerable<PictureRequestProjection> Pictures { get; set; }
    }
}
