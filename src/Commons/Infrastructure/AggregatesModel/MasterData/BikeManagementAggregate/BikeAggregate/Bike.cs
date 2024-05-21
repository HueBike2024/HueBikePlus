using Core.Models.Base;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeStationAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.MapLocationAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using Infrastructure.AggregatesModel.MasterData.TripManagementAggregate.TicketAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate
{
    public class Bike : BaseEntity
    {
        
        public string BikeName { get; set; }
        public string? PathQr { get; set; }
        public string? QrCodeImage { get; set; }
        public int? Power { get; set; }
        public int RentalQuantity { get; set; }
        public long StatusId { get; set; }
        public long StationId { get; set; }
        public bool IsActive { get; set; }

        public virtual Status Status { get; set; }
        public virtual Station Station { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

        public Bike()
        {

        }

        public Bike(string bikeName, long statusId, long stationId)
        {
            BikeName = bikeName;
            StatusId = statusId;
            StationId = stationId;
            RentalQuantity = 0;
            IsActive = false;
        }

        //Thêm khóa mới cho xe
        public void AddLock(string pathQR, string qrCodeImg, int power)
        {
            PathQr = pathQR;
            QrCodeImage = qrCodeImg;
            Power = power;
        }
        // set trang thái của khóa
        public static void EnableBike(ref Bike bike)
        {
            bike.IsActive = true;
        }
        // xóa xe
        public static void DeleteBike(ref Bike bike)
        {
            bike.IsDeleted = true;
        }

    }
}
