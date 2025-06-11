using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConfigCore
{
    public class CPU : Component
    {

        public int Cores { get; private set; }
        public int Threads { get; private set; }
        public string Socket { get; private set; }
        public double GHz { get; private set; }

        public CPU(string name, string manufacture, int price, int powerConsumption, int cores, int threads, string socket, double gHz) :base(name, manufacture, price, powerConsumption)
        {
            Cores = cores;
            Threads = threads;
            Socket = socket;
            GHz = gHz;
        }

        public override string GetCategory() => "CPU";
        public override double GetPerfomanceScore()
        {
            return Cores * 10 + Threads * 5 + GHz * 20;
        }
        public override string Beaty()
        {
            return $"\nCPU: {Name} by {Manufacture}, {Cores}C/{Threads}T, {GHz} GHz, Socket: {Socket}, " +
                   $"Power: {PowerConsumption}W, Price: ${Price}";
        }

    }
    public class GPU : Component
    {
        public int Memory { get; private set; }
        public GPU(string name, string manufacture, int price, int powerConsumption, int memory) : base(name, manufacture, price, powerConsumption)
        {
            Memory = memory;
        }
        public override string GetCategory() => "GPU";
        public override double GetPerfomanceScore()
        {
            return Memory * 20 + PowerConsumption * 0.5;
        }
        public override string Beaty()
        {
            return $"\nGPU: {Name} by {Manufacture}, {Memory}GB, Power: {PowerConsumption}W, Price: ${Price}";
        }

    }
    public class RAM : Component
    {
        public int Size { get; private set; }
        public int MHz { get; private set; }
        public int Type { get; private set; }
        public RAM(string name, string manufacture, int price, int powerConsumption, int size, int mhz, int type) : base(name, manufacture, price, powerConsumption)
        {
            Size = size;
            MHz = mhz;
            Type = type;
        }

        public override string GetCategory() => "RAM";
        public override double GetPerfomanceScore()
        {
            return Size * (MHz / 50) + Type * 5;
        }
        public override string Beaty()
        {
            return $"\nRAM: {Name} by {Manufacture}, {Size}GB RAMType DDR:{Type} @ {MHz}MHz, " +
                   $"Power: {PowerConsumption}W, Price: ${Price}";
        }


    }
    public class Motherboard : Component
    {
        public string Socket { get; private set; }
        public int RamType { get; private set; }
        public Motherboard(string name, string manufacture, int price, int powerConsumption, string socket, int ramtype) : base(name, manufacture, price, powerConsumption)
        {
            Socket = socket;
            RamType = ramtype;
        }

        public override string GetCategory() => "Motherboard";
        public override double GetPerfomanceScore()
        {
            return Price / 10;
        }
        public override string Beaty()
        {
            return $"\nMotherboard: {Name} by {Manufacture}, Socket: {Socket}, RAMType DDR:{RamType}, " +
                   $"Power: {PowerConsumption}W, Price: ${Price}";
        }

    }
    public class PowerSupply : Component
    {
        public int W { get; private set; }
        public string Rating { get; private set; }
        public PowerSupply(string name, string manufacture, int price, int powerConsumption, int w, string rating) : base(name, manufacture, price, powerConsumption)
        {
            W = w;
            Rating = rating;
        }
        public override string GetCategory() => "PowerSupply";
        public override double GetPerfomanceScore()
        {
            return W * 0.5;
        }
        public override string Beaty()
        {
            return $"\nPower Supply: {Name} by {Manufacture}, {W}W, {Rating}, " +
                   $"Power Consumption: {PowerConsumption}W, Price: ${Price}";
        }

    }
}

