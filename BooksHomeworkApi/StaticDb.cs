using BooksHomeworkApi.Models;

namespace BooksHomeworkApi
{
    public static class StaticDb
    {
        public static List<Book> Books = new List<Book>
        {
            new Book { Title = "Lord of the rings: Fellowship of the ring", Author = "J.R.R. Tolkein" },
            new Book { Title = "Lord of the rings: Two towers", Author = "J.R.R. Tolkein" }
        };
    }
}
