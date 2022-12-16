namespace ProductMicroservice.Models.Response
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Name { get; set; }
        public string? LinkImage { get; set; }
    }
}
