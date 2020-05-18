﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Coco.Entities.Domain.Identity
{
    public class UserAttribute
    {
        public int Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        [Required]
        public bool IsDisabled { get; set; }
    }
}
