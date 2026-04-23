namespace HelloGraphQL.Schema;

public record Book(string Title, string Author, int Year);

public class Query
{
    public string Hello(string name = "World") => $"Hello, {name}!";

    public IEnumerable<Book> Books() =>
    [
        new("The Pragmatic Programmer", "David Thomas & Andrew Hunt", 1999),
        new("Clean Code", "Robert C. Martin", 2008),
        new("Designing Data-Intensive Applications", "Martin Kleppmann", 2017),
    ];

    public Book? BookByTitle(string title) =>
        Books().FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
}
