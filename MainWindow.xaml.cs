using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private libraryBase library;
        private ObservableCollection<Debtor> observable;
        private ObservableCollection<ViewDebtor> observableDebtors;
        private ObservableCollection<ViewBook> observableBooks;
        private HelloForm hello;

        private List<Control> AddBook;
        public MainWindow()
        {

            library = new libraryBase();
            observable = new ObservableCollection<Debtor>();
            observableDebtors = new ObservableCollection<ViewDebtor>();
            observableBooks = new ObservableCollection<ViewBook>();

            var bin = new BinaryFormatter();
            using (var file = new FileStream("Library.bin", FileMode.OpenOrCreate))
            {
                try
                {
                    library = bin.Deserialize(file) as libraryBase;
                    file.Close();
                    int Max = 0;
                    foreach (var item in library._debtors)
                    {
                        if (item._id > Max)
                            Max = item._id;
                    }
                    Debtor.GenerateId = Max;
                }
                catch
                {
                    library = new libraryBase();
                }
            }
            CreateFictiouns();
            InitializeComponent();

            CreateObservableCollection();
            UpdateViewInfoDebtors();
            CreateListAddBook();
            hello = new HelloForm();
            hello.Show();
        }
        private void CreateListAddBook()
        {
            AddBook = new List<Control>();
            AddBook.Add(BooksForAdd);
            AddBook.Add(TextBoxNameAdd);
            AddBook.Add(TextBoxAuthorAdd);
            AddBook.Add(LabelAuthorAdd);
            AddBook.Add(LabelNameAdd);
            AddBook.Add(ButtonConfirmAdd);
            foreach(var item in AddBook)
            {
                item.Visibility = Visibility.Hidden;
            }

        }
        private void CreateObservableCollection()
        {
            foreach(var item in library._debtors)
            {
                observable.Add(item);
            }
            MyDataGrid.ItemsSource = observable;
        }
        private void CreateFictiouns()
        {
            string[] namesArr = { "Антон", "Дмитрий", "Александр", "Данила", "Егор", "Георгий", "Григорий", "Иван", "Денис", "Игорь", "Леонид", "Даниил", "Михаил", "Алексей", "Никита", "Артем", "Владимир" };
            string[] surnamesArr = { "Шолохов", "Антонов", "Нагарев", "Романовский", "Савельев", "Щипачев", "Телегин", "Перевертов", "Попов", "Картошкин", "Капустин", "Морбит", "Медведев", "Арзамасцев", "Петухов", "Шишкин" };
            string[] otchestvosArr = { "Дмитриевич", "Васильевич", "Александрович", "Федорович", "Владимирович", "Ахметович", "Вадимович", "Леонидович", "Аркадьевич", "Ахматович", "Алексеевич", "Михайлович", "Денисович", "Данилович" };
            Book[] books = {new Book("Война и мир","Лев Толстой"), new Book("Горе от ума", "Александр Грибоедов"), new Book("Евгений Онегин", "Александр Пушкин"), new Book("Беседа пьяного с трезвым чертом", "Антон Чехов"),
            new Book("Чудесный доктор","Александр Куприн"),new Book("Герой нашего времени","Михаил Лермонтов"),new Book("Ашик-Кериб","Михаил Лермонтов"),new Book("Ревизор","Николай Гоголь"),
            new Book("Что значит досуг","Владимир Даль"),new Book("Идиот","Федор Достоевский"),new Book("Муму","Иван Тургенев"),new Book("Недоросль","Денис Фонвизин"),new Book("Фауст","Иоганн В. Гёте"), new Book("Дивный новый мир","Олдос Хаксли"), new Book("Аэропорт","Артур Хейли")};

            foreach (var book in books)
            {
                library._bookDepository.AddGroup(book._name, book._author);
            }
            Random random = new Random();
            for (int i =0;i<10000;i++)
            {
                library._bookDepository.Add(books[random.Next(0, books.Length)]);
            }
            for (int i = 0; i < 5000; i++)
            {
                Book book = books[random.Next(0, books.Length)];
                DateTime timeStart = RandomDay(random);
                DateTime finish = FinishTime(random, timeStart);
                library.AddDebtor(namesArr[random.Next(0, namesArr.Length)], surnamesArr[random.Next(0, surnamesArr.Length)],
                    otchestvosArr[random.Next(0, otchestvosArr.Length)], book._name, book._author, timeStart, finish);
            }
            
        }
        private void UpdateViewInfoDebtors()
        {
            observableDebtors.Clear();
            HashSet<Client> _closedClient = new HashSet<Client>();
            foreach(var debtor in library._debtors)
            {
                if (_closedClient.Contains(debtor._client))
                    continue;
                int count = 0;
                foreach(var item in library._debtors)
                {
                    if (debtor._client._key == item._client._key)
                        count++;
                }
                _closedClient.Add(debtor._client);
                observableDebtors.Add(new ViewDebtor(debtor._client,count));
            }
            DataGridWithInfo.ItemsSource = observableDebtors;
        }
        private void UpdateViewInfoBooks()
        {
            observableBooks.Clear();
            foreach (var item in library._bookDepository._groupBooks)
            {
                observableBooks.Add(new ViewBook(item._name, item._author, item._count, observableBooks.Count+1));
            }
            DataBooks.ItemsSource = observableBooks;
        }
        private DateTime FinishTime(Random gen,DateTime time)
        {
            int year = time.Year + gen.Next(0, 3);
            var month = gen.Next(1, 12);
            var day = gen.Next(1, 28);
            DateTime returnTime = new DateTime(year, month, day);
            while (month>12 || day>28 || returnTime.Year>DateTime.Now.Year+8)
            {
                month = gen.Next(1, 12);
                day = gen.Next(1, 28);
                year = time.Year + gen.Next(2018, 2025);
                returnTime = new DateTime(year,month,day);
            }
            return returnTime;
        }
        private DateTime RandomDay(Random gen)
        {
            DateTime start = new DateTime(2018, 1, 1);
            int range = (new DateTime(2022, 1, 1) - start).Days;
            return start.AddDays(gen.Next(range));
        }

        private void ChangeElementOnGrid(DataGrid dg,object element)
        {
            dg.UpdateLayout();
            dg.SelectedItem = element;
            dg.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            dg.ScrollIntoView(MyDataGrid.SelectedItem, null);
        }
        private void LookForButton_Click(object sender, RoutedEventArgs e)
        {
            string name  = LookForText.Text.ToString().ToLower();
            Debtor debtor;
            //Поиск по названию
            if ((debtor = library.TakeFirstDebtorWithNameBook(name))!=null)
            {
                ChangeElementOnGrid(MyDataGrid, debtor);
                return;

            }
            //Поиск по автору
            if ((debtor = library.TakeFirstDebtorWithAuthorBook(name)) != null)
            {
                ChangeElementOnGrid(MyDataGrid, debtor);
                return;
            }
            //Поиск по Id
            if (int.TryParse(LookForText.Text, out int ID) && library._debtors.Any(x=> (debtor=x)._id==ID))
            {
                ChangeElementOnGrid(MyDataGrid, debtor);
                return;
            }
        }

        private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
        {
            IList<Debtor> temp;
            if ((temp = MyDataGrid.SelectedItems.Cast<Debtor>().ToList()).Count > 0)
            {
                foreach (var item in temp)
                {
                    library.Remove(item);
                    observable.Remove(item);
                }
                return;
            }
            if (int.TryParse(DeleteOrder.Text, out int id))
            {
                Debtor debtor = library.TakeDebtorId(id);
                if (debtor==null)
                    DeleteOrder.Text = "Объект не найден";
                else
                {
                    library.Remove(debtor);
                    observable.Remove(debtor);
                }
            }
        }

        private void SaveBDButton_Click(object sender, RoutedEventArgs e)
        {
            var bin = new BinaryFormatter();
            using (var file = new FileStream("Library.bin", FileMode.OpenOrCreate))
            {
                bin.Serialize(file, library);
                file.Close();
            }
        }

        private void DeleteAllBaseData_Click(object sender, RoutedEventArgs e)
        {
            FileInfo fileInf = new FileInfo("Library.bin");
            fileInf.Delete();
        }

        private void UpdateComboBoxBooks(ComboBox cm)
        {
            cm.Items.Clear();
            foreach(var item in library._bookDepository._groupBooks)
            {
                if (item._count != 0)
                    cm.Items.Add(item._name+" - "+item._author);
            }
            cm.Text = "Выбор книги";    
        }

        private void TabItem_Selected(object sender, RoutedEventArgs e)
        {
            ComboBooks.SelectionChanged -= ComboBooks_SelectionChanged;
            LabelMessage.Content = "";
            ClearAddOrderCotrols();
            UpdateComboBoxBooks(ComboBooks);
            ComboBooks.SelectionChanged += ComboBooks_SelectionChanged;
        }

        private void ComboBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboBoxText = (sender as ComboBox).SelectedItem.ToString();
            var strings = comboBoxText.Split(" - ");
            if (strings.Length < 2)
                throw new Exception();
            TextBoxNameBook.Text = strings[0];
            TextBoxAuthor.Text = strings[1];
        }
        private void SendMessage(string message,Brush brush)
        {
            LabelMessage.Content = message;
            LabelMessage.Foreground = brush;
        }
        private void ButtonCreateDebtor_Click(object sender, RoutedEventArgs e)
        {
            if(ComboBooks.SelectedIndex==-1)
            {
                SendMessage("Выберите книгу", Brushes.Red);
                return;
            }
            if (TextBoxName.Text=="")
            {
                SendMessage("Введите имя", Brushes.Red);
                return;
            }
            if (TextBoxSurname.Text == "")
            {
                SendMessage("Введите фамилию", Brushes.Red);
                return;
            }
            if (TextBoxPatronymic.Text == "")
            {
                SendMessage("Введите отчество", Brushes.Red);
                return;
            }
            if (DateStart.SelectedDate==null || DateFinish.SelectedDate==null)
            {
                SendMessage("Выберите обе даты", Brushes.Red);
                return;
            }
            if(DateStart.SelectedDate>DateFinish.SelectedDate)
            {
                SendMessage("Дата получения старше даты возвращения", Brushes.Red);
                return;
            }
            string nameClient = TextBoxName.Text;
            string surnameClient = TextBoxSurname.Text;
            string patronymicClient = TextBoxPatronymic.Text;
            library.AddDebtor(nameClient, surnameClient, patronymicClient, TextBoxNameBook.Text, TextBoxAuthor.Text, DateStart.SelectedDate.Value, DateFinish.SelectedDate.Value);
            SendMessage("Вы взяли книгу!", Brushes.Green);
            ComboBooks.SelectionChanged -= ComboBooks_SelectionChanged;
            UpdateComboBoxBooks(ComboBooks);
            ComboBooks.SelectionChanged += ComboBooks_SelectionChanged;
            ClearAddOrderCotrols();
            observable.Add(library._debtors.Last());
        }
        private void ClearAddOrderCotrols()
        {
            TextBoxName.Text = "";
            TextBoxSurname.Text = "";
            TextBoxPatronymic.Text = "";
            DateStart.SelectedDate = null;
            DateFinish.SelectedDate = null;
            TextBoxNameBook.Text = "";
            TextBoxAuthor.Text = "";
        }

        private string ReplaceSeveralCharOne(string baseStr, string symbol)
        {
            string temp = "";
            foreach (var myChar in baseStr)
            {
                if (myChar.ToString() == symbol && temp.Length > 0)
                {
                    if (temp[temp.Length - 1].ToString() != symbol)
                    {
                        temp += myChar.ToString();
                        continue;
                    }
                    continue;
                }
                temp += myChar.ToString();
            }
            return temp;
        }
        private void MyDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    var debtor = e.Row.Item as Debtor;
                    if (bindingPath == typeof(Debtor).GetProperty("_timeOfReceipt").Name)
                    {
                        var el = e.EditingElement as TextBox;
                        var temp = ReplaceSeveralCharOne(el.Text.Trim(), " ");
                        var formats = new String[] {
                                       "MM/dd/yyyy hh:mm:ss tt",
                                       "M/dd/yyyy hh:mm:ss tt",
                                       "MM/d/yyyy hh:mm:ss tt",
                                       "M/d/yyyy hh:mm:ss tt",
                                       "MM/dd/yyyy hh:mm:ss",
                                       "M/dd/yyyy hh:mm:ss",
                                       "MM/d/yyyy hh:mm:ss",
                                       "M/d/yyyy hh:mm:ss",
                                       "MM/dd/yyyy",
                                       "M/dd/yyyy",
                                       "MM/d/yyyy",
                                       "M/d/yyyy",
                                       "d-M-yyyy"};
                        if (!DateTime.TryParseExact(temp, formats,System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt))
                        {
                            MyDataGrid.CancelEdit();
                            return;
                        }
                        if (debtor._timeOfReturn< dt)
                        {
                            MyDataGrid.CancelEdit();
                            return;
                        }
                    }
                    if (bindingPath == typeof(Debtor).GetProperty("_timeOfReturn").Name)
                    {
                        var el = e.EditingElement as TextBox;
                        var temp = ReplaceSeveralCharOne(el.Text.Trim(), " ");
                        var formats = new String[] {
                                       "MM/dd/yyyy hh:mm:ss tt",
                                       "M/dd/yyyy hh:mm:ss tt",
                                       "MM/d/yyyy hh:mm:ss tt",
                                       "M/d/yyyy hh:mm:ss tt",
                                       "MM/dd/yyyy hh:mm:ss",
                                       "M/dd/yyyy hh:mm:ss",
                                       "MM/d/yyyy hh:mm:ss",
                                       "M/d/yyyy hh:mm:ss",
                                       "MM/dd/yyyy",
                                       "M/dd/yyyy",
                                       "MM/d/yyyy",
                                       "M/d/yyyy",
                                       "d-M-yyyy"};
                        if (!DateTime.TryParseExact(temp, formats, System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dt))
                        {
                            MyDataGrid.CancelEdit();
                            return;
                        }
                        if (debtor._timeOfReceipt > dt)
                        {
                            MyDataGrid.CancelEdit();
                            return;
                        }
                    }
                }
            }
        }

        private void TabItem_Selected_1(object sender, RoutedEventArgs e)
        {
            UpdateViewInfoDebtors();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataBooks.Visibility = Visibility.Hidden;
            ClearAddControls();
            foreach (var item in AddBook)
            {
                item.Visibility = Visibility.Hidden;
            }

            UpdateViewInfoDebtors();
            DataGridWithInfo.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataGridWithInfo.Visibility = Visibility.Hidden;
            ClearAddControls();
            foreach (var item in AddBook)
            {
                item.Visibility = Visibility.Hidden;
            }
            UpdateViewInfoBooks();
            DataBooks.Visibility = Visibility.Visible;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UpdateComboBoxBooks(BooksForAdd);
            if(ButtonConfirmAdd.Visibility==Visibility.Hidden)
            {
                ClearAddControls();
                foreach (var item in AddBook)
                {
                    item.Visibility = Visibility.Visible;
                }
            }
            else
            {
                foreach(var item in AddBook)
                {
                    item.Visibility = Visibility.Hidden;
                }
            }
        }

        private void BooksForAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem;
            if (item == null)
                return;
            string comboBoxText = item.ToString();
            var strings = comboBoxText.Split(" - ");
            if (strings.Length < 2)
                throw new Exception();
            TextBoxNameAdd.Text = strings[0];
            TextBoxAuthorAdd.Text = strings[1];
        }

        private void TextBoxNameAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            var item = BooksForAdd.SelectedItem;
            if (item != null)
            {
                var strings = item.ToString().Split(" - ");
                if(strings[0]!=TextBoxNameAdd.Text)
                {
                    BooksForAdd.SelectedItem = null;
                    BooksForAdd.Text = "Выбор книги";
                    return;
                }
            }
            foreach(var text in BooksForAdd.Items)
            {
                var strings = text.ToString().Split(" - ");
                if (strings.Length < 2)
                    return;
                if (TextBoxNameAdd.Text==strings[0] && TextBoxAuthorAdd.Text==strings[1])
                {
                    BooksForAdd.SelectedItem = text.ToString();
                    return;
                }
            }
        }

        private void TextBoxAuthorAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            var item = BooksForAdd.SelectedItem;
            if (item != null)
            {
                var strings = item.ToString().Split(" - ");
                if (strings[1] != TextBoxAuthorAdd.Text)
                {
                    BooksForAdd.SelectedItem = null;
                    BooksForAdd.Text = "Выбор книги";
                    return;
                }
            }
            foreach (var text in BooksForAdd.Items)
            {
                var strings = text.ToString().Split(" - ");
                if (strings.Length < 2)
                    return;
                if (TextBoxNameAdd.Text == strings[0] && TextBoxAuthorAdd.Text == strings[1])
                {
                    BooksForAdd.SelectedItem = text.ToString();
                    return;
                }
            }
        }
        private void ClearAddControls()
        {
            foreach (var item2 in AddBook)
            {
                item2.Visibility = Visibility.Hidden;
                var cmb = item2 as ComboBox;
                if (cmb != null)
                {
                    cmb.SelectedItem = null;
                    cmb.Text = "Выбор книги";
                    continue;
                }
                var txb = item2 as TextBox;
                if (txb != null)
                {
                    txb.Text = "";
                    continue;
                }
            }
        }
        private void ButtonConfirmAdd_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxAuthorAdd.Text == "")
                return;
            if (TextBoxNameAdd.Text == "")
                return;
            foreach(var item in library._bookDepository._groupBooks)
            {
                if(item._name== TextBoxNameAdd.Text && item._author== TextBoxAuthorAdd.Text)
                {
                    library._bookDepository.Add(TextBoxNameAdd.Text, TextBoxAuthorAdd.Text);
                    ClearAddControls();
                    UpdateViewInfoBooks();
                    return;
                }
            }
            library._bookDepository.AddGroup(TextBoxNameAdd.Text, TextBoxAuthorAdd.Text);
            library._bookDepository.Add(TextBoxNameAdd.Text, TextBoxAuthorAdd.Text);
            ClearAddControls();
            UpdateViewInfoBooks();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (hello != null)
                hello.Close();
        }
    }
}
