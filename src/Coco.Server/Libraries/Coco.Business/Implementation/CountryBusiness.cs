﻿using Coco.Business.Contracts;
using Coco.Data.Contracts;
using Coco.Data.Entities.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Business.Implementation
{
    public class CountryBusiness : ICountryBusiness
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryBusiness(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<Country> GetAll()
        {
            return _countryRepository.Get().ToList();
        }
    }
}
