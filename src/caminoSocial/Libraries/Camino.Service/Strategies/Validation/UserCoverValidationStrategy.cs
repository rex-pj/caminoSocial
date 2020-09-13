﻿using System;
using System.Collections.Generic;
using System.Linq;
using Camino.Service.Strategies.Validation.Contracts;
using Camino.Core.Exceptions;
using Camino.Core.Utils;
using Camino.Service.Projections.Error;
using Camino.Service.Projections.Request;

namespace Camino.Service.Strategies.Validation
{
    public class UserCoverValidationStrategy : IValidationStrategy
    {
        public IEnumerable<BaseErrorResult> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            var data = value as UserPhotoUpdateRequest;

            if (string.IsNullOrEmpty(data.PhotoUrl))
            {
                Errors = GetErrors(new PhotoSizeInvalidException(nameof(data.PhotoUrl)));
            }

            var image = ImageUtil.Base64ToImage(data.PhotoUrl);
            if (image.Width < 1000 || image.Height < 300)
            {
                Errors = GetErrors(new PhotoSizeInvalidException(nameof(data.PhotoUrl)));
            }

            return Errors == null || !Errors.Any();
        }

        public IEnumerable<BaseErrorResult> GetErrors(Exception e)
        {
            yield return new BaseErrorResult
            {
                Message = e.Message
            };
        }
    }
}
