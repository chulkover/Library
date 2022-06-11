using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    [Serializable]
    class Book
    {
        /// <summary>
        /// Класс предназначен для реализации сущность книги
        /// </summary>
        /// <param>
        /// _name - название книги, _author - автор (Имя, Фамилия), _id - уникальный код
        /// </param>
        public string _name { get; set; }
        public string _author { get; set; }
        public int _id { get; private set; }
        private static int generateId;
        static Book()
        {
            generateId = 0;
        }

        public Book(string name, string author)
        {
            _name = name;
            _author = author;
            _id = generateId++;
        }
    }
}
