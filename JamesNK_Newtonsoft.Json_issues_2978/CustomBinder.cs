using Newtonsoft.Json.Serialization;

namespace JamesNK_Newtonsoft.Json_issues_2978;

internal class CustomBinder : Newtonsoft.Json.Serialization.DefaultSerializationBinder
{
    internal static readonly ISerializationBinder Instance = new CustomBinder();

    private static readonly Dictionary<Type, string> bindNames = new()
    {
        { typeof(RadioQuestion), "radio" },
        { typeof(SingleLinePlainTextQuestion), "single-plain" }
    };
    private static readonly Dictionary<string, Type> bindTypes = bindNames.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public override void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        assemblyName = null;
        if (!bindNames.TryGetValue(serializedType, out typeName))
        {
            base.BindToName(serializedType, out assemblyName, out typeName);
        }
    }

    public override Type BindToType(string? assemblyName, string typeName) => 
        assemblyName == null && bindTypes.TryGetValue(typeName, out var type) ?
            type :
            base.BindToType(assemblyName, typeName);
}