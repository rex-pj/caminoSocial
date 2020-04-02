﻿using Api.Public.Models;
using HotChocolate.Types;

namespace Api.Public.GraphQLTypes.InputTypes
{
    public class FindUserInputType : InputObjectType<FindUserModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<FindUserModel> descriptor)
        {
            descriptor.Field(x => x.UserId).Type<NonNullType<StringType>>();
        }
    }
}
