using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigCore;

//DataSet thisDataSet = new DataSet();
//SqlDataAdapter custAdapter = new SqlDataAdapter(
//     "SELECT * FROM CPU", thisConnection);
//custAdapter.Fill(thisDataSet, "CPU");
//foreach (DataRow row in thisDataSet.Tables["CPU"].Rows)
//{
//    list.Add(new CPU(row["Name"].ToString(), row["Manufacture"].ToString(), (int)row["Price"],
//        row["PowerConsumptio"], row["Cores"],row["Threads"],row["Socket"],row["GHz"]);
//}

namespace ConfigData
{
    public static class DataLoader
    {
        static string dataFile = Path.GetFullPath(@"..\..\..\ConfigData\Database1.mdf");
        static string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    $@"AttachDbFilename={dataFile};" +
                    @"Integrated Security=True;Connect Timeout=30";
        public static List<CPU> LoadCpu()
        {
            List<CPU> list = new List<CPU>();
            SqlConnection thisConnection = new SqlConnection(ConnectionString);
            thisConnection.Open();
            SqlCommand command = thisConnection.CreateCommand();
            command.CommandText = "SELECT * FROM CPU";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CPU(reader["Name"].ToString(),
                                  reader["Manufacture"].ToString(),
                                  (int)reader["Price"],
                                  (int)reader["PowerConsumption"],
                                  (int)reader["Cores"],
                                  (int)reader["Thread"],
                                  reader["Socket"].ToString(),
                                  (double)reader["GHz"]));
            }
            return list;
        }
        public static List<GPU> LoadGpu()
        {
            List<GPU> list = new List<GPU>();
            SqlConnection thisConnection = new SqlConnection(ConnectionString);
            thisConnection.Open();
            SqlCommand command = thisConnection.CreateCommand();
            command.CommandText = "SELECT * FROM GPU";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new GPU(reader["Name"].ToString(),
                                  reader["Manufacture"].ToString(),
                                  (int)reader["Price"],
                                  (int)reader["PowerConsumptio"],(int)reader["Memory"]));
            }
            return list;
        }
        public static List<RAM> LoadRAM()
        {
            List<RAM> list = new List<RAM>();
            SqlConnection thisConnection = new SqlConnection(ConnectionString);
            thisConnection.Open();
            SqlCommand command = thisConnection.CreateCommand();
            command.CommandText = "SELECT * FROM RAM";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new RAM(reader["Name"].ToString(),
                                  reader["Manufacture"].ToString(),
                                  (int)reader["Price"],
                                  (int)reader["PowerConsumptio"],
                                  (int)reader["Size"],
                                  (int)reader["MHz"],
                                  (int)reader["Type"]));
            }
            return list;
        }
        public static List<Motherboard> LoadMotherboardu()
        {
            List<Motherboard> list = new List<Motherboard>();
            SqlConnection thisConnection = new SqlConnection(ConnectionString);
            thisConnection.Open();
            SqlCommand command = thisConnection.CreateCommand();
            command.CommandText = "SELECT * FROM Motherboard";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Motherboard(reader["Name"].ToString(),
                                  reader["Manufacture"].ToString(),
                                  (int)reader["Price"],
                                  (int)reader["PowerConsumption"],
                                  reader["Socket"].ToString(),
                                  (int)reader["RamType"]));
            }
            return list;
        }
        public static List<PowerSupply> LoadPowerSupply()
        {
            List<PowerSupply> list = new List<PowerSupply>();
            SqlConnection thisConnection = new SqlConnection(ConnectionString);
            thisConnection.Open();
            SqlCommand command = thisConnection.CreateCommand();
            command.CommandText = "SELECT * FROM PowerSupply";
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PowerSupply(reader["Name"].ToString(),
                                  reader["Manufacture"].ToString(),
                                  (int)reader["Price"],
                                  (int)reader["PowerConsumption"],
                                  (int)reader["W"],
                                  reader["Rating"].ToString()));
            }
            return list;
        }
    }

 }
