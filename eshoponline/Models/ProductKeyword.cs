namespace eshoponline.Models
{
    public class ProductKeyword
    {
        public int ProductKeywordId { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int KeywordId { get; set; }
        public virtual Keyword Keyword { get; set; }
    }
}
