using System.Text.Json.Serialization;

namespace MagniseAPI.Models
{
    public class HistoricalAssetsResponse
    {
        [JsonPropertyName("paging")]
        public Paging Paging { get; set; }

        [JsonPropertyName("data")]
        public List<AssetForCreationDto> Data { get; set; }
    }

    public class Paging
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("items")]
        public int Items { get; set; }
    }

    public class AssetForCreationDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("tickSize")]
        public double TickSize { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("baseCurrency")]
        public string BaseCurrency { get; set; }

        [JsonPropertyName("mappings")]
        public Mappings Mappings { get; set; }
    }

    public class Mappings
    {
        [JsonPropertyName("cryptoquote")]
        public Cryptoquote Cryptoquote { get; set; }

        [JsonPropertyName("simulation")]
        public Simulation Simulation { get; set; }

        [JsonPropertyName("alpaca")]
        public Alpaca Alpaca { get; set; }

        [JsonPropertyName("dxfeed")]
        public Dxfeed Dxfeed { get; set; }

        [JsonPropertyName("active-tick")]
        public ActiveTick ActiveTick { get; set; }

        [JsonPropertyName("oanda")]
        public Oanda Oanda { get; set; }
    }

    public class Cryptoquote
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }
    }

    public class Simulation
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("defaultOrderSize")]
        public int DefaultOrderSize { get; set; }
    }

    public class Alpaca
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("defaultOrderSize")]
        public int DefaultOrderSize { get; set; }
    }

    public class Dxfeed
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("defaultOrderSize")]
        public int DefaultOrderSize { get; set; }
    }

    public class ActiveTick
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("defaultOrderSize")]
        public int DefaultOrderSize { get; set; }
    }

    public class Oanda
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("defaultOrderSize")]
        public int DefaultOrderSize { get; set; }
    }
}
