﻿using Coco.Api.Framework.GraphQLTypes.Redefines;
using Coco.Api.Framework.Models;
using HotChocolate.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class UpdatePerItemInputType : InputObjectType<UpdatePerItemModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePerItemModel> descriptor)
        {
            descriptor.Field(x => x.Key).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.PropertyName).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Value).Type<NonNullType<DynamicType>>();
            descriptor.Field(x => x.Type).Type<NonNullType<IntType>>();
            descriptor.Field(x => x.CanEdit).Type<NonNullType<BooleanType>>();
        }
    }
}
