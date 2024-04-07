using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Example.Core.Contracts.Data;

[DataContract]
public class ClientWithContactData
{
    [DataMember]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [DataMember]
    [JsonPropertyName("contact_count")]
    public required int ContactCount { get; init; }
}
