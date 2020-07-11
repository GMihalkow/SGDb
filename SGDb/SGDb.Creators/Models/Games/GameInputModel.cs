using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGDb.Common.Infrastructure.Attributes.Validation;
using SGDb.Creators.Infrastructure;

namespace SGDb.Creators.Models.Games
{
    public class GameInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Url]
        [ValidateImgExtensions(ValidationConstants.AllowedImgExtensions)]
        public string HeaderImageUrl { get; set; }

        [Required]
        [Url]
        [ValidateImgExtensions(ValidationConstants.AllowedImgExtensions)]
        public string BackgroundImageUrl { get; set; }

        [Url]
        public string WebsiteUrl { get; set; }

        public string About { get; set; }

        public uint CreatorId { get; set; }

        public decimal? Price { get; set; }

        public DateTime? ReleasedOn { get; set; }

        public IEnumerable<uint> PublisherIds { get; set; }

        public IEnumerable<uint> GenreIds { get; set; }
    }
}