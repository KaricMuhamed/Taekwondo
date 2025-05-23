using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaekwondoBackend.Entities
{
    [Table("grupe")]
    public class Grupa
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("naziv")]
        public required string Naziv { get; set; }

        [Column("opis")]
        public required string Opis { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<GrupaUcenik> GrupaUcenici { get; set; } = new List<GrupaUcenik>();

        [JsonIgnore]
        public virtual ICollection<TreningGrupa> TreningGrupe { get; set; } = new List<TreningGrupa>();
    }
}
