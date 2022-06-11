using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    [Serializable]
    class GroupBooks
    {
        /// <summary>
        /// Класс предназначен для реализации сущности "Группа книг";
        /// группирует одинаковые книги
        /// </summary>
        /// <param>
        /// _books - объект группы книг; 1 группа, _count - количество книг в группе, _name - название книги, _author - имя и фамилия автора
        /// </param>
        public List<Book> _books { get; private set; }
        public string _name { get; private set; }
        public string _author { get; private set; }
        public int _count => _books.Count;
        public GroupBooks(string name,string author)
        {
            _name = name;
            _author = author;
            _books = new List<Book>();
        }
        public Book GiveBook()
        {
            if (_count <= 0)
                return null;
            var temp =_books[0];
            _books.RemoveAt(0);
            return temp;
        }
        public void Add(Book book)
        {
            if (book != null && book._name == _name && book._author == _author)
                _books.Add(new Book(book._name,book._author));
            else
                throw new ArgumentException("Group doesn't suit for this book");
        }
        public void Add(string name,string author)
        {
            if (name == _name && author == _author)
                _books.Add(new Book(name,author));
            else
                throw new ArgumentException("Group doesn't suit for this book");
        }
    }
}
