using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HomeWorkJSON.ApiObjects
{

    class TriviaApiObjectList
    {
        [JsonProperty("response_code")]
        public int ResponseCode { get; set; }
        [JsonProperty("results")]
        public List<TriviaApiObject> ApiObjectList { get; set; }
    }

    internal class TriviaApiObject
    {
        

        [JsonProperty("category")]
         public string Category { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }
        [JsonProperty("question")]
        public string Question { get; set; }
        [JsonProperty("correct_answer")]
        public string CorrectAnswer { get; set; }
        [JsonProperty("incorrect_answers")]
        public List<string> IncorrectAnswers { get; set; }
        public List<string> AllAnswers { get; set; }

        public void PopulateAllAnswersList()
        {
            AllAnswers = new List<string>();
            AllAnswers.Add(CorrectAnswer);
            IncorrectAnswers.ForEach(x => AllAnswers.Add(x));
        }
    }

    static class ListExtension
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
