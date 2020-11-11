

using System.Threading.Tasks;
using Xunit;

namespace eshoponline_test.Controllers.Brands
{
    public class ProductsCount : SliceFixture
    {
        [Fact]
        public async Task Expect_products_count_add()
        {
            //Create brand
            var commandCreateBrand = new eshoponline.Controllers.Brands.Create.Command()
            {
                Name = "IBM",
                Slug = "ibm"
            };
            var brand = await BrandHelper.CreateBrand(this, commandCreateBrand);

            //Create product
            var commandCreateProduct = new eshoponline.Controllers.Products.Create.Command()
            {
                Name = "Book 1",
                Description = "Description book",
                Slug = "book_1",
                Summary = "Summary",
                BrandId = brand.BrandId
            };
            await eshoponline_test.Controllers.Products.ProductHelper.CreateProduct(this, commandCreateProduct);

            //Verify brand count
            brand = await BrandHelper.GetBrand(this, brand.BrandId);

            Assert.Equal(1, brand.ProductsCount);
        }

        [Fact]
        public async Task Expect_products_count_add_remove()
        {
            //Create brand
            var commandCreateBrand = new eshoponline.Controllers.Brands.Create.Command()
            {
                Name = "IBM",
                Slug = "ibm"
            };
            var brand = await BrandHelper.CreateBrand(this, commandCreateBrand);

            //Create product
            var commandCreateProduct = new eshoponline.Controllers.Products.Create.Command()
            {
                Name = "Book 1",
                Description = "Description book",
                Slug = "book_1",
                Summary = "Summary",
                BrandId = brand.BrandId
            };
            var product = await eshoponline_test.Controllers.Products.ProductHelper.CreateProduct(this, commandCreateProduct);

            //Verify brand count
            brand = await BrandHelper.GetBrand(this, brand.BrandId);
            Assert.Equal(1, brand.ProductsCount);

            //Delete product
            var commandDeleteProduct = new eshoponline.Controllers.Products.Delete.Command()
            {
                ProductId = product.ProductId
            };
            await eshoponline_test.Controllers.Products.ProductHelper.DeleteProduct(this, commandDeleteProduct);

            //Verify product count
            brand = await BrandHelper.GetBrand(this, brand.BrandId);
            Assert.Equal(0, brand.ProductsCount);
        }
    }
}
