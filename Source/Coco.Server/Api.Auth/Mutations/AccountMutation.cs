﻿using Api.Identity.Resolvers;
using Api.Identity.GraphQLTypes.InputTypes;
using Api.Identity.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;
using Coco.Entities.Model.General;

namespace Api.Identity.Mutations
{
    public class AccountMutation : ObjectGraphType
    {
        public AccountMutation(AccountResolver accountResolver)
        {
            FieldAsync(typeof(ApiResultType), "updateUserInfo",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UserInfoUpdateInputType>> { Name = "userInfo" }),
                resolve: async context => await accountResolver.UpdateUserInfoAsync(context));

            FieldAsync(typeof(ApiResultType<ItemUpdatedResultType, UpdatePerItemModel>), "updateUserInfoItem",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdatePerItemInputType>> { Name = "criterias" }),
                resolve: async context => await accountResolver.UpdateUserInfoItemAsync(context));

            FieldAsync(typeof(ApiResultType<AvatarUpdatedResultType, UpdateAvatarModel>), "updateAvatar",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<UpdateAvatarInputType>> { Name = "criterias" }),
                resolve: async context => await accountResolver.UpdateAvatarAsync(context));
        }
    }
}
