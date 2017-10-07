using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WillCrypto
{
    class Request
    {
        public async Task<String> Get(string url)
        {
            Uri uri = new Uri(url);
            WebRequest webRequest = WebRequest.Create(uri);
            WebResponse response = await webRequest.GetResponseAsync();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            return streamReader.ReadToEnd();
        }

    }
}
