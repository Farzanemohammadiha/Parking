using System;
using System.Collections.Generic;

namespace SimpleParkingApp
{
    class Car
    {
        public string PlateNumber { get; set; }
        public string Model { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public int TotalCost { get; set; }

        public void ShowInfo()
        {
            string exitText;
            if (ExitTime != null)
            {
                exitText = ExitTime.ToString();
            }
            else
            {
                exitText = "Still in parking";
            }

            string costText;
            if (TotalCost > 0)
            {
                costText = TotalCost + " Toman";
            }
            else
            {
                costText = "Not calculated";
            }

            Console.WriteLine("Plate: " + PlateNumber + " - Model: " + Model);
            Console.WriteLine("Entry time: " + EntryTime);
            Console.WriteLine("Exit time: " + exitText);
            Console.WriteLine("Cost: " + costText);
        }
    }

    class Parking
    {
        private int capacity;
        private const int FixedFee = 50;
        private const int RatePerMinute = 1000;

        private Dictionary<string, Car> parkedCars = new Dictionary<string, Car>();
        private List<Car> exitedCars = new List<Car>();

        public Parking(int capacity)
        {
            this.capacity = capacity;
        }

        public bool EnterCar(Car car)
        {
            if (parkedCars.Count >= capacity)
            {
                Console.WriteLine("Parking is full.");
                return false;
            }

            if (parkedCars.ContainsKey(car.PlateNumber))
            {
                Console.WriteLine("This car is already parked.");
                return false;
            }

            car.EntryTime = DateTime.Now;
            parkedCars[car.PlateNumber] = car;

            Console.WriteLine("Car entered. Fixed fee: " + FixedFee);
            return true;
        }

        public bool ExitCar(string plateNumber)
        {
            if (!parkedCars.ContainsKey(plateNumber))
            {
                Console.WriteLine("Car not found.");
                return false;
            }

            Car car = parkedCars[plateNumber];
            car.ExitTime = DateTime.Now;

            TimeSpan duration = car.ExitTime.Value - car.EntryTime;
            int totalMinutes = (int)Math.Ceiling(duration.TotalMinutes);
            car.TotalCost = FixedFee + (totalMinutes * RatePerMinute);

            exitedCars.Add(car);
            parkedCars.Remove(plateNumber);

            Console.WriteLine("Car exited.");
            Console.WriteLine("Entry time: " + car.EntryTime);
            Console.WriteLine("Exit time: " + car.ExitTime);
            Console.WriteLine("Duration: " + totalMinutes + " minutes");
            Console.WriteLine("Total cost: " + car.TotalCost + " Toman");

            return true;
        }

        public void ShowParkedCars()
        {
            if (parkedCars.Count == 0)
            {
                Console.WriteLine("No cars in parking.");
                return;
            }

            Console.WriteLine("Cars currently parked:");
            foreach (var car in parkedCars.Values)
            {
                car.ShowInfo();
                Console.WriteLine("------------------");
            }
        }

        public void ShowExitedCars()
        {
            if (exitedCars.Count == 0)
            {
                Console.WriteLine("No cars have exited yet.");
                return;
            }

            Console.WriteLine("Exited cars:");
            foreach (var car in exitedCars)
            {
                car.ShowInfo();
                Console.WriteLine("------------------");
            }
        }

        public void ShowFreeSpaces()
        {
            int freeSpaces = capacity - parkedCars.Count;
            Console.WriteLine("Total capacity: " + capacity + ", Free spaces: " + freeSpaces);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parking parking = new Parking(5);

            while (true)
            {
                Console.WriteLine("\n***** MENU ******");
                Console.WriteLine("1. Enter car");
                Console.WriteLine("2. Exit car");
                Console.WriteLine("3. Show parked cars");
                Console.WriteLine("4. Show exited cars");
                Console.WriteLine("5. Show free spaces");
                Console.WriteLine("6. Exit program");
                Console.Write("Choose: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Enter plate number: ");
                    string plate = Console.ReadLine();

                    Console.Write("Enter model: ");
                    string model = Console.ReadLine();

                    Car car = new Car
                    {
                        PlateNumber = plate,
                        Model = model
                    };

                    parking.EnterCar(car);
                }
                else if (choice == "2")
                {
                    Console.Write("Enter plate number: ");
                    string plate = Console.ReadLine();

                    parking.ExitCar(plate);
                }
                else if (choice == "3")
                {
                    parking.ShowParkedCars();
                }
                else if (choice == "4")
                {
                    parking.ShowExitedCars();
                }
                else if (choice == "5")
                {
                    parking.ShowFreeSpaces();
                }
                else if (choice == "6")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }
            }
        }
    }
}
