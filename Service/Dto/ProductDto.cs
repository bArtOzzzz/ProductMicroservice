namespace Services.Dto
{
    public class ProductDto
    {
        public CrudOperationsInfo CrudOperationsInfo { get; set; }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? PreviousName { get; set; }
        public string? LinkImage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
