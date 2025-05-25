using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public UserDto(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public UserDto() { }
}
