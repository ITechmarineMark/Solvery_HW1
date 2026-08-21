using Solvery_HW1;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Runtime.CompilerServices;

string optionShow = "[1.]   Вывести все отели";
string optionShowFree = "[2.]   Вывести отели, где есть свободные места";
string optionBook = "[3.]   Забронировать номер";
string optionOwnBooking = "[4.]   Вывести свои бронирования";

string pathDZ = "..\\..\\..\\dz_1.txt";
string pathRes = "..\\..\\..\\reservations.txt";

// заменено на var, вместо явного объявления
var hotels = new List<Hotel>();
var reservations = new List<Reservation>();
// выполнение программы
// создаем reservations.txt, если его нет
if (!File.Exists((pathRes)))
{
    var sw = File.CreateText(pathRes);
    sw.Write("ReservationId\t");
    sw.Write("ClientName\t");
    sw.WriteLine("HotelId");
    sw.Close();
}
LoadHotelsAndReservations(pathDZ, pathRes);
ShowOptions();

/// <summary>
/// Вывод функций доступных для
/// пользователя.
/// Метод считывает ввод с консоли
/// и парсит в тип int.
/// Затем идет обработка switch-case
/// и в зависимости от выбранной
/// опции (1-4), выполняется метод:
/// ShowAllHotels()
/// ShowAllFreeHotels()
/// Book()
/// ShowOwnBooking()
/// А также заново запускается метод
/// ShowOptions() (по требованию)
/// </summary>
void ShowOptions()
{
    Console.WriteLine();
    Console.WriteLine(optionShow);
    Console.WriteLine(optionShowFree);
    Console.WriteLine(optionBook);
    Console.WriteLine(optionOwnBooking);
    Console.WriteLine();

    Console.Write("Выберите: ");
    // заменено с Int32 на int.Parse()
    int answer = int.Parse(Console.ReadLine());
    // заменено на switch case (был if)
    switch (answer)
    {
        case 1:
            ShowAllHotels();
            ShowOptions();
            break;

        case 2:
            ShowAllFreeHotels();
            ShowOptions();
            break;

        case 3:
            Book();
            ShowOptions();
            break;

        case 4:
            ShowOwnBooking();
            ShowOptions();
            break;
        
        default:
            ShowOptions();
            break;
    } 
}
/// <summary>
/// Показать все отели.
/// Через цикл foreach выводится
/// HotelId, HotelName, HotelAddress
/// каждого отеля
/// </summary>
void ShowAllHotels()
{
    foreach (Hotel hotel in hotels)
    {
        // заменено с Write на WriteLine
        Console.WriteLine($"{hotel.HotelId} ");
        Console.Write($"{hotel.HotelName} ");
        Console.Write($"{hotel.HotelAddress}");
    }
}
/// <summary>
/// Показать все доступные (свободные)
/// для бронирования отели.
/// Используется два цикла foreach
/// для соотношения пары:
/// HotelId в классе Hotel и Reservation.
/// При совпадении значений переменная
/// counter прибавляет +1
/// </summary>
void ShowAllFreeHotels()
{
    int counter = 0;
    foreach (Hotel hotel in hotels)
    {
        foreach (Reservation reservation in reservations)
        {
            if (reservation.HotelId == hotel.HotelId)
            {
                counter++;
            }
        }
        if (counter < hotel.Capacity)
        {
            Console.Write($"{hotel.HotelId} ");
            Console.Write($"{hotel.HotelName} ");
            Console.Write($"{hotel.HotelAddress} | ");
            Console.Write($"{hotel.Capacity - counter}\n");
        }
        counter = 0;
    }
}
/// <summary>
/// Забронировать отель.
/// Ввод пользователя сплитуется .Split(' '),
/// splitted[0] - число, выбранный HotelId,
/// splitted[1] - имя бронирующего
/// Цикл foreach смотрит, есть ли свободные места.
/// Если нет, выводит "Ошибка бронирования".
/// Если свободно, создается объект и добавляется в List
/// и делается запись в файл reservations.txt
/// </summary>
void Book()
{
    Console.Write("Введите ID отеля и свое имя: ");
    string answer = Console.ReadLine();
    string[] splitted = answer.Split(' ');
    int size = reservations.Count();
    int counterRes = 0;
    // Заменено с Int32 на int.Parse()
    int hotelId = int.Parse(splitted[0]);
    // считаем сколько мест забронировано для конкретного отеля
    foreach (Reservation reservation in reservations) {
        if (reservation.HotelId == hotelId)
        {
            counterRes++;
        }
    }
    if (hotels[hotelId].Capacity <= counterRes) 
    {
        Console.WriteLine("Ошибка бронирования");
    }
    else
    {
        reservations.Add(new Reservation(
                size,
                splitted[1],
                Int32.Parse(splitted[0])));

        var sw = File.AppendText(pathRes);
        sw.WriteLine($"{size}\t{splitted[1]}\t{hotelId}");
        sw.Close();
    }
}
/// <summary>
/// Показать свои бронирования.
/// Через цикл foreach выводятся бронирования.
/// </summary>
void ShowOwnBooking()
{
    foreach (Reservation reservation in reservations)
    {
        Console.Write($"{reservation.ReservationId} ");
        Console.Write($"{reservation.ClientName} ");
        Console.Write($"{reservation.HotelId}\n");
    }
}
/// <summary>
/// Загрузка отелей и бронирований.
/// Через while бегаем по файлу
/// и создаем объекты типа Hotel
/// и Reservation, а затем добавляем их
/// в два List'а
/// </summary>
void LoadHotelsAndReservations(string pathH, string pathR)
{
    // отели
    bool isFirst = true;
    string line;
    StreamReader sr = new StreamReader(pathH);
    while ((line = sr.ReadLine()) != null)
    {
        if (isFirst) 
        {
            isFirst = false;
            continue; 
        }
        string[] splitted = line.Split('\t');
        hotels.Add(new Hotel(
            Int32.Parse(splitted[0]),
            splitted[1],
            splitted[2],
            splitted[3],
            Int32.Parse(splitted[4])));
    }
    // бронирования
    isFirst = true;
    sr = new StreamReader(pathR);
    while ((line = sr.ReadLine()) != null)
    {
        if (isFirst)
        {
            isFirst = false;
            continue;
        }
        string[] splitted = line.Split('\t');
        reservations.Add(new Reservation(
            Int32.Parse(splitted[0]),
            splitted[1],
            Int32.Parse(splitted[2])));
    }
    sr.Close();
}