﻿using Coco.Entities.Constant;
using Coco.Entities.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.UserDAL.MappingConfigs.AuthMappings
{
    public class UserRoleMappingConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole), TableSchemaConst.AUTH);
            builder.HasKey(c => new { c.UserId, c.RoleId });
        }
    }
}