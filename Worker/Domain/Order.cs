namespace API.Domain
{
    public class Order
    {
        public int Number { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return "Product\n\n" +
                   $"Number: {this.Number}" +
                   $"Name: {this.ItemName}" +
                   $"Price: {this.Price:F2}";
        }
    }
}