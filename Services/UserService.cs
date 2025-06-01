public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<UserProfileDto>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return users.Select(u => new UserProfileDto
        {
            Id = u.Id,
            Name = u.Name,
            Orders = u.Orders.Select(o => new OrderDto
            {
                Id = o.Id,
                Date = o.Date,
                Products = o.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                }).ToList()
            }).ToList()
        });
    }


    public async Task<UserProfileDto?> GetByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null)
            throw new NotFoundException($"User with ID {id} not found.");

        return new UserProfileDto
        {
            Id = user.Id,
            Name = user.Name,
            Orders = user.Orders.Select(order => new OrderDto
            {
                Id = order.Id,
                Date = order.Date,
                Products = order.Products.Select(product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                }).ToList()
            }).ToList()
        };
    }


    public async Task<UserDto> CreateAsync(CreateUserDto userDto)
    {
        var user = new User { Name = userDto.Name };
        await _repo.AddAsync(user);
        await _repo.SaveAsync();

        return new UserDto(user.Id, user.Name);
    }

    public async Task<bool> UpdateAsync(Guid id, UserDto userDto)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return false;

        user.Name = userDto.Name;
        await _repo.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return false;

        await _repo.DeleteAsync(user);
        await _repo.SaveAsync();
        return true;
    }
}
