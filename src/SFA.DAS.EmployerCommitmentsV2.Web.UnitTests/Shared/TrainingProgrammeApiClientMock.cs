using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Shared
{
    public class TrainingProgrammeApiClientMock
    {
        private readonly Mock<ITrainingProgrammeApiClient> _trainingProgrammeApiClient;
        private readonly List<ITrainingProgramme> _standardTrainingProgrammes;
        private readonly List<ITrainingProgramme> _allTrainingProgrammes;

        public TrainingProgrammeApiClientMock()
        {
            var autoFixture = new Fixture();
            var standards = autoFixture.Create<List<Standard>>();
            var frameworks = autoFixture.Create<List<Framework>>();
            _standardTrainingProgrammes = standards.Select(x => x as ITrainingProgramme).ToList();
            _allTrainingProgrammes = new List<ITrainingProgramme>();
            _allTrainingProgrammes.AddRange(standards);
            _allTrainingProgrammes.AddRange(frameworks);
            _trainingProgrammeApiClient = new Mock<ITrainingProgrammeApiClient>();
            _trainingProgrammeApiClient.Setup(x => x.GetAllTrainingProgrammes()).ReturnsAsync(_allTrainingProgrammes);
            _trainingProgrammeApiClient.Setup(x => x.GetStandardTrainingProgrammes())
                .ReturnsAsync(_standardTrainingProgrammes);

            _trainingProgrammeApiClient.Setup(x => x.GetTrainingProgramme(It.IsAny<string>()))
                .ReturnsAsync((string requestedId) => { return _allTrainingProgrammes.Single(y => y.Id == requestedId); });
        }

        public ITrainingProgrammeApiClient Object => _trainingProgrammeApiClient.Object;
        public IReadOnlyList<ITrainingProgramme> Standards => _standardTrainingProgrammes;
        public IReadOnlyList<ITrainingProgramme> All => _allTrainingProgrammes;
    }
}
