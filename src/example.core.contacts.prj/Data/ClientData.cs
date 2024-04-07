using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Example.Core.Contracts.Data;

[DataContract]
public class ClientData
{
    [DataMember]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [DataMember]
    [JsonPropertyName("contacts")]
    public required List<ContactData> Contacts { get; init; }
}
