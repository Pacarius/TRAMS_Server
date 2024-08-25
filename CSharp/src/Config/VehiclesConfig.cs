using System.Text.Json.Serialization;

namespace Config
{
    public class VehiclesConfig(List<Vehicle>? vehicles)
    {
        [JsonRequired]
        public List<Vehicle> Vehicles {get; set;} = vehicles ?? ([
            new Vehicle(){PlateNo = "00000000", OwnerName = "you", OwnerID = "you", Type = VehicleType.Taxi},
            new Vehicle(){PlateNo = "11111111", OwnerName = "me", OwnerID = "me", Type = VehicleType.PublicBus}
        ]);
    }
    public class Vehicle(string plateNo = "NULL", string ownerName = "NULL", string ownerID = "NULL", VehicleType type = VehicleType.NULL)
    {
        [JsonRequired]
        public string PlateNo { get; set; } = plateNo;
        [JsonRequired]
        public string OwnerName { get; set; } = ownerName;
        [JsonRequired]
        public string OwnerID { get; set; } = ownerID;
        [JsonRequired]
        public VehicleType Type { get; set; } = type;
        public string ReturnType (){
            return Type switch
            {
                VehicleType.PrivateCar => "Private Car",
                VehicleType.LightGoodsVehicle => "Light Goods Vehicle",
                VehicleType.Motorcycle => "Motorcycle",
                VehicleType.PrivateLightBus => "Private Light Bus",
                VehicleType.PublicLightBus => "Public Light Bus",
                VehicleType.Taxi => "Taxi",
                VehicleType.PrivateBus => "Private Bus",
                VehicleType.PublicBus => "Public Bus",
                VehicleType.InvalidCarriage => "Invalid Carriage",
                VehicleType.GovernmentVehicle => "Government Vehicle",
                VehicleType.PublicBusFranchised => "Public Bus (Franchised)",
                VehicleType.MediumGoodsVehicle => "Medium Goods Vehicle",
                VehicleType.HeavyGoodsVehicle => "Heavy Goods Vehicle",
                VehicleType.ArticulatedVehicle => "Articulated Vehicle",
                VehicleType.SpecialPurposeVehicle => "Special Purpose Vehicle",
                VehicleType.MotorTricycle => "Motor Tricycle",
                _ => "NULL",
            };
        }
    }
    public enum VehicleType
    {
        NULL,
        PrivateCar,
        LightGoodsVehicle,
        Motorcycle,
        PrivateLightBus,
        PublicLightBus,
        Taxi,
        PrivateBus,
        PublicBus,
        InvalidCarriage,
        GovernmentVehicle,
        PublicBusFranchised,
        MediumGoodsVehicle,
        HeavyGoodsVehicle,
        ArticulatedVehicle,
        SpecialPurposeVehicle,
        MotorTricycle
    }
}