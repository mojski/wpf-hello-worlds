using WinUI.Models.Entities;

namespace WinUI.Models.Interfaces
{
    public interface IFunFactService
    {
        Task<IEnumerable<FunFactEntity>> ListAsync(string filePath, CancellationToken cancellationToken = default);
        Task UpdateAsync(IEnumerable<FunFactEntity> entities, string filePath, CancellationToken cancellationToken = default);
    }
}
