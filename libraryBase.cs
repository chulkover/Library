using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    [Serializable]
    class libraryBase
    {
        /// <summary>
        /// Класс предназначен для реализации сущности "база данных"; отвечает за вывод информации в таблицу
        /// </summary>
        /// <param>
        /// _bookDepository - хранилище книг, _debtors - должники
        /// </param>
        public BookDepository _bookDepository { get; private set; }
        public List<Debtor> _debtors { get; set; }
        public libraryBase()
        {
            _bookDepository = new BookDepository();
            _debtors = new List<Debtor>();
        }
        public bool AddDebtor(string name,string surname,string patronymic,string nameBook,string author,DateTime start,DateTime finish)
        {
            Client clientAdd=null;
            foreach(var item in _debtors)
            {
                var client = item._client;
                if(client._name==name && client._surname==surname && client._patronymic==patronymic)
                {
                    clientAdd = client;
                    break;
                }
            }
            if (clientAdd == null)
                clientAdd = new Client(name, surname, patronymic);
            Book book=_bookDepository.GiveBook(nameBook, author);
            if (book == null)
                return false;
            _debtors.Add(new Debtor(clientAdd, start, finish, book));
            return true;
        }


        internal Debtor TakeFirstDebtorWithNameBook(string name)
        {
            foreach(var item in _debtors)
            {
                if (item._book._name == name || item._book._name.ToLower()==name)
                    return item;
            }
            return null;
        }
        internal Debtor TakeFirstDebtorWithAuthorBook(string author)
        {
            foreach (var item in _debtors)
            {
                if (item._book._author == author || item._book._author.ToLower() == author)
                    return item;
            }
            return null;
        }

        internal void Remove(Debtor item)
        {
            _debtors.Remove(item);
        }

        internal bool Remove(int id)
        {
            foreach (var item in _debtors)
                if (item._id == id)
                {
                    _debtors.Remove(item);
                    return true;
                }
            return false;
        }

        internal Debtor TakeDebtorId(int id)
        {
            foreach (var debtor in _debtors)
                if (debtor._id == id)
                    return debtor;
            return null;
        }
    }
}
