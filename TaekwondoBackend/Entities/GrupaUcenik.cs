using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaekwondoBackend.Entities
{
    [Table("grupa_ucenici")]
    public class GrupaUcenik
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("grupa_id")]
        public Guid GrupaId { get; set; }

        [Required]
        [Column("ucenik_id")]
        public Guid UcenikId { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("GrupaId")]
        public virtual Grupa Grupa { get; set; }

        [ForeignKey("UcenikId")]
        public virtual Korisnik Ucenik { get; set; }
    }
}
