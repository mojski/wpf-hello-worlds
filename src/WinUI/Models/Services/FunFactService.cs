using Newtonsoft.Json;
using System.IO;
using WinUI.Models.Entities;
using WinUI.Models.Interfaces;

namespace WinUI.Models.Services
{
    public sealed class FunFactService : IFunFactService
    {
        public async Task<IEnumerable<FunFact>> ListAsync(string filePath, CancellationToken cancellationToken = default)
        {
            var fileContent = await File.ReadAllTextAsync(filePath, cancellationToken);
            var entities = JsonConvert.DeserializeObject<IEnumerable<FunFactEntity>>(fileContent);

            var funFacts = new List<FunFact>();

            foreach (var entity in entities) // TODO load images by demand
            {
                var imageFileName = FileService.GetFullFileName(entity.Image);
                var imagePhysicalPath =
                    FileService.GetImagePhysicalPath(filePath, imageFileName);

                var bitmap = await FileService.GetBitmapImageAsync(imagePhysicalPath, cancellationToken);

                var model = entity.ToFunFact();

                model.Image = new Image
                {
                    FileName = imageFileName,
                    Value = bitmap,
                };

                funFacts.Add(model);
            }
            return funFacts;
        }

        public async Task UpdateAsync(IEnumerable<FunFact> funFacts, string filePath, CancellationToken cancellationToken = default)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var enumerable = funFacts.ToList();

            foreach (var funFact in enumerable)
            {
                await FileService.SaveImageAndGetFileNameAsync(funFact.Image.Value, funFact.Image.FileName, filePath, cancellationToken);
            }

            var entities = enumerable.Select(FunFactEntity.FromFunFact);
            var json = JsonConvert.SerializeObject(entities);
            await File.WriteAllTextAsync(filePath!, json, cancellationToken);
        }
    }
}
