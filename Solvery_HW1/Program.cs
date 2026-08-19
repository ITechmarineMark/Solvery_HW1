using Solvery_HW1;
using System.Drawing;
using System.Runtime.CompilerServices;

string optionShow = "[1.]   Вывести все отели";
string optionShowFree = "[2.]   Вывести отели, где есть свободные места";
string optionBook = "[3.]   Забронировать номер";
string optionOwnBooking = "[4.]   Вывести свои бронирования";

string pathDZ = "..\\..\\..\\dz_1.txt";
string pathRes = "..\\..\\..\\reservations.txt";

List<Hotel> hotels = new List<Hotel>();
List<Reservation> reservations = new List<Reservation>();
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

// функции
void ShowOptions()
{
    Console.WriteLine();
    Console.WriteLine(optionShow);
    Console.WriteLine(optionShowFree);
    Console.WriteLine(optionBook);
    Console.WriteLine(optionOwnBooking);
    Console.WriteLine();

    Console.Write("Выберите: ");
    int x = Int32.Parse(Console.ReadLine());
    if (x == 1)
    {
        ShowAllHoltels();
        ShowOptions();
    }
    else if (x == 2)
    {
        ShowAllFreeHotels();
        ShowOptions();
    }
    else if (x == 3)
    {
        Book();
        ShowOptions();
    }
    else if (x == 4)
    {
        ShowOwnBooking();
        ShowOptions();
    }
}

void ShowAllHoltels()
{
    foreach (Hotel hotel in hotels)
    {
        Console.Write($"{hotel.HotelId} ");
        Console.Write($"{hotel.HotelName} ");
        Console.Write($"{hotel.HotelAddress}\n");
    }
}

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

void Book()
{
    Console.Write("Введите ID отеля и свое имя: ");
    string answer = Console.ReadLine();
    string[] splitted = answer.Split(' ');
    int size = reservations.Count();
    int counterRes = 0;
    int hotelId = Int32.Parse(splitted[0]);
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

void ShowOwnBooking()
{
    foreach (Reservation reservation in reservations)
    {
        Console.Write($"{reservation.ReservationId} ");
        Console.Write($"{reservation.ClientName} ");
        Console.Write($"{reservation.HotelId}\n");
    }
}

// инстанцируем отели
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