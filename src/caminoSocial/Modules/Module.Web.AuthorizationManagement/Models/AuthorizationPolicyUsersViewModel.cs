﻿using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Web.AuthorizationManagement.Models
{
    public class AuthorizationPolicyUsersViewModel : PageListViewModel<UserViewModel>
    {
        public AuthorizationPolicyUsersViewModel(IEnumerable<UserViewModel> collections):base(collections)
        {
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
    }
}
