using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{

    public class ConvertAllPatchesToSolutionsRequest 
    {
    }

    class Program
    {

        //JsonTextReader reader = new JsonTextReader(new StringReader(sInputParameter));

        static void Main(string[] args)
        {
            var serializer = Newtonsoft.Json.JsonSerializer.Create();
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            JsonTextWriter writer = new JsonTextWriter(sw);
            var aa = new ConvertAllPatchesToSolutionsRequest();
            

            serializer.Serialize(writer, aa);
            var aaa = sb.ToString();

        }
    }
}
