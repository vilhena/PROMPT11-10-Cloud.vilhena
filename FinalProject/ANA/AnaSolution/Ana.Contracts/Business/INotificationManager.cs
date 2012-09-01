using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Web.Models;
using Ana.Domain;

namespace Ana.Contracts.Business
{
    public interface INotificationManager
    {
        void ProcessPendingNotifications();

        void AlertBoardUpdate(Board board, User updatedBy);
        void AlertCardUpdate(Card card, User updatedBy);

    }
}
