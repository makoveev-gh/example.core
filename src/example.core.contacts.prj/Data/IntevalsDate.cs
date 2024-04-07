using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Example.Core.Contracts.Data;

[DataContract]
public class IntervalsDate
{
    [DataMember]
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [DataMember]
    [JsonPropertyName("start_date")]
    public required DateOnly StartDate { get; init; }

    [DataMember]
    [JsonPropertyName("end_date")]
    public required DateOnly EndDate { get; init; }
}
