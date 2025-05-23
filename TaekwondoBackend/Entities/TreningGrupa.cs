using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaekwondoBackend.Entities
{
    [Table("trening_grupe")]
    public class TreningGrupa
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("trening_id")]
        public Guid TreningId { get; set; }

        [Required]
        [Column("grupa_id")]
        public Guid GrupaId { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("TreningId")]
        public virtual Trening Trening { get; set; }

        [ForeignKey("GrupaId")]
        public virtual Grupa Grupa { get; set; }
    }
}
