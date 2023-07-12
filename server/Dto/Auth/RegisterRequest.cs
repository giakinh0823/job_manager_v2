using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace server.Dto.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        [MinLength(6, ErrorMessage = "Password có độ dài từ 6 ký tự")]
        public string? Password { get; set; }
    }
}
