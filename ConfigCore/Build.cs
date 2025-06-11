using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace ConfigCore
{
    public class Build
    {
        public string Name { get; set; } = "Собственная сборка";
        public CPU cpu { get; set; }
        public GPU gpu { get; set; }
        public Motherboard motherboard { get; set; }
        public RAM ram { get; set; }
        public PowerSupply powerSupply { get; set; }
        public Build(string name, CPU cpu, GPU gpu, Motherboard motherboard, RAM ram, PowerSupply powerSupply)
        {
            Name = cpu.Name+" "+gpu.Name;
            this.cpu = cpu;
            this.gpu = gpu;
            this.motherboard = motherboard;
            this.ram = ram;
            this.powerSupply = powerSupply;
        }
        public Build() { }

        public IEnumerable <IComponent> Components()
        {
            yield return cpu; yield return gpu; yield return motherboard; yield return ram; yield return powerSupply;//
        }

        public int TotalPowerConsumption()
        {
            return Components().Where(x => x != null).Sum(x => x.PowerConsumption);
        }
        public int TotalPrice()
        {
            return Components().Where(x => x != null).Sum(x => x.Price);
        }
        public double TotalPerfomance()
        {
            return Components().Where(x => x != null).Sum(x => x.GetPerfomanceScore());
        }
        public string Check()
        {
            string warnings= "";
            if (Components().Any(x => x == null)) {
                warnings += "Какой-то компонент не введен\n";
                return warnings;
            }
            if (TotalPowerConsumption() > powerSupply.W)
            {
                warnings += "БП слабый\n";
            }
            if(cpu.Socket != motherboard.Socket)
            {
                warnings += "Сокеты не совпадают\n";
            }
            if (ram.Type != motherboard.RamType)
            {
                warnings += "Тип ОЗУ не подходит\n";
            }
            return warnings;
        }
        public static string Compare(Build a, Build b)
        {
            string res = "";
            if (a == null || b == null)
                return res;
            if ((a.Components().Any(x => x == null)) || b.Components().Any(x => x == null))
                return "Какой-то компонент какой-то из сборок не введен";
            if (!(string.IsNullOrEmpty(a.Check())|| string.IsNullOrEmpty(b.Check())))
                return "Какая то сборка не правильная";
            res += $"Сравнение сборок {a.Name} и {b.Name}\n";
            res += $"\nЦена: {a.TotalPrice()} против {b.TotalPrice()}";
            res += $"\nПроизводительность {a.TotalPerfomance()} против {b.TotalPerfomance()}";
            return res;
        }
        public string Tips()
        {
            string tips = "";
            if (Components().Any(x => x == null))
                return("Какой-то компонент не введен\n - совет не дать" );
            if(ram.Type==3)
                tips += "Старая ОЗУ\n";
            if (powerSupply.Rating == "No")
                tips += "Качество блока питания не очень хорошее\n";
            return tips;
        }
        public override string ToString()
        {
            return $"{Name} -{TotalPrice()}$";
        }
        public string BeautyString()
        {
            var components = new Component[] {cpu, gpu, motherboard, powerSupply, ram };
            return string.Join(", ", components.Where(c => c != null).Select(c => c.Beaty()));
        }

    }
}
