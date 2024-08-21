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