﻿using System.ComponentModel.DataAnnotations;

public class Contact
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Message { get; set; }
}