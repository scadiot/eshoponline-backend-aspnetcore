using System.Threading.Tasks;
using Xunit;
using eshoponline.Controllers.Specifications;
using Microsoft.EntityFrameworkCore;

namespace eshoponline_test.Controllers.Specifications
{
    public class ListTest : SliceFixture
    {
        [Fact]
        public async Task Expect_List_SpecificationGroups()
        {
            await MainHelper.CreateRandomData(this);

            //Create specification
            var command1 = new Create.Command()
            {
                Name = "Frequency",
                LongName = "Processeur du frequency",
                Type = eshoponline.Models.SpecificationType.Decimal,
                Unity = "Ghz"
            };
            var specification1 = await SpecificationHelper.CreateSpecification(this, command1);

            var command2 = new Create.Command()
            {
                Name = "Frequency 2",
                LongName = "Processor frequency 2",
                Type = eshoponline.Models.SpecificationType.Decimal,
                Unity = "Ghz"
            };
            var specification2 = await SpecificationHelper.CreateSpecification(this, command2);

            //List specification
            var command = new List.Query();
            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();
            var specificationsListHandler = new List.Handler(dbContext, mapper);
            var specificationsList = await specificationsListHandler.Handle(command, new System.Threading.CancellationToken());

            //List specification
            var dbSpecifications = await ExecuteDbContextAsync(db => db.Specifications.ToListAsync());

            Assert.Equal(dbSpecifications.Count, specificationsList.Specifications.Count);
        }
    }
}
