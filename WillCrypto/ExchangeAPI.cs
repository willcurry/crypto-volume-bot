using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WillCrypto
{
    internal class ExchangeAPI
    {
        private string _url;

        public ExchangeAPI(String url)
        {
            _url = url;
        }

        public async Task<List<Coin>> GetAllCoins()
        {
            String jsonData = await new Request().Get(_url + "getmarketsummaries");
            List <Coin> result = JsonConvert.DeserializeObject<Result>(jsonData).result;
            return result;
        }

        public async Task<Coin> GetCoinInfo(string coin)
        {
            String jsonData;
            if (coin.Contains("-"))
            {
                jsonData = await new Request().Get(_url + "getmarketsummary?market=" + coin);
            } else
            {
                jsonData = await new Request().Get(_url + "getmarketsummary?market=btc-" + coin);
            }
            List <Coin> result = JsonConvert.DeserializeObject<Result>(jsonData).result;
            return result.First();
        }
    }

    internal class Result 
    {
        public List<Coin> result { get; set; }
    }
}