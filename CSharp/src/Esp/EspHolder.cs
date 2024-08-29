using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CSharp.src.Generics;
using Microsoft.VisualBasic;

namespace Server
{
    internal class EspHolder
    {
        readonly ILoggingHelper helper = new() {LoggingDepth = 99};
        public Dictionary<string, Anchor>? Anchors { get; set; }
        public EspHolder(string JsonSource, out Response _res) { 
            try
            {
                var Source = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, double>>>>(JsonSource);
                Anchors = new EspHolder(Source, out Response res).Anchors;
                _res = res;
            }
            catch (JsonException)
            {
                _res = Response.Fail;
            }
        }
        Response res = Response.Begin;
        Response _res { get {
            helper.Log($"Got response {res.ToString()}", this);
            return res;
        } set {res = value;}}
        public EspHolder(Dictionary<string, Dictionary<string, Dictionary<string, double>>>? Source, out Response res)
        {
            Anchors = [];
            if (Source == null)
            {
                _res = Response.Fail;
                res = _res;
                return;
            }
            else if (Source.Count == 0)
            {
                _res = Response.Empty;
                res = _res;
                return;
            }
            else
            {
                foreach (var item in Source)
                {
                    Anchors.Add(item.Key, new Anchor(item.Value, out Response _res));
                    if (_res == Response.Fail)
                    {
                        _res = Response.Partial;
                        res = _res;
                        return;
                    }
                }
                _res = Response.Success;
                res = _res;
            }
        }
        public override string ToString()
        {
            return base.ToString() == null? "Literally Nothing.": JsonSerializer.Serialize(this);
        }
    }
    class VehicleSum{
        readonly Dictionary<string, double> X =[];
        readonly Dictionary <string, double> Y =[];
        readonly Dictionary<string, List<string>> Anchors = [];
        public VehicleSum(EspHolder holder){
            foreach (var (anchor, vehicle) in from anchor in holder.Anchors
                                              from vehicle in anchor.Value.Vehicles
                                              select (anchor, vehicle))
            {
                if (X.ContainsKey(vehicle.Key))
                {
                    X[vehicle.Key] += vehicle.Value.X;
                    Y[vehicle.Key] += vehicle.Value.Y;
                    Anchors[vehicle.Key].Add(anchor.Key);
                }
                else
                {
                    X.Add(vehicle.Key, vehicle.Value.X);
                    Y.Add(vehicle.Key, vehicle.Value.Y);
                    Anchors.Add(vehicle.Key, [anchor.Key]);
                }
            }
        }
        public List<(string Vehicle, double X, double Y, string[] Anchors)> GetSums(){
            List<(string, double, double, string[])> result = [];
            result.AddRange(from Vehicle in Anchors
                            select (Vehicle.Key, X[Vehicle.Key] / Vehicle.Value.Count, Y[Vehicle.Key] / Vehicle.Value.Count, Vehicle.Value.ToArray()));
            return result;
        }
    }
    internal class Anchor
    {
        public Dictionary<string, Vehicle> Vehicles { get; set; }
        public Anchor(Dictionary<string, Dictionary<string, double>> Source, out Response res)
        {
            Vehicles = [];
            foreach (var item in Source)
            {
                Vehicles.Add(item.Key, new Vehicle(item.Value, out Response _res));
                if (_res == Response.Fail)
                {
                    res = Response.Partial;
                    return;
                }
            }
            res = Response.Success;
        }
    }
    internal class Vehicle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Vehicle(Dictionary<string, double> Source, out Response _res)
        {
            try
            {
                X = Source["X"];
                Y = Source["Y"];
            }
            catch (KeyNotFoundException)
            {
                _res = Response.Fail;
                return;
            }
            _res = Response.Success;
        }
    }
}
