using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Entities.Authorization;
using Vale.Geographic.Domain.Base.Interfaces;
using Vale.Geographic.Infra.Data.Base;
using System.Text;
using System.Data;
using System.Linq;
using Dapper;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class UserRepository: Repository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork context) : base(context)
        {

        }

        public User GetByMatricula(string matricula)
        {
            var param = new DynamicParameters();
            StringBuilder sqlQuery = new StringBuilder();

            sqlQuery.AppendLine(@"SELECT  US.[Id],
		                                  US.[CreatedAt],
		                                  US.[LastUpdatedAt],
		                                  US.[Status],
		                                  US.[Name],
		                                  US.[Matricula],
		                                  US.[Email],
		                                  US.[Profile]
	                                FROM dbo.[Users] US                            
	                                WHERE US.[Status] = 1
                                      AND US.[Matricula] = @Matricula");

            param.Add("Matricula", matricula);

            User result = this.Connection.Query<User>(sqlQuery.ToString(), param,
                (IDbTransaction)this.Uow.Transaction).FirstOrDefault();

            return result;
        }
    }
}
