using System;
using System.Threading.Tasks;
using Authorisation.Adaptor.Request;
using Authorisation.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;


namespace Authorisation.Tests
{
    [TestFixture]
    public class CognitiveAdminServiceFixtures
    {
        private const int _groupId = 1;

        private ICognitiveAdminService _cognitiveAdminService;

        [SetUp]
        public void Setup()
        {
            _cognitiveAdminService = new CognitiveAdminService();
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

            await _cognitiveAdminService.Handle(addGroupRequest);

            var persistedGroup = await _cognitiveAdminService.Handle(getGroupRequest);

            Assert.That(persistedGroup.Id == expectedGroupId.ToString());

            Assert.That(persistedGroup.Name == expectedGroupName);
        }


        [Test]
        [Explicit()]
        public async Task AddPerson()
        {
            var addPersonRequest = new AddPersonRequest()
            {
                Name = "Brian Mooney",

                GroupId = _groupId
            };

            var persistedPerson = await _cognitiveAdminService.Handle(addPersonRequest);

            Assert.IsNotNull(persistedPerson.PersonId);                        
        }


        [Test]        
        public async Task GetPerson()
        {            
            Guid personId = Guid.Parse("90a33570-3369-4e63-ae73-97f55b8d4aca");

            var getPersonRequest = new GetPersonRequest()
            {
                GroupId = _groupId,

                PersonId = personId
            };

            var persistedPerson = await _cognitiveAdminService.Handle(getPersonRequest);

            Assert.IsNotNull(persistedPerson.PersonId);

            Assert.IsNotNull(persistedPerson.Name);
        }

        [Test]
        public void RemovePerson()
        {
            // todo: 


        }

        [Test]
        [Explicit]
        public async Task AddFaceToPerson()
        {
            Guid personId = Guid.Parse("90a33570-3369-4e63-ae73-97f55b8d4aca");

            // todo; Read in from test data
            const string capturePath = @"C:\Users\breen\Desktop\Temp.jpg";

            byte[] faceCapture = System.IO.File.ReadAllBytes(capturePath);

            var addFaceToPersonRequest = new AddFaceToPersonRequest()
            {
                GroupId = _groupId,

                PersonId = personId,

                FaceCapture = faceCapture
            };
            
            var addFaceToPersonResponse = await _cognitiveAdminService.Handle(addFaceToPersonRequest);            

            Assert.IsNotNull(addFaceToPersonResponse.PersistedFaceId);
        }


        [Test]
        [Explicit]
        public async Task TrainGroup()
        {
            var trainGroupRequest = new TrainGroupRequest
            {
                GroupId = _groupId
            };

            var trainGroupResponse = await _cognitiveAdminService.Handle(trainGroupRequest);

            Assert.AreEqual(trainGroupResponse.Status, "Succeeded");
        }

    }
}