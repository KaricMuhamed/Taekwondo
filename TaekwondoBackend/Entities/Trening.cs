using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaekwondoBackend.Entities
{
    [Table("treninzi")]
    public class Trening
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("naziv")]
        public string Naziv { get; set; }

        [Column("opis")]
        public string Opis { get; set; }

        [Required]
        [Column("datum")]
        public DateTime Datum { get; set; }

        [Required]
        [Column("vrijeme_od")]
        public TimeSpan VrijemeOd { get; set; }

        [Required]
        [Column("vrijeme_do")]
        public TimeSpan VrijemeDo { get; set; }

        [StringLength(255)]
        [Column("lokacija")]
        public string Lokacija { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<TreningTrener> TreningTreneri { get; set; } = new List<TreningTrener>();

        [JsonIgnore]
        public virtual ICollection<TreningGrupa> TreningGrupe { get; set; } = new List<TreningGrupa>();
    }
}
