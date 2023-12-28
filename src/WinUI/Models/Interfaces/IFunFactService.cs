namespace WinUI.Models.Interfaces
{
    public interface IFunFactService
    {
        Task<IEnumerable<FunFact>> ListAsync(string filePath, CancellationToken cancellationToken = default);
        Task UpdateAsync(IEnumerable<FunFact> entities, string filePath, CancellationToken cancellationToken = default);
    }
}
