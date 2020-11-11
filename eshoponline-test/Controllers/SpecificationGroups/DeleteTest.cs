using System.Threading.Tasks;
using Xunit;
using eshoponline.Controllers.SpecificationGroups;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace eshoponline_test.Controllers.SpecificationGroups
{
    public class DeleteTest : SliceFixture
    {
        [Fact]
        public async Task Expect_Delete_SpecificationGroups()
        {
            await MainHelper.CreateRandomData(this);

            //Create specification group
            var command = new Create.Command()
            {
                Name = "performances",
                LongName = "Performances"
            };

            var specificationGroup = await SpecificationGroupHelper.CreateSpecificationGroup(this, command);

            //Delete specification group
            var commandDelete = new Delete.Command()
            {
                SpecificationGroupId = specificationGroup.SpecificationGroupId
            };

            var dbContext = this.GetDbContext();
            var specificationGroupDeleteHandler = new Delete.Handler(dbContext);
            await specificationGroupDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Test specification group existence
            var dbSpecificationGroup = await ExecuteDbContextAsync(db => db.SpecificationGroups
                .Where(d => d.SpecificationGroupId == specificationGroup.SpecificationGroupId)
                .SingleOrDefaultAsync());

            Assert.Null(dbSpecificationGroup);
        }

        [Fact]
        public async Task Expect_Delete_SpecificationGroups_affect_child()
        {
            //Create specification group
            var commandCreateSpecificationGroup = new Create.Command()
            {
                Name = "performances",
                LongName = "Performances"
            };

            var specificationGroup = await SpecificationGroupHelper.CreateSpecificationGroup(this, commandCreateSpecificationGroup);

            //Create specification
            var commandCreateSpecification = new eshoponline.Controllers.Specifications.Create.Command()
            {
                Name = "Fréquence",
                LongName = "Fréquence du processeur",
                Type = eshoponline.Models.SpecificationType.Decimal,
                Unity = "Ghz",
                SpecificationGroupId = specificationGroup.SpecificationGroupId
            };

            var specification = await Specifications.SpecificationHelper.CreateSpecification(this, commandCreateSpecification);

            Assert.NotNull(specification.SpecificationGroup);

            //Delete specification group
            var commandDelete = new Delete.Command()
            {
                SpecificationGroupId = specificationGroup.SpecificationGroupId
            };

            var dbContext = this.GetDbContext();
            var specificationGroupDeleteHandler = new Delete.Handler(dbContext);
            await specificationGroupDeleteHandler.Handle(commandDelete, new System.Threading.CancellationToken());

            //Reload specification
            specification = await Specifications.SpecificationHelper.GetSpecification(this, specification.SpecificationId);

            Assert.Null(specification.SpecificationGroup);
        }
    }
}
