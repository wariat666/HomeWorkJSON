using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using HomeWorkJSON.ApiObjects;

namespace HomeWorkJSON
{
    internal class ApiClient
    {
        public RestClient Client { get; set; }
        public ApiClient(string apiUri)
        {
            Client = new RestClient(apiUri);
        }
        
        public async Task<RestResponse> GetResponseAsync(string path)
        {
            RestRequest request = new RestRequest("api.php?amount=1");
            return await Client.GetAsync(request);
        }

        public TriviaApiObjectList GetTriviaApiObjectFromJsonResponse(string jsonResponse)
        {
            return JsonConvert.DeserializeObject<TriviaApiObjectList>(jsonResponse);
        }
    }
}
