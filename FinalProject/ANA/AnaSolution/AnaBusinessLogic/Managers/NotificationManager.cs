using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Contracts.Business;
using Ana.Contracts.Repository;
using Ana.Domain;
using Ana.Web.Models;

namespace Ana.Business.Managers
{
    public class NotificationManager : BaseManager, INotificationManager
    {
        private IUserNotificationRepository _userNotificationRepository;
        private IEmailProvider _emailProvider;
        private IBoardUserShareRepository _boardUserShareRepository;
        private IUserManager _userManager;

        public NotificationManager(IUserNotificationRepository userNotificationRepository
            ,IEmailProvider emailProvider
            ,IBoardUserShareRepository boardUserShareRepository
            ,IUserManager userManager)
        {
            _userNotificationRepository = userNotificationRepository;
            _userNotificationRepository.EnsureExist();
            _emailProvider = emailProvider;
            _boardUserShareRepository = boardUserShareRepository;
            _userManager = userManager;
        }

        public void ProcessPendingNotifications()
        {
            var message = _userNotificationRepository.GetMessage();

            _emailProvider.SendEmail(message.Email, message.Message, message.Url);

            _userNotificationRepository.DeleteMessage(message);
        }


        public void AlertBoardUpdate(Board board, User updatedBy)
        {
            var usersid =  _boardUserShareRepository.Query
                .Where(b => b.PartitionKey == board.RowKey)
                .ToList()
                .Select(u=>u.RowKey);

            foreach (var id in usersid)
            {
                var user = _userManager.GetUser(id);

                if (user.id != updatedBy.RowKey)
                {
                    //Check if is online and notify by signalR
                    _userNotificationRepository.AddMessage(new UserNotification
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = user.email,
                        Message = "Update of Board "+board.Name + " by " + user.name ,
                        Url = "http://ana.cloudapp.net/Board/" + board.UrlName + "/"+ board.RowKey,
                    });
                }
            }
        }

        public void AlertCardUpdate(Card card, User updatedBy)
        {
            
        }
    }
}
