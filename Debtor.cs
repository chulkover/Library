using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    [Serializable]
    class Debtor
    {
        /// <summary>
        /// Класс предназначен для реализации сущности должника (человек, взявший книгу)
        /// </summary>
        /// <param>
        /// _id - уникальный код, _client - общая информация, _timeOfReceipt - время взятия, _timeOfReturn - время возврата
        /// </param>
        public int _id { get; set; }
        public Client _client { get; set; }
        public DateTime _timeOfReceipt { get; set; }
        public DateTime _timeOfReturn { get; set; }
        public Book _book { get;  set; }
        public static int GenerateId = 0;

        public Debtor(Client client,DateTime start,DateTime finish,Book book)
        {
            _client = client;
            _timeOfReceipt = start;
            _timeOfReturn = finish;
            _book = book;
            _id = GenerateId++;
        }
    }
}
