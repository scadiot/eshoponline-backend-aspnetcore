using System.Threading.Tasks;
using Xunit;
using eshoponline.Controllers.Specifications;
using eshoponline_test.Controllers.SpecificationGroups;

namespace eshoponline_test.Controllers.Specifications
{
    public class EditTest : SliceFixture
    {
        [Fact]
        public async Task Expect_Edit_Specifications()
        {
            await MainHelper.CreateRandomData(this);

            //Create specification and specification group
            var commandCreateGroup1 = new eshoponline.Controllers.SpecificationGroups.Create.Command()
            {
                Name = "performances",
                LongName = "Performances"
            };
            var specificationGroup1 = await SpecificationGroupHelper.CreateSpecificationGroup(this, commandCreateGroup1);

            var commandCreateGroup2 = new eshoponline.Controllers.SpecificationGroups.Create.Command()
            {
                Name = "performances 2",
                LongName = "Performances 2"
            };
            var specificationGroup2 = await SpecificationGroupHelper.CreateSpecificationGroup(this, commandCreateGroup2);

            var command = new Create.Command()
            {
                Name = "Fréquence",
                LongName = "Fréquence du processeur",
                Type = eshoponline.Models.SpecificationType.Decimal,
                Unity = "Ghz",
                SpecificationGroupId = specificationGroup1.SpecificationGroupId
            };
            var specification = await SpecificationHelper.CreateSpecification(this, command);

            //Edit specification
            var commandEdit = new Edit.Command()
            {
                SpecificationId = specification.SpecificationId,
                Name = "Fréquence 2",
                LongName = "Fréquence du processeur 2",
                Type = eshoponline.Models.SpecificationType.Boolean,
                Unity = "Ghz 2",
                SpecificationGroupId = specificationGroup2.SpecificationGroupId
            };

            var dbContext = this.GetDbContext();
            var mapper = this.GetMapper();

            var specificationEditHandler = new Edit.Handler(dbContext, mapper);
            var editedSpecification = await specificationEditHandler.Handle(commandEdit, new System.Threading.CancellationToken());

            //Test results
            Assert.NotNull(specification);
            Assert.Equal(commandEdit.SpecificationId, editedSpecification.SpecificationId);
            Assert.Equal(commandEdit.Name, editedSpecification.Name);
            Assert.Equal(commandEdit.LongName, editedSpecification.LongName);
            Assert.Equal(commandEdit.Type, editedSpecification.Type);
            Assert.Equal(commandEdit.Unity, editedSpecification.Unity);
            Assert.Equal(commandEdit.SpecificationGroupId, editedSpecification.SpecificationGroupId);
        }
    }
}
