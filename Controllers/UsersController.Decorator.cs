using Swashbuckle.AspNetCore.Filters;

public class CreateUserDtoExample : IExamplesProvider<CreateUserDto>
{
    public CreateUserDto GetExamples() => new CreateUserDto
    {
        Name = "Jane",
    };
}
