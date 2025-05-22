using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaekwondoBackend.Entities
{
    [Table("objave")]
    public class Objava
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("naslov")]
        public string Naslov { get; set; }

        [Required]
        [Column("sadrzaj")]
        public string Sadrzaj { get; set; }

        [Required]
        [StringLength(20)]
        [Column("tip")]
        public string Tip { get; set; } // vijesti, obavjestenje

        [StringLength(512)]
        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Required]
        [Column("autor_id")]
        public Guid AutorId { get; set; }

        [Required]
        [Column("objavljeno")]
        public DateTime Objavljeno { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("AutorId")]
        [JsonIgnore]
        public virtual Korisnik Autor { get; set; }
    }
}
