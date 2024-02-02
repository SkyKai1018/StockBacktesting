using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
