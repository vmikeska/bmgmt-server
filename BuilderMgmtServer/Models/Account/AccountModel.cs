using builder_mgmt_server.Database;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Models.Account;
using builder_mgmt_server.Models.TasksBusyness;
using builder_mgmt_server.Models.User;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models
{
    public class AccountModel : IAccountModel
    {
        public IDbOperations DB;
        public UserModel UModel;

        public AccountModel(IDbOperations db, IUserModel userModel)
        {
            DB = db;
            UModel = (UserModel)userModel;
        }

        public async Task<string> AddNewAccountAsync(NewAccountDO user)
        {
            var userId = ObjectId.GenerateNewId();

            var ae = new AccountEntity()
            {
                id = ObjectId.GenerateNewId(),
                user_id = userId,
                created = DateTime.UtcNow,

                mail = user.Mail,
                password = user.Password
            };

            await DB.SaveAsync(ae);

            var ue = new UserEntity()
            {
                id = userId,
                firstName = user.FirstName,
                lastName = user.LastName
            };

            await DB.SaveAsync(ue);

            var tempWorkloadData = TempData.GetTempWorkloadData(userId);
            await DB.SaveManyAsync(tempWorkloadData);

            return userId.ToString();
        }
    }
}
