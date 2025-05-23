using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaekwondoBackend.Entities
{
    [Table("trening_treneri")]
    public class TreningTrener
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("trening_id")]
        public Guid TreningId { get; set; }

        [Required]
        [Column("trener_id")]
        public Guid TrenerId { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("TreningId")]
        public virtual Trening Trening { get; set; }

        [ForeignKey("TrenerId")]
        public virtual Korisnik Trener { get; set; }
    }
}
