using System.ComponentModel.DataAnnotations;

public class LoginData
{
    [Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string Email { get; set; }
    [Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string Password { get; set; }

    public bool EmailIsWrong { get; set; }
}