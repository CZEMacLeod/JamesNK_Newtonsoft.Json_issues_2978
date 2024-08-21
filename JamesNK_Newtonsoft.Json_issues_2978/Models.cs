using Newtonsoft.Json;

namespace JamesNK_Newtonsoft.Json_issues_2978;

public abstract class Question
{
    //[JsonProperty("$type")]
    //public abstract string Type { get; }
    [JsonProperty("questionText")]
    public required string QuestionText { get; set; } = null!;

    private string? SimpleType
    {
        get
        {
            CustomBinder.Instance.BindToName(this.GetType(), out _, out string? name);
            return name;
        }
    }

    public override string ToString() =>
        $"{SimpleType}: {JsonConvert.SerializeObject(this)}";
}

public class SingleLinePlainTextQuestion : Question
{
    public SingleLinePlainTextQuestion()
    {
    }
    //public override string Type => "single-plain";
    [JsonProperty("placeholder")]
    public string Placeholder { get; set; } = null!;
}

public class RadioQuestion : Question
{
    public RadioQuestion()
    {
    }
    //public override string Type => "radio";
    [JsonProperty("options")]
    public required ICollection<string> Options { get; set; }
}
