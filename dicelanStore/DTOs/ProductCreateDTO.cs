namespace dicelanStore.DTOs
{
    public class ProductCreateDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double? Price { get; set; }
        public short? StockQuantity { get; set; }
        public int MaterialId { get; set; }
        public int ProductTypeId { get; set; }
        public decimal? Discount { get; set; }
    }

    public class ProductListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double? Price { get; set; }
        public short? StockQuantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public int ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; }
        public decimal? Discount { get; set; }
        public List<byte[]> Images { get; set; } = new List<byte[]>();
        public byte[]? FirstImage { get; set; }
    }
}
