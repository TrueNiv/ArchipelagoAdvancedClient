namespace ArchipelagoAdvancedClient.Business.APConnector;

public partial class LocationDTO
{
    public string? Name { get; set; }
    public long Id { get; set; }
    public string? Item {get; set;}
    public ItemHintState HintState { get; set; }

    public LocationDTO(string? name, long id, ItemHintState hintState, string item = "")
    {
        Name = name;
        Id = id;
        HintState = hintState;
        Item = item;
    }
}