﻿using System.Collections.Generic;

namespace Camino.Service.Projections.Identity
{
    public class RoleAuthorizationPoliciesProjection
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<AuthorizationPolicyProjection> AuthorizationPolicies { get; set; }
    }
}
