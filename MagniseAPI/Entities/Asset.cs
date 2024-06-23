using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagniseAPI.Entities
{
    [Table("Assets")]
    public class Asset
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Symbol { get; set; }

        public Asset(Guid id, string symbol) 
        {
            Id = id;
            Symbol = symbol;
        }
    }
}
