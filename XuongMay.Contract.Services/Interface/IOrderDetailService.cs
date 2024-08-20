﻿using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.OrderDetailModelView;

namespace XuongMay.Contract.Services.Interface
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetPaginatedOrderDetailsAsync(int pageNumber, int pageSize);
        Task<OrderDetail> GetOrderDetailByIdAsync(string id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(string orderId);
        Task<OrderDetail> CreateOrderDetailAsync(CreateOrderDetailModelView orderDetailModel);
        Task<OrderDetail> UpdateOrderDetailAsync(string id, UpdateOrderDetailModelView orderDetailModel);
        Task<bool> DeleteOrderDetailAsync(string id);
        Task<OrderDetail> CancelOrderDetailAsync(string id);
        Task<OrderDetail> MoveToNextStatusAsync(string id);
    }
}
