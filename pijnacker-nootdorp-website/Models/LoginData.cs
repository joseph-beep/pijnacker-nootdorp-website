using System.ComponentModel.DataAnnotations;

public class LoginData
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }

    public bool EmailIsWrong { get; set; }
}