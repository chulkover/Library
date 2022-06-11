using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    [Serializable]
    class BookDepository
    {
        /// <summary>
        /// Класс предназначен для реализации сущности "хранилище книг"
        /// </summary>
        /// <param>
        /// _groupBooks - группы книг, 1 объект == 1 группа; _generalCount - общее количество книг в хранилище
        /// </param>
        internal List<GroupBooks> _groupBooks { get; private set; }
        internal int _generalCount
        {
            get
            {
                int temp = 0;
                foreach(var group in _groupBooks)
                {
                    temp += group._count;
                }
                return temp;
            }
        }

        internal BookDepository()
        {
            _groupBooks = new List<GroupBooks>();
        }
        internal Book GiveBook(string name,string author)
        {
            GroupBooks groupBooks = FindGroup(name, author);
            if (groupBooks == null)
                return null;
            return groupBooks.GiveBook();
        }
        internal void Add(Book book)
        {
            GroupBooks temp = FindGroup(book._name, book._author);
            if (temp != null)
                temp.Add(book);
            else
                throw new ArgumentException("Group doesn't exist");
        }
        internal void Add(string name, string author)
        {
            GroupBooks temp = FindGroup(name, author);
            if (temp != null)
                temp.Add(name,author);
            else
                throw new ArgumentException("Group doesn't exist");
        }
        internal bool AddGroup(string name, string author)
        {
            if (FindGroup(name, author) == null)
                _groupBooks.Add(new GroupBooks(name, author));
            else
                return false;
            return true;
        }
        private GroupBooks FindGroup(string name,string author)
        {
            foreach(var group in _groupBooks)
            {
                if (group._name == name && group._author == author)
                    return group;
            }
            return null;
        }

    }
}
