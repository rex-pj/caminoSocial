﻿using Camino.Business.Dtos.Identity;
using Camino.Data.Entities.Identity;
using System;
using System.Linq.Expressions;

namespace Camino.Business.AutoMap
{
    public static class UserExpressionMapping
    {
        public static Expression<Func<User, UserDto>> UserModelSelector { get; } = user => new UserDto
        {
            DisplayName = user.DisplayName,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            UserName = user.UserName,
            UpdatedDate = user.UpdatedDate,
            CreatedDate = user.CreatedDate,
            UpdatedById = user.UpdatedById,
            CreatedById = user.CreatedById,
            StatusId = user.StatusId,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            SecurityStamp = user.SecurityStamp,
            Id = user.Id,
            IsEmailConfirmed = user.IsEmailConfirmed,
            GenderId = user.UserInfo.GenderId,
            Address = user.UserInfo.Address,
            BirthDate = user.UserInfo.BirthDate,
            CountryId = user.UserInfo.CountryId,
            PhoneNumber = user.UserInfo.PhoneNumber,
        };

        public static Expression<Func<User, UserFullDto>> FullUserModelSelector { get; } = user => new UserFullDto
        {
            CreatedDate = user.CreatedDate,
            DisplayName = user.DisplayName,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.UserInfo.PhoneNumber,
            Description = user.UserInfo.Description,
            Address = user.UserInfo.Address,
            BirthDate = user.UserInfo.BirthDate,
            GenderId = user.UserInfo.GenderId,
            GenderLabel = user.UserInfo.Gender.Name,
            StatusId = user.StatusId,
            StatusLabel = user.Status.Name,
            Id = user.Id,
            CountryId = user.UserInfo.CountryId,
            CountryCode = user.UserInfo.Country.Code,
            CountryName = user.UserInfo.Country.Name
        };
    }
}
