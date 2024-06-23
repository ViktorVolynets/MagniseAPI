namespace MagniseAPI.Models;

public class AssetDto
{
    public Guid Id { get; set; }
    public string Symbol { get; set; }
  
    public AssetDto(Guid id, string symbol)
    {
        Id = id;
        Symbol = symbol;
    }
}
