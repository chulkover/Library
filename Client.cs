using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    [Serializable]
    class Client
    {
        /// <summary>
        /// Класс предназначен для реализации сущности клиента (человека, пришедшего в библиотеку)
        /// </summary>
        /// <param>
        /// _key - уникальный ключ, _name - имя, _surname - фамилия, _patronymic - отчество
        /// </param>
        public int _key { get; private set; }
        public string _name { get;  set; }
        public string _surname { get;  set; }
        public string _patronymic { get;  set; }
        private static int KeyGenerate;
        static Client()
        {
            KeyGenerate = 0;
        }
        public Client(string name, string surname, string pathronymic)
        {
            this._name = name;
            this._surname = surname;
            this._patronymic = pathronymic;
            _key = KeyGenerate++;
        }
    }
}
