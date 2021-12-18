using System.ComponentModel.DataAnnotations;

public class Contact
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string Name { get; set; }
    [Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string Email { get; set; }
    [Required(ErrorMessage = "Dit is verplicht om in te vullen")] public string Message { get; set; }
}