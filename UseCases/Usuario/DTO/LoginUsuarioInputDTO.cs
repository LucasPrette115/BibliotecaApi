using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.UseCases.Usuario.DTO
{
    public class LoginUsuarioInputDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email é obrigatório.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Senha é obrigatória.")]
        public string Senha { get; set; }
    }
}
