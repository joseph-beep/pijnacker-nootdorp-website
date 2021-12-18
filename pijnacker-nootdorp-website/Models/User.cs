using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string FirstName { get; set; } = "";
    [Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string LastName { get; set; } = "";

    [EmailAddress, Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string Email { get; set; } = "";
    [Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string Password { get; set; } = "";

    public virtual Order Order { get; set; }
}