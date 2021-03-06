﻿using Camino.Service.Projections.Identity;
using System.Collections.Generic;

namespace Camino.Service.Projections.PageList
{
    public class AuthorizationPolicyUsersPageList : BasePageList<UserProjection>
    {
        public AuthorizationPolicyUsersPageList(IEnumerable<UserProjection> collections) : base(collections)
        {

        }

        public AuthorizationPolicyUsersPageList() : base(new List<UserProjection>())
        {

        }


        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
