$projectName = "Hello"

# create sln and project
dotnet new sln --name $projectName --output .\src
dotnet new wpf --name WinUI --output .\src\WinUI
dotnet sln .\src\$projectName.sln add .\src\WinUI
dotnet new gitignore


#add packages
cd .\src\WinUI\
dotnet add package CommunityToolkit.Common
    dotnet add package CommunityToolkit.Diagnostics
    dotnet add package CommunityToolkit.HighPerformance
    dotnet add package CommunityToolkit.Mvvm
    dotnet add package Microsoft.Extensions.DependencyInjection
    dotnet add package Microsoft.Xaml.Behaviors.Wpf
    dotnet add package MvvmDialogs

cd ..\..

