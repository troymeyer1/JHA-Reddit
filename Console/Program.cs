using System.ComponentModel.DataAnnotations;
using Jha.Reddit.Business;
using Jha.Reddit.Shared.Entities;
using Microsoft.Extensions.Configuration;

try
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("config.json", optional: false);

    IConfiguration config = builder.Build();
    var configSettings = config.GetSection("RedditConfiguration").Get<RedditConfigEntity>();

    var results = new List<ValidationResult>();
    Validator.TryValidateObject(configSettings, new(configSettings), results, true);

    if (results.Count != 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid configuration settings, please correct the following items:");
        Console.WriteLine();

        foreach (var validationResult in results)
        {
            Console.WriteLine($"\t- {validationResult.ErrorMessage}");
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine();
        return;
    }

    using var service = new RedditMonitorService(configSettings);
    var consoleLock = new object();

    service.TopPostChanged += (sender, args) =>
    {
        lock (consoleLock)
        {
            Console.WriteLine($"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff}: New top post in {args.Feed.SubReddit} ...");
            Console.WriteLine($"Url: {args.Post.Url}");
            Console.WriteLine($"Title: {args.Post.Title}");
            Console.WriteLine($"Author: {args.Post.Author}");
            Console.WriteLine($"Created: {args.Post.Created}");
            Console.WriteLine($"Up Votes: {args.Post.UpVotes}");
            Console.WriteLine($"Down Votes: {args.Post.DownVotes}");
            Console.WriteLine();
        }
    };

    service.TopUserChanged += (senders, args) =>
    {
        lock (consoleLock)
        {
            Console.WriteLine($"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff}: New top user in {args.Feed.SubReddit} ...");
            Console.WriteLine($"User Name: {args.UserStats.UserName}");
            Console.WriteLine($"Post Count: {args.UserStats.PostCount}");
            Console.WriteLine();
        }
    };

    lock (consoleLock)
    {
        Console.WriteLine("Monitoring Reddit, press any key to exit ...");
        Console.WriteLine();
    }

    service.Start();

    while (!Console.KeyAvailable)
    {
        Thread.Sleep(1000);
    }

    service.Stop();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex);
}
