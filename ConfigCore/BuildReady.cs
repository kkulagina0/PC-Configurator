using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigCore
{
    public class BuildReady
    {
        private List<CPU> allCpu;
        private List<GPU> allGpu;
        private List<RAM> allRam;
        private List<Motherboard> allMotherboar;
        private List<PowerSupply> allPowerSupply;

        public BuildReady(List<CPU> cpus, List<GPU> gpus, List<RAM> rams,List<Motherboard> mbs, List<PowerSupply> psys)
        {
            allCpu = cpus;
            allGpu = gpus;
            allRam = rams;
            allMotherboar = mbs;
            allPowerSupply = psys;
        }
        public Build CreateByBudget(int budget)
        {
            CPU minCpu = allCpu.OrderBy(x => x.Price).FirstOrDefault();
            RAM minRam = allRam.OrderBy(x => x.Price).FirstOrDefault();
            Motherboard minMb = allMotherboar.OrderBy(x => x.Price).FirstOrDefault();
            int remainingBudget = budget - (minCpu.Price + minRam.Price + minMb.Price);
            GPU gpu = allGpu.Where(x => x.Price <= remainingBudget * 0.65).OrderByDescending(x => x.GetPerfomanceScore()).FirstOrDefault();
            if (gpu != null) { remainingBudget -= gpu.Price; }
            else { return null; }
            Build build = new Build("Temp", minCpu, gpu, minMb, minRam, null);
            int requiredPower = build.TotalPowerConsumption();

            PowerSupply psu = allPowerSupply.Where(x => x.W >= requiredPower)
                                   .OrderBy(x => x.Price)
                                   .FirstOrDefault();
            if (psu != null)
            {
                if (psu.Price > remainingBudget)
                    return null;
                remainingBudget -= psu.Price;
            }
            else { return null; }
            CPU cpu = minCpu;
            if (remainingBudget > 0)
            {
                CPU betterCpu = allCpu.Where(x => x.Price <= remainingBudget *0.5 + minCpu.Price)
                             .OrderByDescending(x => x.GetPerfomanceScore())
                             .FirstOrDefault();
                if (betterCpu != null)
                {
                    remainingBudget -= (betterCpu.Price - minCpu.Price);
                    cpu = betterCpu;
                }
            }
            RAM ram = minRam;
            if (remainingBudget > 0)
            {
                RAM betterRam = allRam.Where(x => x.Price <= remainingBudget * 0.5 + minRam.Price)
                                     .OrderByDescending(x => x.GetPerfomanceScore())
                                     .FirstOrDefault();
                if (betterRam != null)
                {
                    remainingBudget -= (betterRam.Price - minRam.Price);
                    ram = betterRam;
                }
            }
            Motherboard mb = minMb;
            if (remainingBudget > 0)
            {
                Motherboard betterMb = allMotherboar.Where(x => x.Price <= remainingBudget + minMb.Price)
                                     .OrderByDescending(x => x.GetPerfomanceScore())
                                     .FirstOrDefault();
                if (betterMb != null)
                {
                    remainingBudget -= (betterMb.Price - minMb.Price);
                    mb = betterMb;
                }
            }
            Build buildFinal = new Build("Идеальная сборка ", cpu, gpu, mb, ram, psu);
            if (buildFinal.Check() == "" && buildFinal.TotalPrice() <= budget)
                return buildFinal;
            budget = (int)(budget * 0.9);
            return CreateByBudget(budget);
        }
        public Build CreateBestBuild() { 

            var build = new Build();

            build.cpu = Analizer.GetTopPerformer(allCpu);
            build.gpu = Analizer.GetTopPerformer(allGpu);
            build.ram = allRam.OrderByDescending(r => r.Size).FirstOrDefault();
            build.motherboard = allMotherboar.FirstOrDefault(mb => mb.Socket == build.cpu?.Socket && mb.RamType == build.ram?.Type);
            build.powerSupply = allPowerSupply.FirstOrDefault(p => p.W >= build.TotalPowerConsumption());
            build.Name = $"Геймерская сборка: {build.cpu} и {build.gpu}";

            return build;
        }

    }
}
