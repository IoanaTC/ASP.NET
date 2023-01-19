using System.ComponentModel.DataAnnotations;

namespace Modeltest2021.Models
{
    public class GiftCard
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Denumirea cardului este obligatorie")]
        public string Denumire { get; set; }

        [Required(ErrorMessage = "Descrierea cardului este obligatorie")]
        public string Descriere { get; set; }

        [Required(ErrorMessage = " Data expirarii este obligatorie")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataExp { get; set; }

        [Required(ErrorMessage = "Procentul este obligatoriu")]
        [Range(1, 100, ErrorMessage = "Procentul trebuie sa fie cuprins intre 1 si 100")]
        public int Procent { get; set; }

        [Required(ErrorMessage = "Brandul trebuie selectat")]
        public int? BrandId { get; set; }

        public virtual Brand? Brand { get; set; }
    }
}
