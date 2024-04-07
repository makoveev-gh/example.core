using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Example.Core.Contracts.Data;

[DataContract]
public class DateData
{
    [DataMember]
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [DataMember]
    [JsonPropertyName("date")]
    public required DateOnly Date { get; init; }
}
