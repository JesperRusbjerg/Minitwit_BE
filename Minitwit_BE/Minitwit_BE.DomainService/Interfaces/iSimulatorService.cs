namespace Minitwit_BE.DomainService.Interfaces
{
    public interface ISimulationService
    {
        Task UpdateLatest(int? latest);
        Task<int> GetLatest();
    }
}
