public class UserProfile
{
    public User User { get; set; }

    public string NewFirstName { get; set; }
    public string NewLastName { get; set; }

    public string NewEmail { get; set; }
    public string RepeatedNewEmail { get; set; }
    public string PasswordEmail { get; set; }

    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string RepeatedNewPassword { get; set; }
}
