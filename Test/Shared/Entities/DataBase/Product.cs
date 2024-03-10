using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Test.Shared.Entities.DataBase
{
    public class Product
    {
        class product
        {

        }
        [Key]
        [MaxLength(37)]
        public string? Id { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Name { get; set; }
        [MaxLength(50)]
        public string? Description { get; set; }
        [Required]
        
        public decimal? Price { get; set; }
        [Required]
        [DisplayName("upload Image")]
        public string? Photo { get; set; }
    }
}
