using System.ComponentModel.DataAnnotations;

namespace pfe.ModelViews
{
    public class registerModel
    {
        [StringLength(256), Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(256), Required]
        public String Username { get; set; }
        [StringLength(256)]
        public String PhoneNumber { get; set; }

        [Required]
        public string role { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
