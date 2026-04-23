namespace HelloGraphQL.Schema;

public record Book(string Title, string Author, int Year);

public class Query
{
    private readonly ILogger<Query> _logger;

    public Query(ILogger<Query> logger)
    {
        _logger = logger;
    }

    public string Hello(string name = "World")
    {
        _logger.LogDebug("Hello query called with name={Name}", name);
        return $"Hello, {name}!";
    }

    public IEnumerable<Book> Books()
    {
        _logger.LogDebug("Books query called, returning {Count} books", 3);
        return
        [
            new("The Pragmatic Programmer", "David Thomas & Andrew Hunt", 1999),
            new("Clean Code", "Robert C. Martin", 2008),
            new("Designing Data-Intensive Applications", "Martin Kleppmann", 2017),
        ];
    }

    public Book? BookByTitle(string title)
    {
        _logger.LogDebug("BookByTitle query called with title={Title}", title);
        var result = Books().FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        if (result is null)
            _logger.LogInformation("No book found with title={Title}", title);
        return result;
    }
}
