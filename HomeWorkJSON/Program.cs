using RestSharp;
using Newtonsoft.Json;

var client = new RestClient("https://restcountries.com/v3.1");

var request = new RestRequest("all");
var response = await client.GetAsync(request);


List<Country> countryList = JsonConvert.DeserializeObject<List<Country>>(response.Content);

Console.WriteLine("");



class Country
{
    public Name name { get; set; }
    public string region { get; set; }
    public string subregion { get; set; }
    public int population { get; set; }

    public class Name
    {
        [JsonProperty("common")]
        public string Common { get; set; }
        [JsonProperty("official")]
        public string Official { get; set; }
    }
}

