using Camino.Data.Enums;
using System;

namespace Module.Api.Feed.Models
{
    public class FeedModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long PictureId { get; set; }
        public long CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public int FeedType { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string CreatedByIdentityId { get; set; }
    }
}
