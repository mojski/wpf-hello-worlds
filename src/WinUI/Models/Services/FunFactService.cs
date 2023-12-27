using Newtonsoft.Json;
using System.IO;
using WinUI.Models.Entities;
using WinUI.Models.Interfaces;

namespace WinUI.Models.Services
{
    public sealed class FunFactService : IFunFactService
    {
        public async  Task<IEnumerable<FunFactEntity>> ListAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var fileContent = await File.ReadAllTextAsync(filePath, cancellationToken);
            var entities = JsonConvert.DeserializeObject<IEnumerable<FunFactEntity>>(fileContent);
            return entities;
        }

        public async Task UpdateAsync(IEnumerable<FunFactEntity> entities, string filePath, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(entities);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await File.WriteAllTextAsync(filePath!, json, cancellationToken);
        }
    }
}
