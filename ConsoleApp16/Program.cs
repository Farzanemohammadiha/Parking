using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Parking parking = new Parking();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- PARKING MENU ---");
            Console.WriteLine("1. Enter a car");
            Console.WriteLine("2. Exit a car");
            Console.WriteLine("3. Show exited cars");
            Console.WriteLine("4. Exit program");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Cars carIn = new Cars();
                    Console.Write("Enter plate number: ");
                    carIn.plateNumber = Console.ReadLine();
                    Console.Write("Enter model: ");
                    carIn.model = Console.ReadLine();
                    parking.carsIn(carIn);
                    break;

                case "2":
                    Cars carOut = new Cars();
                    Console.Write("Enter plate number to exit: ");
                    carOut.plateNumber = Console.ReadLine();
                    parking.carsOut(carOut);
                    break;

                case "3":
                    parking.show();
                    break;

                case "4":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }

    public class Cars
    {
        public string model { get; set; }
        public string plateNumber { get; set; }
    }

    public class Parking
    {
        Dictionary<string, (DateTime entryTime, string model)> parkedCars = new Dictionary<string, (DateTime, string)>();
        Dictionary<string, (DateTime entry, DateTime exit, double minutes, int hourlyFee, int totalFee)> exitedCars
         = new Dictionary<string, (DateTime, DateTime, double, int, int)>();

        const int entryCost = 50;

        public void carsIn(Cars car)
        {
            if (!parkedCars.ContainsKey(car.plateNumber))
            {
                parkedCars[car.plateNumber] = (DateTime.Now, car.model);
                Console.WriteLine($"[ENTRY] Car with plate {car.plateNumber} entered at {DateTime.Now}, model: {car.model}");
            }
            else
            {
                Console.WriteLine("Car is already in the parking!");
            }
        }

        public void carsOut(Cars car)
        {
            if (parkedCars.ContainsKey(car.plateNumber))
            {
                var carInfo = parkedCars[car.plateNumber];
                DateTime entryTime = carInfo.entryTime;
                string model = carInfo.model;
                DateTime exitTime = DateTime.Now;

                TimeSpan duration = exitTime - entryTime;
                int totalHours = (int)Math.Ceiling(duration.TotalHours);
                int feePerHour = 100;
                int hourlyFee = totalHours * feePerHour;
                int totalFee = entryCost + hourlyFee;

                Console.WriteLine($"\n--- CAR EXIT INFO ---");
                Console.WriteLine($"Plate: {car.plateNumber}");
                Console.WriteLine($"Model: {model}");
                Console.WriteLine($"Entry Time: {entryTime}");
                Console.WriteLine($"Exit Time: {exitTime}");
                Console.WriteLine($"Total Duration: {duration.TotalMinutes:F1} minutes");
                Console.WriteLine($"Charged Hours: {totalHours} hour(s)");
                Console.WriteLine($"Fixed Entry Fee: {entryCost} Toman");
                Console.WriteLine($"Hourly Fee     : {hourlyFee} Toman");
                Console.WriteLine($"Total Fee      : {totalFee} Toman");
                Console.WriteLine("-----------------------\n");

                exitedCars[car.plateNumber] = (entryTime, exitTime, duration.TotalMinutes, hourlyFee, totalFee);
                parkedCars.Remove(car.plateNumber);
            }
            else
            {
                Console.WriteLine("This car is not found in the parking!");
            }
        }

        public void show()
        {
            Console.WriteLine("\n=== CARS THAT EXITED THE PARKING ===");

            if (exitedCars.Count == 0)
            {
                Console.WriteLine("No exited cars yet.");
                return;
            }

            foreach (var item in exitedCars)
            {
                string plate = item.Key;
                var info = item.Value;

                Console.WriteLine($"Plate: {plate}");
                Console.WriteLine($"Entry Time: {info.entry}");
                Console.WriteLine($"Exit Time : {info.exit}");
                Console.WriteLine($"Total Duration : {info.minutes:F1} minutes");
                Console.WriteLine($"Hourly Fee Paid: {info.hourlyFee} Toman");
                Console.WriteLine($"Fixed Entry Fee: {entryCost} Toman");
                Console.WriteLine($"Total Paid     : {info.totalFee} Toman");
                Console.WriteLine("-----------------------");
            }
        }
    }
}
   