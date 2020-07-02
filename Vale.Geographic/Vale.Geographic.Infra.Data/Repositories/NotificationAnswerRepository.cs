using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Domain.Entities;
using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Infra.Data.Base;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class NotificationAnswerRepository : Repository<NotificationAnswer>, INotificationAnswerRepository
    {
        public NotificationAnswerRepository(IUnitOfWork context) : base(context)
        {
        }

        public NotificationAnswer GetLastByFocalPointId(Guid focalPointId)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@"SELECT top 1 NA.[Id]
                                              ,NA.[CreatedAt]
                                              ,NA.[LastUpdatedAt]
                                              ,NA.[CreatedBy]
                                              ,NA.[LastUpdatedBy]
                                              ,NA.[Status]
                                              ,NA.[FocalPointId]                                              
                                              ,NA.[NotificationId]
                                              ,NA.[Answered]
                                          FROM [dbo].[NotificationAnswers] NA
                                          WHERE NA.[FocalPointId] = @FocalId
                                        ORDER BY  NA.[CreatedAt] DESC");

            param.Add("FocalId", focalPointId);

            NotificationAnswer result = this.Connection.Query<NotificationAnswer>(sqlQuery.ToString(), param,
                (IDbTransaction)this.Uow.Transaction).SingleOrDefault();

            return result;
        }
    }
}