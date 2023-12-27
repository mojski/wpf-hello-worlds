using System.Collections.ObjectModel;

namespace WinUI.Models.Entities
{
    public sealed class FunFactEntity
    {
        public int Id { get; init; } = default;
        public string Title { get; init; } = string.Empty;
        public string Image { get; init; } = string.Empty;
        public string Link { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public List<int> RelatedMovies { get; init; } = [];

        public FunFact ToFunFact()
        {
            var relatedMovies = new ObservableCollection<RelatedMovie>();

            foreach (var movieId in RelatedMovies)
            {
                relatedMovies.Add(new RelatedMovie{Id = movieId});
            }

            return new FunFact
            {
                Id = this.Id,
                Title = this.Title,
                Image = this.Image,
                Link = this.Link,
                Content = this.Content,
                RelatedMovies = relatedMovies
            };
        }

        public static FunFactEntity FromFunFact(FunFact funFact)
        {
            return new FunFactEntity
            {
                Id = funFact.Id.Value,
                Title = funFact.Title,
                Image = funFact.Image,
                Link = funFact.Link,
                Content = funFact.Content,
                RelatedMovies = funFact.RelatedMovies.Select(movie => movie.Id).ToList(),
            };
        }
    }
}
    