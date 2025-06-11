using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigCore
{
    public interface IComponent
    {
        string Name { get; }
        string Manufacture { get; }
        int Price { get; }
        int PowerConsumption { get; }

        string GetCategory();
        double GetPerfomanceScore();
    }
}
