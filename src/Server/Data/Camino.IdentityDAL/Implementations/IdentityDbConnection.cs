﻿using Camino.Data.Contracts;
using Camino.IdentityDAL.Entities;
using LinqToDB;
using LinqToDB.Configuration;

namespace Camino.IdentityDAL.Implementations
{
    public class IdentityDbConnection : BaseDataConnection
    {
        public IdentityDbConnection(LinqToDbConnectionOptions<IdentityDbConnection> options) : base(options)
        {
        }

        public ITable<User> User { get; set; }
        public ITable<UserInfo> UserInfo { get; set; }
        public ITable<Gender> Gender { get; set; }
        public ITable<Status> Status { get; set; }
        public ITable<Role> Role { get; set; }
        public ITable<RoleClaim> RoleClaim { get; set; }
        public ITable<Country> Country { get; set; }
        public ITable<UserAttribute> UserAttribute { get; set; }
        public ITable<AuthorizationPolicy> AuthorizationPolicy { get; set; }
        public ITable<UserAuthorizationPolicy> UserAuthorizationPolicy { get; set; }
        public ITable<RoleAuthorizationPolicy> RoleAuthorizationPolicy { get; set; }
    }
}
