using Authorisation.Adaptor.Request;
using Authorisation.Core.Services;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Authorisation.Tests
{
    [TestFixture]
    public class CognitiveAdminServiceFixtures
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Explicit()]
        public async Task AddGroupAsync()
        {
            const int expectedGroupId = 1;

            const string expectedGroupName = "Employees";
            ;
            var addGroupRequest = new AddGroupRequest()
            {
                Id = expectedGroupId,
                Name = expectedGroupName
            };

            var getGroupRequest = new GetGroupRequest()
            {
                Id = expectedGroupId
            };

            var cognitiveAdminService = new CognitiveAdminService();

            await cognitiveAdminService.Handle(addGroupRequest);

            var persistedGroup = await cognitiveAdminService.Handle(getGroupRequest);

            Assert.That(persistedGroup.Id == expectedGroupId.ToString());

            Assert.That(persistedGroup.Name == expectedGroupName);
        }


        [Test]
        [Explicit()]
        public async System.Threading.Tasks.Task AddPerson()
        {
            // start here.
            Task.Delay(1000);
        }

        [Test]
        [Explicit()]
        public async System.Threading.Tasks.Task AddPersonToGroupAsync()
        {
            // start here.
            Task.Delay(1000);
        }
    }
}