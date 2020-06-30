using SGDb.Common.Data.Models;

namespace SGDb.Statistics.Data.Models
{
    public class Statistics : BaseEntity<uint>
    {
        public uint TotalGamesCount { get; set; }

        public uint TotalPublishersCount { get; set; }

        public uint TotalGenresCount { get; set; }
    }
}