using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace FastFoodWeb.Hubs
{
    public class OrderHub : Hub
    {
        // Nhân viên/Admin gọi method này để nhận thông báo đơn mới
        public async Task JoinStaffGroup()
            => await Groups.AddToGroupAsync(Context.ConnectionId, "Staff");

        // Khách hàng gọi method này để nhận cập nhật trạng thái đơn của mình
        public async Task JoinOrderRoom(string userId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
    }
}
