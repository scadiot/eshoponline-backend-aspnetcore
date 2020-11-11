using System.Threading.Tasks;
using Xunit;
using eshoponline.Controllers.Specifications;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eshoponline_test.Controllers.Specifications
{
    public class DeleteTest : SliceFixture
    {
        [Fact]
        public async Task Expect_Delete_Specifications()
        {
            await MainHelper.CreateRandomData(this);

            //Create specification
            var command = new Create.Command()
            {
                Name = "Frequency",
                LongName = "Processor frequency",
                Type = eshoponline.Models.SpecificationType.Decimal,
                Unity = "Ghz"
            };

            var specification = await SpecificationHelper.CreateSpecification(this, command);

            //Delete specification
            var commandDelete = new Delete.Command()
            {
                SpecificationId = specification.SpecificationId
            };

            var dbContext = this.GetDbContext();
            var specificationDeleteHandler = new Delete.Handler(dbContext);
            await specificationDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test specification existence
            var dbSpecification = await SpecificationHelper.GetSpecification(this, specification.SpecificationId);

            Assert.Null(dbSpecification);
        }
    }
}
