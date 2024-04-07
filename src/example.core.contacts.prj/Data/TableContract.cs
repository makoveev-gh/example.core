using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Example.Core.Contracts.Data;


[DataContract]
public record TableContract
{
    [DataMember]
    [JsonPropertyName("code")]
    public required int Code { get; init; }

    [DataMember]
    [JsonPropertyName("value")]
    public required string Value { get; init; }
}