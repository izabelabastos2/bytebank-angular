using System;
using System.Collections.Generic;
using System.Text;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;

namespace Vale.Geographic.Domain.Repositories.Interfaces
{
    public interface INotificationAnswerRepository : IRepository<NotificationAnswer>
    {
        NotificationAnswer GetLastByFocalPointId(Guid focalPointId);
    }
}
