﻿using Module.Api.Auth.Models;
using Camino.Service.Data.Identity;
using Camino.Framework.Models;
using System.Threading.Tasks;

namespace  Module.Api.Auth.GraphQL.Resolvers.Contracts
{
    public interface IUserResolver
    {
        FullUserInfoModel GetLoggedUser();
        Task<FullUserInfoModel> GetFullUserInfoAsync(FindUserModel criterias);
        Task<UpdatePerItemModel> UpdateUserInfoItemAsync(UpdatePerItemModel criterias);
        Task<ICommonResult> SignoutAsync();
        Task<UserIdentifierUpdateDto> UpdateIdentifierAsync(UserIdentifierUpdateDto criterias);
        Task<Camino.Framework.Models.UserTokenResult> UpdatePasswordAsync(UserPasswordUpdateDto criterias);

        Task<Camino.Framework.Models.UserTokenResult> SigninAsync(SigninModel criterias);
        Task<ICommonResult> SignupAsync(SignupModel criterias);
        Task<ICommonResult> ActiveAsync(ActiveUserModel criterias);
        Task<ICommonResult> ForgotPasswordAsync(ForgotPasswordModel criterias);
        Task<ICommonResult> ResetPasswordAsync(ResetPasswordModel criterias);
    }
}
