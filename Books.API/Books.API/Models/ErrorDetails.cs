using Newtonsoft.Json;

namespace Books.API.Models
{
    public class ErrorDetails
    {
        public int Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
