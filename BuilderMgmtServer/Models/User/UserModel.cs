using builder_mgmt_server.Controllers;
using builder_mgmt_server.Database;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Models.TasksBusyness;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.User
{
    public class UserModel : IUserModel
    {

        public IDbOperations DB;

        public UserIdService UserIdSvc;

        public UserModel(IDbOperations db, IUserIdService userIdSvc)
        {
            DB = db;
            UserIdSvc = (UserIdService)userIdSvc;
        }

        public async Task<ObjectId> UpdateUserAsync(UpdateUserDO user)
        {
            var e = new UserEntity()
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                desc = user.Desc,
                mail = user.Mail,
                phone = user.Phone,
                website = user.Website                
            };

            if (user.Location != null)
            {
                e.location = new LocationSE()
                {
                    text = user.Location.Text,
                    coords = user.Location.Coords
                };
            };

            var userEntity = await DB.ReplaceOneAsync(e);

            return e.id;
        }

        //public async Task AddCoworkerAsync(NewCoworkerDO coworker)
        //{
        //    var ce = await GetCoworkersEntitySafeAsync(coworker.requestingUser);

        //    var binding = new CoworkerBindingSE()
        //    {
        //        user_id = coworker.targetUser
        //    };

        //    var filter = DB.F<CoworkersEntity>().Eq(p => p.user_id, coworker.requestingUser);

        //    var update = DB.U<CoworkersEntity>().Push(p => p.friends, binding);
        //    var res = await DB.UpdateAsync(filter, update);

        //    //ce.friends.Add(binding);
        //    //await DB.ReplaceOneAsync<CoworkersEntity>(ce);
        //}

        //private async Task<CoworkersEntity> GetCoworkersEntitySafeAsync(ObjectId userId)
        //{
        //    var coworkersEntity = DB.FOD<CoworkersEntity>((c) => c.user_id == userId);
        //    if (coworkersEntity != null)
        //    {
        //        return coworkersEntity;
        //    }

        //    var e = new CoworkersEntity()
        //    {
        //        id = ObjectId.GenerateNewId(),
        //        user_id = userId,
        //        friends = new List<CoworkerBindingSE>()
        //    };
        //    await DB.SaveAsync(e);
        //    return e;
        //}
    }

    public class UpdateUserDO
    {
        public ObjectId Id;
        public string FirstName;
        public string LastName;

        public string Desc;

        public string Phone;

        public string Mail;

        public string Website;

        public LocationDO Location { get; set; }

        

    }

    public class LocationDO
    {
        public string Text;
        public List<double> Coords;
    }



    public class NewCoworkerDO
    {
        public ObjectId requestingUser;
        public ObjectId targetUser;
    }
}
