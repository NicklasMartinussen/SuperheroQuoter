using System.Text.Json.Serialization;

namespace SuperheroQuoter.Server.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Quote> Quotes { get; }
    }
}
