public interface IUserService
{
    Task<IEnumerable<UserProfileDto>> GetAllAsync();
    Task<UserProfileDto?> GetByIdAsync(Guid id);
    Task<UserDto> CreateAsync(CreateUserDto userDto);
    Task<bool> UpdateAsync(Guid id, UserDto userDto);
    Task<bool> DeleteAsync(Guid id);
}
