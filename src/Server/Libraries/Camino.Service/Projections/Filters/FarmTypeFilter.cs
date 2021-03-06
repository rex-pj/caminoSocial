﻿using System;

namespace Camino.Service.Projections.Filters
{
    public class FarmTypeFilter : BaseFilter
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
    }
}
