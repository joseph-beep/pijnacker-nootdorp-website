using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required] public string FirstName { get; set; } = "";
    [Required] public string LastName { get; set; } = "";

    [EmailAddress, Required] public string Email { get; set; } = "";
    [Required] public string Password { get; set; } = "";

    public virtual Order Order { get; set; }
}