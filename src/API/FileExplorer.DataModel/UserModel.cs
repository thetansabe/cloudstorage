using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileExplorer.DataModel
{
    [Table("User", Schema = "User")]
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserModel: BaseModel
    {
        [Required, StringLength(50)]
        public string UserName { get; set; } = null!;

        [Required]
        public string HashPassword { get; set; } = null!;

        [StringLength(50)]
        public string? DisplayName { get; set; }

        [Required, StringLength(50)]
        public string Email { get; set; } = null!;
    }
}
