using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Example.Core.Contracts.Data;

[DataContract]
public class ContactData
{
    [DataMember]
    [JsonPropertyName("contact_type")]
    public required string ContactType { get; init; }

    [DataMember]
    [JsonPropertyName("contact_value ")]
    public required string ContactValue { get; init; }
}
