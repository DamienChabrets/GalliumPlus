/*using System.ComponentModel.DataAnnotations;

namespace GalliumPlus.WebApi.Dto
{
    public class PasswordModification
    {
        public string? CurrentPassword { get; }

        public string? ResetToken { get; }

        [Required] public string NewPassword { get; }

        public PasswordModification(string newPassword, string? currentPassword, string? resetToken)
        {
            this.CurrentPassword = currentPassword;
            this.ResetToken = resetToken;
            this.NewPassword = newPassword;
        }
    }
}
*/