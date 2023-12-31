namespace GalliumPlusApi.Dto
{
    public class ProductDetails
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public int Stock { get; set; } = 0;
        public decimal NonMemberPrice { get; set; } = -1;
        public decimal MemberPrice { get; set; } = -1;
        public string Availability { get; set; } = "";
        public bool Available { get; set; } = true;
        public CategoryDetails Category { get; set; } = new();
    }
}