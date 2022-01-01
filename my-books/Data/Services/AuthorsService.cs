using my_books.Data.Models;
using my_books.Data.ViewModels;
using System.Linq;

namespace my_books.Data.Services
{
    public class AuthorsService
    {
        private AppDbContext _context;

        public AuthorsService(AppDbContext context) 
        {
            _context = context;
        }

        public void AddAuthor(AuthorVM author)
        {
            var _author = new Author()
            {
                FullName = author.FullName,
            };
            _context.Authors.Add(_author);
            _context.SaveChanges();
        }

        public AuthorWithBookVM GetAuthorWithBooks(int authorId)
        {
            var _author = _context.Authors.Where(a => a.Id == authorId).Select(autohr => new AuthorWithBookVM()
            {
                FullName = autohr.FullName,
                BookTitles = autohr.Book_Authors.Select(ba => ba.Book.Title).ToList(),
            }).FirstOrDefault();

            return _author;
        }
    }
}
