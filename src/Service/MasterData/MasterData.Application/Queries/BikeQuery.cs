using Core.SeedWork;
using Core.SeedWork.Repository;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.BikeAggregate;
using Infrastructure.AggregatesModel.MasterData.BikeManagementAggregate.StationAggregate;
using Infrastructure.AggregatesModel.MasterData.StatusAggregate;
using MasterData.Application.Commands.BikeCommand;
using MasterData.Application.Commands.BikeLockCommand;
using MasterData.Application.Commands.UnitCommand;
using MasterData.Application.DTOs.Bike;
using MasterData.Application.DTOs.BikeLock;
using MasterData.Application.DTOs.Notification;
using MasterData.Application.DTOs.Unit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Queries
{
    public interface IBikeQuery
    {
        /// <summary>
        /// Chi tiết thông tin 1 xe
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<BikeDetailResponse> GetAsync(GetBikeCommand command);
        /// <summary>
        /// Danh sách xe
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagingResultSP<BikeResponse>> ListAsync(ListBikeCommand command);
    }
    public class BikeQuery : IBikeQuery
    {
        private readonly IRepository<Bike> _bikeRep;
        private readonly IRepository<Status> _statusRep;
        private readonly IRepository<Station> _stationRep;
        public BikeQuery(IRepository<Bike> bikeRep, IRepository<Status> statusRep, IRepository<Station> stationRep)
        {
            _bikeRep = bikeRep;
            _statusRep = statusRep;
            _stationRep = stationRep;
        }
        public async Task<BikeDetailResponse> GetAsync(GetBikeCommand request)
        {
            return await _bikeRep.GetQuery(e => e.Id == request.Id)
                   .Include(k => k.Status)   // Join với bảng Status
                   .Include(k => k.Station)
               .Select(k => new BikeDetailResponse
               {
                   Id = k.Id,
                   BikeName = k.BikeName,
                   StationId = k.StationId,
                   StationName = k.Station.StationName,
                   PathQr = k.PathQr,
                   QrCodeImage = k.QrCodeImage,
                   Power = k.Power,
                   RentalQuantity = k.RentalQuantity,
                   StatusId = k.StatusId,
                   StatusName = k.Status.StatusName,
                   ActiveStatus = k.IsActive ? "Đã kích hoạt" : "Chưa kích hoạt"
               }).FirstOrDefaultAsync();
        }

        // Danh sách xe

        public async Task<PagingResultSP<BikeResponse>> ListAsync(ListBikeCommand request)
        {
            var listBikeResponse = from Bike in _bikeRep.GetQuery()
                                             join Status in _statusRep.GetQuery() on Bike.StatusId equals Status.Id
                                             join Station in _stationRep.GetQuery() on Bike.StationId equals Station.Id
                                             select new BikeResponse
                                             {
                                                 Id = Bike.Id,
                                                 BikeName = Bike.BikeName,
                                                 Location = Station.LocationName,
                                                 StatusId= Bike.StatusId,
                                                 StationName = Station.StationName,
                                                 PathQr = Bike.PathQr,
                                                 QrCodeImage = Bike.QrCodeImage,
                                                 Power = Bike.Power,
                                                 RentalQuantity= Bike.RentalQuantity,
                                                 StationId = Bike.StationId,
                                                 StatusName= Status.StatusName,
                                                 ActiveStatus = Bike.IsActive ? "Đã kích hoạt" : "Chưa kích hoạt"
                                             };

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                request.SearchTerm = request.SearchTerm.ToLower().Trim();
                listBikeResponse = listBikeResponse.Where(e => e.BikeName.Trim().ToLower().Contains(request.SearchTerm) || e.PathQr.Trim().ToLower().Contains(request.SearchTerm));
            }

            
            if (request.StatusId != null)
            {
                var status = await _statusRep.FindOneAsync(e => e.Id == request.StatusId);
                listBikeResponse = listBikeResponse.Where(e => e.StatusName == status.StatusName);
            }

            
            if (request.StationId != null)
            {
                var station = await _stationRep.FindOneAsync(e => e.Id == request.StationId);
                listBikeResponse = listBikeResponse.Where(e => e.StationName == station.StationName);
            }

            if (string.IsNullOrEmpty(request.OrderBy) && string.IsNullOrEmpty(request.OrderByDesc))
            {
                listBikeResponse = listBikeResponse.OrderByDescending(e => e.RentalQuantity);
            }
            else
            {
                listBikeResponse = PagingSorting.Sorting(request, listBikeResponse);
            }
            var pageIndex = request.PageSize * (request.PageIndex - 1);

            var response = await PaginatedList<BikeResponse>.CreateAsync(listBikeResponse, pageIndex, request.PageSize);

            var result = new PagingResultSP<BikeResponse>(response, response.Total, request.PageIndex, request.PageSize);
            var i = pageIndex + 1;

            foreach (var item in result.Data)
            {
                item.Index = i++;
            }

            return result;

        }
    }
}
