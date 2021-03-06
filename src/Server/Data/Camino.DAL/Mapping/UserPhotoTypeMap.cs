﻿using Camino.Core.Constants;
using Camino.DAL.Entities;
using Camino.Data.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.DAL.Mapping
{
    public class UserPhotoTypeMap : EntityMapBuilder<UserPhotoType>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<UserPhotoType>()
                .HasTableName(nameof(UserPhotoType))
                .HasSchemaName(TableSchemaConst.DBO)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.UserPhotos, (userPhotoType, userPhotos) => userPhotoType.Id == userPhotos.TypeId);
        }
    }
}
