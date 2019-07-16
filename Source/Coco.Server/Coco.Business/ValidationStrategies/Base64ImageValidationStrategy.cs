﻿using System;
using System.Collections.Generic;
using Coco.Business.ValidationStrategies.Interfaces;
using Coco.Business.ValidationStrategies.Models;
using Coco.Common.Helper;

namespace Coco.Business.ValidationStrategies
{
    public class Base64ImageValidationStrategy : IValidationStrategy
    {
        public IEnumerable<ErrorObject> Errors { get; set; }

        public bool IsValid<T>(T value)
        {
            try
            {
                if (value == null)
                {
                    return false;
                }
                ImageHelper.Base64ToImage(value.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
