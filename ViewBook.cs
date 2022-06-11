using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    class ViewBook
    {
        /// <summary>
        /// Класс предназначен для отображения книг в таблице
        /// </summary>
        /// <param>
        /// _number - номер книги, _name - название, _author - автор, _count - количество
        /// </param>
        public int _number { get; set; }
        public string _name { get; set; }
        public string _author { get; set; }
        public int _count { get; set; }
        public ViewBook(string name,string author, int count,int number)
        {
            _name = name;
            _author = author;
            _count = count;
            _number = number;
        }
    }
}
