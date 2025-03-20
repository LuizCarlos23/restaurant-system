using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        [DisplayName("Confirmar senha")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
