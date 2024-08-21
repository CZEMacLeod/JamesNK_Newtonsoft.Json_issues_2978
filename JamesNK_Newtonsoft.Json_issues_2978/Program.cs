using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JamesNK_Newtonsoft.Json_issues_2978;

internal class Program
{
    static void Main(string[] args)
    {
        var qs = new List<Question>()
        {
            new SingleLinePlainTextQuestion()
            {
                QuestionText="Single-line plain text",
                Placeholder="Test placeholder"
            },
            new RadioQuestion()
            {
                QuestionText="Single selection",
                Options = new List<string>()
                {
                    "Option 0",
                    "Option 1",
                    "Option 2"
                }
            }
        };

        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            SerializationBinder = CustomBinder.Instance,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented
        };

        var json = JsonConvert.SerializeObject(qs, settings);

        Console.WriteLine(json);
        var questions = JsonConvert.DeserializeObject<ICollection<Question>>(json, settings);

        foreach (var q in questions!)
        {
            Console.WriteLine(q.ToString());
        }

        // Fixup data in 'old' format
        using var oldJsonStream = System.IO.File.OpenText("test.json");
        using var jr = new JsonTextReader(oldJsonStream);
        var jarr = (JArray)JToken.ReadFrom(jr);
        foreach (var jo in jarr)
        {
            if (jo.First is JProperty prp && prp.Name == "type")
            {
                prp.Replace(new JProperty("$type", prp.Value));
            }
        }

        var oldJson = jarr.ToString();

        var oldQuestions = JsonConvert.DeserializeObject<ICollection<Question>>(oldJson, settings);

        foreach (var q in oldQuestions!)
        {
            Console.WriteLine(q.ToString());
        }
    }
}
