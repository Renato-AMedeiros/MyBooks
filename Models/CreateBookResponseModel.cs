using Newtonsoft.Json;

namespace my_library_cosmos_db.Models
{
    public class CreateBookResponseModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("libraryId")]
        public string LibraryId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string? Author { get; set; }

        [JsonProperty("year")]
        public int? Year { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("numberPages")]
        public int? NumberPages { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }
    }
}
