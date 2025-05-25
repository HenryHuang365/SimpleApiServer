using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}

public class UserProfileDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<OrderDto> Orders { get; set; } = new();
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
