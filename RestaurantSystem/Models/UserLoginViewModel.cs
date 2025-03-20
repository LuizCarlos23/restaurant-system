using System.ComponentModel;
using System.ComponentModel.DataAnnotations;    

namespace RestaurantSystem.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório.")]
        [DisplayName("Senha")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Lembrar de mim?")]
        public bool RememberMe { get; set; }
    }
}
