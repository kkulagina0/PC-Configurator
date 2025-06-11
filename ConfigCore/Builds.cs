using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConfigCore
{
    public class Builds
    {
        private List<Build> builds;
        public Builds()
        {
            builds = new List<Build>();
        }
        public int Count() 
        { 
            return builds.Count; 
        }
        public void AddBuild(Build build)
        {
            builds.Add(build);
        }
        public void RemoveBuild(Build build)
        {
            builds.Remove(build);
        }
        public List<Build> All()
        {
            return builds;
        }
        public List<Build> FindName(string name)
        {
            return builds.Where(x => x.Name.Equals(name)).ToList();
        }
        public Build this[int i]
        {
            get { return builds[i]; }
            set { builds[i] = value; }
        }

        public void SaveToFile(string filePath)
        {
            var writer = new StreamWriter(filePath);
            writer.WriteLine("Name,Cpu,Gpu,Ram,Motherboard,Psu,Storage");
            for (int i = 0; i < builds.Count; i++)
            {
                writer.WriteLine(string.Join(",", builds[i].Name,
                    builds[i].cpu.Name,
                    builds[i].gpu.Name,
                    builds[i].motherboard.Name,
                    builds[i].ram.Name,
                    builds[i].powerSupply.Name));
            }
            writer.Close();
        }
        public void LoadFromFile(string filePath, List<CPU> allCpu, List<GPU> allGpu, List<RAM> allRam, List<Motherboard> allMotherboar, List<PowerSupply> allPowerSupply)
        {
            if (!File.Exists(filePath)) return;
            var lines = File.ReadAllLines(filePath).Skip(1);
            foreach (var line in lines)
            {
                var pattern = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
                string[] parts = Regex.Split(line, pattern).Select(s => s.Trim('\"')).ToArray();
                if (parts.Length != 6) continue;
                try
                {
                    Build b = new Build(parts[0], allCpu.First(c => c.Name == parts[1]),
                    allGpu.First(c => c.Name == parts[2]),
                    allMotherboar.First(c => c.Name.Equals(parts[3])),
                    allRam.First(c => c.Name == parts[4]),
                    allPowerSupply.First(c => c.Name == parts[5]));
                    AddBuild(b);
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
            }
        }
    }
}
