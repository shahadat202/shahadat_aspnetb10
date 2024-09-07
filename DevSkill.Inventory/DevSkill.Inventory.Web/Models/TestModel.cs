using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Models
{
    public class TestModel
    {
        [Required, EmailAddress(ErrorMessage = "Please provide valid email"), Display(Name = "Email Address")]
        public string? Email { get; set; }
        //[RegularExpression("", ErrorMessage = "")]
        public string? Password { get; set; }
        
        [Compare("Password")]
        [Display (Name="Confirm Passwrod")]
        public string? ConfirmPassword { get; set; }
    }
}
