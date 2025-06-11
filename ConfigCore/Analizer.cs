using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigCore
{
    public static class Analizer
    {
        public static double AverageComponentByPerfomance<T>(List<T> comp) where T: IComponent
        {
            return comp.Where(c => c != null).Sum(c => c.GetPerfomanceScore()) / comp.Where(c => c != null).Count();
        }
        public static double AverageComponentByPrice<T>(List<T> comp) where T : IComponent
        {
            return comp.Where(c => c != null).Sum(c => c.GetPerfomanceScore()) / comp.Where(c => c != null).Count();
        }
        public static string AnalyzeCurrentComponente<T>(Component comp, List<T> comps) where T : IComponent
        {
            double allPriceToPerfomance = AverageComponentByPrice(comps)/AverageComponentByPerfomance(comps);
            double cur = comp.Price/comp.GetPerfomanceScore();
            if (cur >= allPriceToPerfomance) { return $"{comp} хороший вариант за свою цену"; }
            return $"{comp} не очень хороший вариант за свою цену(дороговат или же просто плохой)";
        }

        public static T GetTopPerformer<T>(IEnumerable<T> components) where T : IComponent
        {
            return components.OrderByDescending(c => c.GetPerfomanceScore()).FirstOrDefault();
        }

    }
}
