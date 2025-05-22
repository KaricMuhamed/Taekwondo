using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaekwondoBackend.Entities
{
    [Table("korisnici")]
    public class Korisnik
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(255)]
        [Column("ime")]
        public string Ime { get; set; }

        [StringLength(20)]
        [Column("telefon")]
        public string Telefon { get; set; }

        [Required]
        [StringLength(20)]
        [Column("uloga")]
        public string Uloga { get; set; } // administrator, trener, ucenik

        [Required]
        [StringLength(20)]
        [Column("stanje")]
        public string Stanje { get; set; } = "na_cekanju"; // aktivan, neaktivan, na_cekanju

        [Required]
        [StringLength(20)]
        [Column("pojas")]
        public string Pojas { get; set; } = "bijeli"; // bijeli, zuti, narancasti, zeleni, plavi, smedi, crni_1_dan...crni_10_dan

        [Required]
        [Column("datum_pridruzivanja")]
        public DateTime DatumPridruzivanja { get; set; } = DateTime.UtcNow.Date;

        [Column("napomene")]
        public string Napomene { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<Objava> AutorObjave { get; set; }
    }
}
