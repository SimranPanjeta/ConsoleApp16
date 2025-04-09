/*
* Project Name: LANGHAM Hotel Management System
* Author Name: [Your Name]
* Date: [Date]
* Application Purpose: To manage rooms and customer allocations in a hotel system.
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace Assessment2Task2
{
    // Custom Class - Room
    public class Room
    {
        public int RoomNo { get; set; }
        public bool IsAllocated { get; set; }

        // Constructor
        public Room(int roomNo)
        {
            RoomNo = roomNo;
            IsAllocated = false;
        }
    }

    // Custom Class - Customer
    public class Customer
    {
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }

        // Constructor
        public Customer(int customerNo, string customerName)
        {
            CustomerNo = customerNo;
            CustomerName = customerName;
        }
    }

    // Custom Class - RoomAllocation
    public class RoomAllocation
    {
        public int AllocatedRoomNo { get; set; }
        public Customer AllocatedCustomer { get; set; }

        // Constructor
        public RoomAllocation(int roomNo, Customer customer)
        {
            AllocatedRoomNo = roomNo;
            AllocatedCustomer = customer;
        }
    }

    // Custom Main Class - Program
    class Program
    {
        // Variables declaration and initialization
        public static List<Room> listOfRooms = new List<Room>();
        public static List<RoomAllocation> listOfRoomAllocations = new List<RoomAllocation>();
        public static string filePath;

        // Main function
        static void Main(string[] args)
        {
            // Set the file path in the user's documents folder
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Path.Combine(folderPath, "HotelManagement.txt");

            char ans;
            do
            {
                Console.Clear();
                Console.WriteLine("***********************************************************************************");
                Console.WriteLine(" LANGHAM HOTEL MANAGEMENT SYSTEM");
                Console.WriteLine(" MENU");
                Console.WriteLine("***********************************************************************************");
                Console.WriteLine("1. Add Rooms");
                Console.WriteLine("2. Display Rooms");
                Console.WriteLine("3. Allocate Rooms");
                Console.WriteLine("4. De-Allocate Rooms");
                Console.WriteLine("5. Display Room Allocation Details");
                Console.WriteLine("6. Billing");
                Console.WriteLine("7. Save the Room Allocations To a File");
                Console.WriteLine("8. Show the Room Allocations From a File");
                Console.WriteLine("9. Exit");
                Console.WriteLine("***********************************************************************************");
                Console.Write("Enter Your Choice Number Here: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddRooms();
                        break;
                    case 2:
                        DisplayRooms();
                        break;
                    case 3:
                        AllocateRoomToCustomer();
                        break;
                    case 4:
                        DeAllocateRoomFromCustomer();
                        break;
                    case 5:
                        DisplayRoomAllocations();
                        break;
                    case 6:
                        Console.WriteLine("Billing Feature is Under Construction and will be added soon...!!!");
                        break;
                    case 7:
                        SaveRoomAllocationsToFile();
                        break;
                    case 8:
                        ShowRoomAllocationsFromFile();
                        break;
                    case 9:
                        Console.WriteLine("Exiting the system...");
                        return;
                    default:
                        Console.WriteLine("Invalid option, please choose a valid number.");
                        break;
                }

                Console.Write("\nWould You Like To Continue (Y/N): ");
                ans = Convert.ToChar(Console.ReadLine());
            } while (ans == 'y' || ans == 'Y');
        }

        // Function to Add Rooms
        static void AddRooms()
        {
            Console.Write("Enter the number of rooms to add: ");
            int numberOfRooms = Convert.ToInt32(Console.ReadLine());
            for (int i = 1; i <= numberOfRooms; i++)
            {
                listOfRooms.Add(new Room(i));
                Console.WriteLine($"Room {i} added successfully!");
            }
        }

        // Function to Display Rooms
        static void DisplayRooms()
        {
            Console.WriteLine("\nAvailable Rooms:");
            foreach (var room in listOfRooms)
            {
                string allocationStatus = room.IsAllocated ? "Allocated" : "Available";
                Console.WriteLine($"Room {room.RoomNo} - Status: {allocationStatus}");
            }
        }

        // Function to Allocate Room to Customer
        static void AllocateRoomToCustomer()
        {
            Console.Write("Enter room number to allocate: ");
            int roomNo = Convert.ToInt32(Console.ReadLine());

            var room = listOfRooms.Find(r => r.RoomNo == roomNo);

            if (room != null && !room.IsAllocated)
            {
                Console.Write("Enter customer number: ");
                int customerNo = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter customer name: ");
                string customerName = Console.ReadLine();

                var customer = new Customer(customerNo, customerName);
                var allocation = new RoomAllocation(roomNo, customer);
                listOfRoomAllocations.Add(allocation);

                room.IsAllocated = true; // Mark room as allocated
                Console.WriteLine($"Room {roomNo} allocated to {customerName}.");
            }
            else
            {
                Console.WriteLine("Room not found or already allocated.");
            }
        }

        // Function to De-Allocate Room from Customer
        static void DeAllocateRoomFromCustomer()
        {
            Console.Write("Enter room number to de-allocate: ");
            int roomNo = Convert.ToInt32(Console.ReadLine());

            var roomAllocation = listOfRoomAllocations.Find(ra => ra.AllocatedRoomNo == roomNo);
            if (roomAllocation != null)
            {
                listOfRoomAllocations.Remove(roomAllocation);
                var room = listOfRooms.Find(r => r.RoomNo == roomNo);
                room.IsAllocated = false;
                Console.WriteLine($"Room {roomNo} has been de-allocated.");
            }
            else
            {
                Console.WriteLine("No allocation found for this room.");
            }
        }

        // Function to Display Room Allocation Details
        static void DisplayRoomAllocations()
        {
            Console.WriteLine("\nRoom Allocation Details:");
            foreach (var allocation in listOfRoomAllocations)
            {
                Console.WriteLine($"Room {allocation.AllocatedRoomNo} is allocated to {allocation.AllocatedCustomer.CustomerName}");
            }
        }

        // Function to Save Room Allocations to a File
        static void SaveRoomAllocationsToFile()
        {
            using (StreamWriter writer = new StreamWriter(filePath, append: true))
            {
                writer.WriteLine($"-- Room Allocations on {DateTime.Now} --");
                foreach (var allocation in listOfRoomAllocations)
                {
                    writer.WriteLine($"Room {allocation.AllocatedRoomNo}: {allocation.AllocatedCustomer.CustomerName}");
                }
                writer.WriteLine("-------------------------------------------------");
            }
            Console.WriteLine("Room allocations saved to file.");
        }

        // Function to Show Room Allocations from a File
        static void ShowRoomAllocationsFromFile()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                Console.WriteLine("\nRoom Allocations From File:");
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine("No allocation data found in the file.");
            }
        }
    }
}
