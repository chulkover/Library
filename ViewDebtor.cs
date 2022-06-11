using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    class ViewDebtor
    {
        /// <summary>
        /// Класс предназначен для отображения должников в таблице
        /// </summary>
        /// <param>
        /// _client - информация, _count - количество взятых книг
        /// </param>
        public Client _client { get; set;}
        public int _count { get; set; }
        public ViewDebtor(Client client,int count)
        {
            _client = client;
            _count = count;
        }
    }
}
