using my_books.Data.Models;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        private AppDbContext _context;

        public PublishersService(AppDbContext context)
        {
            _context = context;
        }

        public Publisher AddPublisher(PublisherVM publisher)
        {
            if (stringStartsWithNumber(publisher.Name))
                throw new PublisherNameException("Name starts with number", publisher.Name);

            var _publisher = new Publisher()
            {
                Name = publisher.Name,
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();
            return _publisher;
        }

        public Publisher GetPublisherById(int publisherId) => _context.Publishers.FirstOrDefault(p=>p.Id== publisherId);

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
            else throw new Exception($"The Publisher with id ${publisherId} does not exist ");
        }

        private bool stringStartsWithNumber(string name) => Regex.IsMatch(name, @"^\d");
    }

}
