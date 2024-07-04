using System.ComponentModel.DataAnnotations;

namespace pfe.ModelViews
{
    public class loginModel
    {
        [StringLength(256), Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public bool rememberMe { get; set; }
    }
}
