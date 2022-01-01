using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Linq;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        private AppDbContext _context;

        public PublishersService(AppDbContext context)
        {
            _context = context;
        }

        public void AddPublisher(PublisherVM publisher)
        {
            var _publisher = new Publisher()
            {
                Name = publisher.Name,
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();
        }

        public PublisherWithBooksAndAuthorsVM GetPublisherData (int publisherId)
        {
            var _publisherData = _context.Publishers.Where(p => p.Id == publisherId)
                .Select(publisher => new PublisherWithBooksAndAuthorsVM()
                {
                    Name = publisher.Name,
                    BookAuthors = publisher.Books.Select(b => new BookAuthorVM()
                    {
                        BookName = b.Title,
                        BookAuthors = b.Book_Authors.Select(a => a.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();

            return _publisherData;
        }

        public void DeletePublisherById(int publisherId)
        {
            var publisher = _context.Publishers.FirstOrDefault(p => p.Id == publisherId);

            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
            }
        }
    }

}
