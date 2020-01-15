﻿using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.ResultTypes
{
    public class RegisterResultType: ObjectGraphType<ApiResult>
    {
        public RegisterResultType()
        {
            Field(x => x.IsSucceed, type: typeof(BooleanGraphType));
            Field(x => x.Errors, type: typeof(ListGraphType<ApiErrorType>));
        }
    }
}
