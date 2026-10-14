using trpo6_var2.Models;

namespace trpo6_var2.Service
{
    public class BookService
    {
        private readonly WebApiDatabaseContext _context = ContextService.Instance.Context;

        private List<Book> books = new();

        public List<Book> GetAll()
        {
            var _books = _context.Books.ToList();

            books.Clear();
            foreach (var book in _books)
            {
                books.Add(book);
            }

            return books;
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            Commit();
        }

        public BookService()
        {
            GetAll();
        }

        public int Commit() => _context.SaveChanges();
    }
}
