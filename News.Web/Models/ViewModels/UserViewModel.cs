using System.ComponentModel.DataAnnotations;

namespace News.Web.Models.ViewModels
{
    public class UserViewModel
    {
        public List<User> Users { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool AdminRoleCheckbox { get; set; }
    }
}
