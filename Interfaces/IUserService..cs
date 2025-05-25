public interface IUserService
{
    Task<IEnumerable<UserProfileDto>> GetAllAsync();
    Task<UserProfileDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(UserDto userDto);
    Task<bool> UpdateAsync(int id, UserDto userDto);
    Task<bool> DeleteAsync(int id);
}
