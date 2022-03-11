using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;
using System.Threading.Tasks;

namespace Minitwit_BE.DomainService
{
    public class SimulatorService : ISimulationService
    {

        private readonly IPersistenceService _persistenceService;

        public SimulatorService(IPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
        }

        public Task<int> GetLatest() { return _persistenceService.GetLatest(); }

        public Task UpdateLatest(int? latest) { return _persistenceService.UpdateLatest(latest); }
    }
}
