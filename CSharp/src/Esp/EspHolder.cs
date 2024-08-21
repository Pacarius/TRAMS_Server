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
        public Dictionary<string, Anchor> Anchors { get; set; }
        public EspHolder(string JsonSource, out Response _res) : this(JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, double>>>>(JsonSource), out _res) { }
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
    class Anchor
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
    class Vehicle
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
