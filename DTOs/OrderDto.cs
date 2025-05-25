public class OrderDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public List<ProductDto> Products { get; set; } = new();
}