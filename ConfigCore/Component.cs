using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigCore
{
    public abstract class Component : IComponent
    {
        public string Name {  get; private set; }
        public string Manufacture { get; private set; }
        public int Price { get; private set; }
        public int PowerConsumption { get; private set; }

        public Component(string Name, string Manufacture, int Price, int PowerConsumption) {
            this.Name = Name;
            this.Manufacture = Manufacture;
            this.Price = Price;
            this.PowerConsumption = PowerConsumption;
        }
        public abstract string GetCategory();
        public abstract string Beaty();
        public abstract double GetPerfomanceScore();
        public override string ToString()
        {
            return $"{Manufacture}{Name}";
        }

    }
}
