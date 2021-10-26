﻿using API.Domains;
using API.DTOs.Notifications;
using API.Interfaces;
using API.Warppers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> SendNotificationToClients(NotificationToClientsRequest request)
        {
            if (request.UserIds.Any())
            {
                var fcms = await _unitOfWork.GetRepository<Fcm>().GetAsync(x => request.UserIds.Contains(x.UserId.ToString()));
                var clientTokens = fcms.Select(x => x.Token).ToList();
                if (clientTokens.Any())
                {
                    var response = await Helpers.Notification.SendNotifications(clientTokens, request.Title, request.Description);
                    foreach (var id in request.UserIds)
                    {
                        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(Guid.Parse(id));
                        if (user != null)
                        {
                            var notification = new Notification
                            {
                                DateCreate = DateTime.UtcNow,
                                Description = request.Description,
                                Id = Guid.NewGuid(),
                                Title = request.Title,
                                UserId = user.Id
                            };
                            await _unitOfWork.GetRepository<Notification>().AddAsync(notification);
                            await _unitOfWork.SaveAsync();
                        }
                    };
                    return new Response<string>(response, message: "Succeed");
                }
            }
            return new Response<string>(message: "Send Notifications Failed");
        }

        public async Task<Response<string>> SendNotificationToRole(NotificationToRoleRequest request)
        {
            var role = await _unitOfWork.GetRepository<Role>().GetByIdAsync(Guid.Parse(request.RoleId));
            if (role != null)
            {
                var users = await _unitOfWork.GetRepository<User>().GetAsync(x => x.RoleId.Equals(role.Id));
                if (users.Any())
                {
                    var userIdList = users.Select(x => x.Id);
                    var fcms = await _unitOfWork.GetRepository<Fcm>().GetAsync(x => userIdList.Contains(x.UserId));
                    var clientTokens = fcms.Select(x => x.Token).ToList();
                    if (clientTokens.Any())
                    {
                        var response = await Helpers.Notification.SendNotifications(clientTokens, request.Title, request.Description);
                        foreach (var user in users)
                        {
                            var notification = new Notification
                            {
                                DateCreate = DateTime.UtcNow,
                                Description = request.Description,
                                Id = Guid.NewGuid(),
                                Title = request.Title,
                                UserId = user.Id
                            };
                            await _unitOfWork.GetRepository<Notification>().AddAsync(notification);
                            await _unitOfWork.SaveAsync();
                        };
                        return new Response<string>(response, message: "Succeed");
                    }
                }
            }
            return new Response<string>(message: "Send Notifications Failed");
        }
    }
}
