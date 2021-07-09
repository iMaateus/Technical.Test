using System.Collections.Generic;
using Technical.Test.Business.Notifications;

namespace Technical.Test.Business.Interfaces
{
    public interface INotifier
    {
        bool HasNotification();

        List<Notification> GetNotifications();

        void Handle(Notification notification);
    }
}
