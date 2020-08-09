﻿using System.Collections.Generic;

namespace Camino.Business.Dtos.General
{
    public class PageListDto<T> where T : class
    {
        public PageListDto(IEnumerable<T> collections)
        {
            Collections = collections;
            Filter = new BaseFilterDto();
        }

        public BaseFilterDto Filter { get; set; }
        public int TotalResult { get; set; }
        public int TotalPage { get; set; }
        public IEnumerable<T> Collections { get; set; }
    }
}
