using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplianceMaintenanceSystem
{
    // ==========================================
    // 1. Abstraction - Interface
    // ==========================================
    public interface IAppliance
    {
        void DisplayDetails();
        void UpdateStatus(string newStatus);
    }

    // ==========================================
    // 1. Abstraction - Abstract Class
    // ==========================================
    public abstract class Appliance : IAppliance
    {
        // 3. Encapsulation - Private Fields
        private string _serialNumber;
        private string _brand;
        private string _status;
        private double _baseMaintenanceCost;

        // Static field to count all registered appliances globally
        private static int _totalAppliancesCount = 0;

        // 4. Delegates & Events
        public delegate void StatusChangedHandler(string message);
        public event StatusChangedHandler OnStatusChanged;

        // 3. Encapsulation - Public Properties (Read-Only fields using expression bodies)
        public string SerialNumber
        { get { return _serialNumber; } }

        public string Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public double BaseMaintenanceCost
        {
            get { return _baseMaintenanceCost; }
            set { _baseMaintenanceCost = value; }
        }

        // Static property to access total global registration count
        public static int TotalAppliancesCount => _totalAppliancesCount;

        public Appliance(string serialNumber, string brand, double baseCost)
        {
            _serialNumber = serialNumber;
            _brand = brand;
            _status = "In Progress";
            _baseMaintenanceCost = baseCost;

            // Increment static global appliance counter upon object construction
            _totalAppliancesCount++;
        }

        public void UpdateStatus(string newStatus)
        {
            _status = newStatus;

            // Trigger event when maintenance is successfully finished
            if (OnStatusChanged != null && _status == "Completed")
            {
                OnStatusChanged($"[ALERT] Appliance with Serial No: {SerialNumber} has been successfully repaired!");
            }
        }

        public virtual void DisplayDetails()
        {
            Console.WriteLine($"Serial: {SerialNumber} | Brand: {Brand} | Status: {Status}");
        }

        public abstract double CalculateRepairCost();
    }

    // ==========================================
    // 2. Inheritance & Polymorphism - Sub-classes
    // ==========================================

    // Sub-class 1: Refrigerator
    public class Refrigerator : Appliance
    {
        private bool _needsGasRefill;

        public Refrigerator(string serialNumber, string brand, double baseCost, bool needsGas)
            : base(serialNumber, brand, baseCost)
        {
            _needsGasRefill = needsGas;
        }

        public override double CalculateRepairCost()
        {
            return _needsGasRefill ? BaseMaintenanceCost + 1500 : BaseMaintenanceCost;
        }

        public override void DisplayDetails()
        {
            Console.WriteLine("| {0,-10} | {1,-10} | {2,-12} | {3,-15} | {4,-12} |", SerialNumber, Brand, Status, "Refrigerator", CalculateRepairCost());
        }
    }

    // Sub-class 2: Washing Machine
    public class WashingMachine : Appliance
    {
        private bool _needsMotorRepair;

        public WashingMachine(string serialNumber, string brand, double baseCost, bool needsMotor)
            : base(serialNumber, brand, baseCost)
        {
            _needsMotorRepair = needsMotor;
        }

        public override double CalculateRepairCost()
        {
            return _needsMotorRepair ? BaseMaintenanceCost + 2500 : BaseMaintenanceCost;
        }

        public override void DisplayDetails()
        {
            Console.WriteLine("| {0,-10} | {1,-10} | {2,-12} | {3,-15} | {4,-12} |", SerialNumber, Brand, Status, "Washing Machine", CalculateRepairCost());
        }
    }

    // Sub-class 3: Air Conditioner
    public class AirConditioner : Appliance
    {
        private bool _needsFilterClean;

        public AirConditioner(string serialNumber, string brand, double baseCost, bool needsFilter)
            : base(serialNumber, brand, baseCost)
        {
            _needsFilterClean = needsFilter;
        }

        public override double CalculateRepairCost()
        {
            return _needsFilterClean ? BaseMaintenanceCost + 500 : BaseMaintenanceCost;
        }

        public override void DisplayDetails()
        {
            Console.WriteLine("| {0,-10} | {1,-10} | {2,-12} | {3,-15} | {4,-12} |", SerialNumber, Brand, Status, "Air Conditioner", CalculateRepairCost());
        }
    }

    // ==========================================
    // 5. Static Classes & Members
    // ==========================================
    public static class MaintenanceValidator
    {
        public static bool ValidateSerialNumber(string serialNumber)
        {
            return !string.IsNullOrEmpty(serialNumber) && serialNumber.Length >= 5;
        }
    }

    // ==========================================
    // 2. Main Class to manage collection and polymorphism
    // ==========================================
    public class MaintenanceManager
    {
        private Appliance[] _applianceArray = new Appliance[100];

        private int _currentCount = 0;

        public Appliance[] ApplianceArray
        {
            get
            { return _applianceArray; }
        }

        public int CurrentCount
        {
            get { return _currentCount; }
        }

        public void AddAppliance(Appliance appliance)
        {
            if (!MaintenanceValidator.ValidateSerialNumber(appliance.SerialNumber))
            {
                Console.WriteLine($"ERROR: Serial number {appliance.SerialNumber} is invalid! (Must be 5+ characters).");
                return;
            }

            if (_currentCount < _applianceArray.Length)
            {
                _applianceArray[_currentCount] = appliance;
                _currentCount++; 
                Console.WriteLine($"SUCCESS: Appliance {appliance.SerialNumber} added to the system.");
            }
            else
            {
                Console.WriteLine("ERROR: System storage is full! Cannot add more appliances.");
            }
        }

        public double CalculateTotalRepairsCost()
        {
            double total = 0;
            for (int i = 0; i < _currentCount; i++)
            {
                total += _applianceArray[i].CalculateRepairCost();
            }
            return total;
        }

        public int GetCountByStatus(string status)
        {
            int count = 0;
            for (int i = 0; i < _currentCount; i++)
            {
                if (_applianceArray[i].Status == status)
                {
                    count++;
                }
            }
            return count;
        }

        public void DisplayAllAppliances()
        {
            Console.Clear();
            Console.WriteLine("\t\tScreen Display All Appliances & Analytics");
            Console.WriteLine("\n=======================================================================================");
            Console.WriteLine("| {0,-10} | {1,-10} | {2,-12} | {3,-15} | {4,-12} |", "Serial", "Brand", "Status", "Type", "Total Cost");
            Console.WriteLine("=======================================================================================");

            for (int i = 0; i < _currentCount; i++)
            {
                _applianceArray[i].DisplayDetails();
            }
            Console.WriteLine("=======================================================================================");
        }
    }

    // ==========================================
    // Execution Program Class
    // ==========================================
    class Program
    {
        static void ShowAlertMessage(string message)
        {
            Console.WriteLine("\n" + message);
        }

        static void Main(string[] args)
        {
            MaintenanceManager manager = new MaintenanceManager();
            bool running = true;

            while (running)
            {
                PrintMainScreen();

                string choice = Console.ReadLine();
                Console.WriteLine("-------------------------------------------------");

                switch(choice)
                {
                    case "1":
                        AddRefrigeratorProcess(manager);
                        PauseAndClearScreen();
                        break;
                    case "2":
                        AddWashingMachineProcess(manager);
                        PauseAndClearScreen();
                        break;
                    case "3":
                        AddAirConditionerProcess(manager);
                        PauseAndClearScreen();
                        break;
                    case "4":
                        UpdateApplianceStatusProcess(manager);
                        PauseAndClearScreen();
                        break;

                    case "5":
                        manager.DisplayAllAppliances();
                        Console.WriteLine("\n------------------------------------------------");
                        Console.WriteLine("--- System Analytics Info ---");
                        Console.WriteLine("------------------------------------------------");
                        Console.WriteLine($"\tTotal Registered Appliances (Static): {Appliance.TotalAppliancesCount}");
                        Console.WriteLine($"\tTotal Expected Repair Cost: {manager.CalculateTotalRepairsCost()} SYP");
                        Console.WriteLine($"\tAppliances In Progress: {manager.GetCountByStatus("In Progress")}");
                        Console.WriteLine($"\tAppliances Completed: {manager.GetCountByStatus("Completed")}");
                        Console.WriteLine("------------------------------------------------");
                        PauseAndClearScreen();
                        break;
                    case "6":
                        running = false;
                        Console.Clear();
                        Console.WriteLine("\n\t\t=================== End Screen ===================");
                        Console.WriteLine("\t\t\tExiting program... Goodbye!");
                        Console.WriteLine("\t\t==================================================");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please enter a number between 1 and 6.");
                        PauseAndClearScreen();
                        break;
                }
            }
        }
        private static void PrintMainScreen()
        {
            Console.WriteLine("\t\t=== WELCOME TO APPLIANCE MAINTENANCE SYSTEM ===");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Select an option:");
            Console.WriteLine("\t1. Add a Refrigerator");
            Console.WriteLine("\t2. Add a Washing Machine");
            Console.WriteLine("\t3. Add an Air Conditioner");
            Console.WriteLine("\t4. Update Appliance Status [Complete Repair -> Triggers Event]");
            Console.WriteLine("\t5. Display All Appliances & Analytics");
            Console.WriteLine("\t6. Exit");
            Console.WriteLine("-------------------------------------------------");
            Console.Write("Enter your choice [1 - 6]: ");
        }
        private static void PauseAndClearScreen()
        {
            Console.Write("\n\tPress Any Key To Continue.....");
            // Console.ReadKey();
            Console.ReadLine();
            Console.Clear();
        }
        private static void AddRefrigeratorProcess(MaintenanceManager manager)
        {
            Console.WriteLine("\n--- Adding Refrigerator ---");
            Console.Write("Enter Serial Number (Min 5 chars): ");
            string serial = Console.ReadLine();
            Console.Write("Enter Brand Name: ");
            string brand = Console.ReadLine();
            Console.Write("Enter Base Maintenance Cost: ");
            double cost = Convert.ToDouble(Console.ReadLine());
            Console.Write("Does it need Gas Refill? [y -> yes] or [n -> no]: ");
            bool gas = Console.ReadLine().ToLower() == "y";
            Refrigerator refri = new Refrigerator(serial, brand, cost, gas);
            refri.OnStatusChanged += ShowAlertMessage;
            manager.AddAppliance(refri);
        }
        private static void AddWashingMachineProcess(MaintenanceManager manager)
        {
            Console.WriteLine("\n--- Adding Washing Machine ---");
            Console.Write("Enter Serial Number (Min 5 chars): ");
            string serial = Console.ReadLine();
            Console.Write("Enter Brand Name: ");
            string brand = Console.ReadLine();
            Console.Write("Enter Base Maintenance Cost: ");
            double cost = Convert.ToDouble(Console.ReadLine());
            Console.Write("Does it need Motor Repair? [y -> yes] or [n -> no]: ");
            bool motor = Console.ReadLine().ToLower() == "y";
            WashingMachine wash = new WashingMachine(serial, brand, cost, motor);
            wash.OnStatusChanged += ShowAlertMessage;
            manager.AddAppliance(wash);
        }
        private static void AddAirConditionerProcess(MaintenanceManager manager)
        {
            Console.WriteLine("\n--- Adding Air Conditioner ---");
            Console.Write("Enter Serial Number (Min 5 chars): ");
            string serial = Console.ReadLine();
            Console.Write("Enter Brand Name: ");
            string brand = Console.ReadLine();
            Console.Write("Enter Base Maintenance Cost: ");
            double cost = Convert.ToDouble(Console.ReadLine());
            Console.Write("Does it need Filter Cleaning? [y -> yes] or [n -> no]: ");
            bool filter = Console.ReadLine().ToLower() == "y";
            AirConditioner air = new AirConditioner(serial, brand, cost, filter);
            air.OnStatusChanged += ShowAlertMessage;
            manager.AddAppliance(air);
        }
        // ========================================================
        // Method to search for an appliance by serial number and update its status
        // ========================================================
        private static void UpdateApplianceStatusProcess(MaintenanceManager manager)
        {
            Console.WriteLine("\n--- Update Appliance Status ---");
            Console.Write("Enter Appliance Serial Number to complete repair: ");
            string serial = Console.ReadLine();
            Appliance foundAppliance = null;

            for (int i = 0; i < manager.CurrentCount; i++)
            {
                if (manager.ApplianceArray[i].SerialNumber == serial)
                {
                    foundAppliance = manager.ApplianceArray[i];
                    break;
                }
            }
            if (foundAppliance != null)
            {
                foundAppliance.UpdateStatus("Completed");
            }
            else
            {
                Console.WriteLine("ERROR: Appliance not found!");
            }
        }
    }
}