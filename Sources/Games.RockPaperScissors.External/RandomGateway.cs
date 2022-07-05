using System;
using Flurl;
using Flurl.Http;
using Games.RockPaperScissors.Domain;
using LanguageExt.Common;
using Newtonsoft.Json;

namespace Games.RockPaperScissors.External
{
    public class RandomGateway : IRandomGateway
    {
        private readonly string apiUri;

        public RandomGateway(string apiUri)
        {
            this.apiUri = apiUri;
        }

        /*
         Possible issues - too long response if we hit out of range multiply.
         Solutions:
         1. Add timer to track total time of all responses and break with failure if we are in timeout.
         2. Use better API of random generation, that allows to use range of expected random numbers.
        */
        /// <inheritdoc />
        public Result<byte> GetNext(int minValue, int maxValue)
        {
            byte? value = null;
            while (value is null)
            {
                Url resource = $"{this.apiUri}";
                IFlurlResponse layoutsResponse = resource
                    .AllowAnyHttpStatus()
                    .GetAsync()
                    .Result;

                if (layoutsResponse.StatusCode != 200)
                {
                    return new Result<byte>(
                        new Exception($"Unexpected status code: {layoutsResponse.StatusCode}"));
                }

                string content = layoutsResponse.ResponseMessage.Content.ReadAsStringAsync().Result;
                var randomValueResponse = JsonConvert.DeserializeObject<RandomValueResponse>(content);
                value = randomValueResponse.RandomValue;
                if (value <= minValue || value >= maxValue)
                {
                    value = null;
                }
            }

            return value.Value;
        }
    }
}