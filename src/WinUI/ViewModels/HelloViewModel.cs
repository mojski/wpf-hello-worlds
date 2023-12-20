using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using WinUI.Models;

namespace WinUI.ViewModels
{
    partial class HelloViewModel : ObservableObject
    {
        private readonly IDialogService dialogService;

        [ObservableProperty] private bool isClosed = default;
        [ObservableProperty] private Funfact? movie = default;

        public HelloViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            this.CloseWindowCommand = new AsyncRelayCommand(this.CloseWindowAsync);
            this.UpdateMovieCommand = new AsyncRelayCommand<Funfact>(this.UpdateMovieAsync);
        }

        private async Task CloseWindowAsync(CancellationToken cancellationToken)
        {
            IsClosed = true;
            await Task.CompletedTask;
        }

        private async Task UpdateMovieAsync(Funfact? parameter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(parameter);

            var viewModel = new UpdateFunfactViewModel() { Item = this.Movie };
            viewModel.Item.Title = parameter.Title;

            var result = this.dialogService.ShowDialog(this, viewModel);

            if (result is true)
            {
                var x = viewModel.Item;
            }

            this.Movie.Title = viewModel.Item.Title;
            this.Movie.Content = viewModel.Item.Content;
            this.Movie.Link = viewModel.Item.Link;
            this.Movie.Image = viewModel.Item.Image;
            this.Movie.RelatedMovies = viewModel.Item.RelatedMovies;

            await Task.CompletedTask;
        }

        public IAsyncRelayCommand CloseWindowCommand { get; }
        public IAsyncRelayCommand<Funfact> UpdateMovieCommand { get; }
    }
}
