﻿using Coco.Entities.Model.Account;
using Coco.Entities.Model.General;
using System.Threading.Tasks;

namespace Coco.Business.Contracts
{
    public interface IAccountBusiness
    {
        long Add(UserModel user);
        UserModel Find(long id);
        UserLoggedInModel GetLoggedIn(long id);
        Task<UserModel> FindUserByEmail(string email, bool includeInActived = false);
        Task<UserModel> FindUserByUsername(string username, bool includeInActived = false);
        void Delete(long id);
        Task<UserModel> UpdateAsync(UserModel user);
        Task<UserModel> FindByIdAsync(long id);
        Task<UserFullModel> GetFullByIdAsync(long id);
        Task<UpdatePerItem> UpdateInfoItemAsync(UpdatePerItem model);
    }
}
