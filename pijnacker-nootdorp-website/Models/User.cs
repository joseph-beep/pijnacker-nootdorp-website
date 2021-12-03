using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string Description { get; set; }

    public int Age { get; set; }
}
