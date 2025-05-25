public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return users.Select(u => new UserDto(u.Id, u.Name));
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user == null ? null : new UserDto(user.Id, user.Name);
    }

    public async Task<UserDto> CreateAsync(UserDto userDto)
    {
        var user = new User { Name = userDto.Name };
        await _repo.AddAsync(user);
        await _repo.SaveAsync();

        return new UserDto(user.Id, user.Name);
    }

    public async Task<bool> UpdateAsync(int id, UserDto userDto)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return false;

        user.Name = userDto.Name;
        await _repo.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return false;

        await _repo.DeleteAsync(user);
        await _repo.SaveAsync();
        return true;
    }
}
