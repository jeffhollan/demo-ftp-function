using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Text;

namespace AzureFunction { 
    public class CSVtoJson {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            string data = await req.Content.ReadAsStringAsync();

            JObject resultSet = JObject.FromObject(new { rows = new JArray() });
            string[] csvLines = data.Split(new char[] { '\n', '\r' });
            var headers = csvLines[0].Split(',').ToList<string>();

            foreach (var line in csvLines.Skip(1))
            {
                if (line == null || line == "")
                    continue;
                var lineObject = new JObject();
                var lineAttr = line.Split(',');
                for (int x = 0; x < headers.Count; x++)
                {
                    lineObject[headers[x]] = lineAttr[x];
                }
                ((JArray)resultSet["rows"]).Add(lineObject);
            }
            log.Info(resultSet.ToString(Formatting.Indented));
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(resultSet.ToString(), Encoding.UTF8, "application/json");
            return response;
        }
    }
}