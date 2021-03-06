﻿using Camino.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Camino.Core.Utils
{
    public static class EnumUtil
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>() where TEnum : struct, IConvertible, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(x => new SelectListItem
            {
                Value = x.ToString("d", null),
                Text = x.ToString()
            });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<TEnum>(TEnum selected) where TEnum : struct, IConvertible, IFormattable
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(x => new SelectListItem
            {
                Selected = x.Equals(selected),
                Value = x.ToString("d", null),
                Text = x.ToString()
            });
        }

        public static TEnum FilterEnumByName<TEnum>(string filter) where TEnum : struct, IConvertible, IFormattable
        {
            var data = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            foreach(var item in data)
            {
                if (filter.StartsWith(item.ToString()))
                {
                    return item;
                }
            }

            return default;
        }

        public static IEnumerable<SelectOption> EnumToSelectList<TEnum>(string selectedId = "")
        {
            var enumsData = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var result = enumsData.Select(e => new SelectOption()
            {
                Id = Convert.ChangeType(e, typeof(int)).ToString(),
                Text = e.ToString()
            });

            return result;
        }

        public static string GetDescription<TEnum>(TEnum value)
        {
            var type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute != null)
                    {
                        return attribute.Description;
                    }
                }
            }
            return value.ToString();
        }
    }
}
