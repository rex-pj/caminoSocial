﻿using HotChocolate.Types;
using Module.Api.Content.Models;

namespace Module.Api.Content.GraphQL.InputTypes
{
    public class SelectFilterInputType : InputObjectType<SelectFilterModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SelectFilterModel> descriptor)
        {
            descriptor.Field(x => x.Query).Type<StringType>();
            descriptor.Field(x => x.CurrentId).Type<LongType>();
            descriptor.Field(x => x.IsParentOnly).Type<BooleanType>();
        }
    }
}
