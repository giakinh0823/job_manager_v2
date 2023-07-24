using BusinessObject;
using server.Validate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Dto.Job
{
    public class JobCreateRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Webhook không được để trống")]
        public string? Webhook { get; set; }
        public string? Payload { get; set; }
        public string? Header { get; set; }
        [Required(ErrorMessage = "Method không được để trống")]
        [AllowedHttpMethods(ErrorMessage = "Sai HttpMethod. chỉ được phép chọn GET, POST, PUT, DELETE.")]
        public string? Method { get; set; }
        [Required(ErrorMessage = "Expression không được để trống")]
        public string? Expression { get; set; }

        [Required(ErrorMessage = "Status không được để trống")]
        [AllowedStatuses(ErrorMessage = "Sai Trạng thái job. chỉ cho phép ACTIVE, INACTIVE.")]
        public string? Status { get; set; }
    }
}
