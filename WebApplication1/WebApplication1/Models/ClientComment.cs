using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ClientComment
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Comment { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string PhotoUrl { get; set; }
    }
}
