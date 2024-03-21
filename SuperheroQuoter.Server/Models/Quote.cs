using System.Text.Json.Serialization;

namespace SuperheroQuoter.Server.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; }

        [JsonIgnore]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
