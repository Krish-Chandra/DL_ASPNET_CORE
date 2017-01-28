using DL_Core_WebAPP_Release.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DL_Using_DotNet_Core.Infrastructure
{
    public class LibraryRepository
    {
        private static DLContext _context;
        public LibraryRepository(DLContext ctx)
        {
            _context = ctx;
        }

        public static DLContext Current
        {
            get
            {
                return _context;
            }
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).ToList();
        }

        public Book Get(int id)
        {

            return _context.Books.Include(b => b.Author).Include(b => b.Category).Include(b => b.Publisher).First(book => book.BookId == id);
        }

        public Book Add(Book item)
        {
            return null;
        }

        public void Remove(int id)
        {
        }

        public bool Update(Book item)
        {
            return true;
            //Book storedItem = Get(item.BookId);
            //if (storedItem != null)
            //{
            //    storedItem.Author = item.Author;
            //    storedItem.ISBN = item.ISBN;
            //    storedItem.Title = item.Title;

            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
    }
}
