using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.MVCWebApp.HoaLQ.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter your username")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Please enter your password")]
    [DataType(DataType.Password)]   
    public string Password { get; set; }
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}