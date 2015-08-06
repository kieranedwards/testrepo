using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Exerp.Api.Offline.SampleData
{
    internal class SampleDataProvider : ISampleDataProvider
    {
        private const string ResourceName = "Exerp.Api.Offline.SampleData.DataSet1.json";

        public dynamic GetDataSet()
        {
            return JObject.Parse(LoadSampleJsonFile());
        }

        private string LoadSampleJsonFile()
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null) return null;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
