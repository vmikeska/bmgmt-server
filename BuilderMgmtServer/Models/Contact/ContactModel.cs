using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.Database;
using builder_mgmt_server.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Models.Contact
{
    public class ContactModel : IContactModel
    {

        public IDbOperations DB;

        public ContactModel(
            IDbOperations db
            )
        {
            DB = db;
        }

        public List<ContactResultDO> GetInitContacts(ObjectId userId)
        {
            var native = QueryNativeContacts(userId, u => true);
            var custom = QueryCustomContacts(userId, u => true);

            var nativeOrdered = native.OrderBy(i => i.Stared);
            var customOrdered = custom.OrderBy(i => i.Stared);

            var allItems = nativeOrdered.Concat(customOrdered).ToList();
            return allItems;
        }


        public ContactResultsDO FindContactsByStr(ObjectId userId, string str)
        {
            var result = new ContactResultsDO()
            {
                Custom = new List<ContactResultDO>(),
                Native = new List<ContactResultDO>()
            };

            if (str == null || str == string.Empty)
            {
                return result;
            }

            result.Native = QueryNativeContacts(userId, u =>
                u.firstName.ToLower().Contains(str)
                ||
                u.lastName.ToLower().Contains(str)
                );

            result.Custom = QueryCustomContacts(userId, u =>
                u.firstName.ToLower().Contains(str)
                ||
                u.lastName.ToLower().Contains(str)
                );

            return result;
        }

        private List<ContactResultDO> QueryNativeContacts(ObjectId userId, Func<UserEntity, bool> query)
        {
            var nativeContacts = DB.FOD<NativeContactsEntity>(i => i.user_id == userId);

            if (nativeContacts != null)
            {
                var uids = nativeContacts.contacts.Select(i => i.user_id);
                var users = DB.List<UserEntity>(u => uids.Contains(u.id));

                var matchingUsers = users.Where(query);

                var native = matchingUsers.Select((u) =>
                {
                    var c = nativeContacts.contacts.Where(i => i.user_id == u.id).FirstOrDefault();

                    var i = new ContactResultDO()
                    {
                        Id = u.id.ToString(),                        
                        Name = $"{u.firstName} {u.lastName}",
                        Stared = c.stared
                    };
                    return i;
                }).ToList();

                return native;
            }

            return new List<ContactResultDO>();
        }


        private List<ContactResultDO> QueryCustomContacts(ObjectId userId, Func<ContactSE, bool> query)
        {
            var customContacts = DB.FOD<CustomContactsEntity>(i => i.user_id == userId);

            if (customContacts != null)
            {
                var matchingContacts = customContacts.contacts.Where(query);

                var custom = matchingContacts.Select((u) =>
                {
                    var i = new ContactResultDO()
                    {
                        Name = $"{u.firstName} {u.lastName}",
                        Stared = u.stared
                    };
                    return i;
                }).ToList();

                return custom;
            }

            return new List<ContactResultDO>();
        }

        public List<ContactResultDO> FindNewContactsByStr(ObjectId userId, string str)
        {
            if (str == null || str == string.Empty)
            {
                return new List<ContactResultDO>();
            }

            var users = DB.List<UserEntity>(u =>
                (
                    u.firstName.ToLower().Contains(str)
                    ||
                    u.lastName.ToLower().Contains(str)
                )
                &&
                u.id != userId
            );

            var nce = DB.FOD<NativeContactsEntity>(u => u.user_id == userId);

            var results = users.Select(u =>
            {
                var res = new ContactResultDO()
                {
                    Id = u.id.ToString(),
                    Name = $"{u.firstName} {u.lastName}"
                };

                if (nce != null)
                {
                    var friendsBinding = nce.contacts.FirstOrDefault(i => i.user_id == u.id);
                    res.AlreadyAdded = friendsBinding != null;
                }

                return res;
            }).ToList();

            return results;
        }

        public async Task<string> AddNewNativeContactAsync(ObjectId userId, ObjectId contactId)
        {
            var me = DB.FODR<NativeContactsEntity>(i => i.user_id == userId);

            if (!me.Exists)
            {
                var ne = new NativeContactsEntity()
                {
                    id = ObjectId.GenerateNewId(),
                    user_id = userId,
                    contacts = new List<CoworkerBindingSE>()
                };

                await DB.SaveAsync<NativeContactsEntity>(ne);
            }

            var nc = new CoworkerBindingSE()
            {
                stared = false,
                user_id = contactId,
                id = ObjectId.GenerateNewId(),
            };

            var filter = DB.F<NativeContactsEntity>().Eq(p => p.user_id, userId);
            var update = DB.U<NativeContactsEntity>().Push(p => p.contacts, nc);
            UpdateResult result = await DB.UpdateAsync(filter, update);

            return nc.id.ToString();
        }

        public async Task<bool> RemoveNativeContactAsync(ObjectId userId, ObjectId contactId)
        {
            var filter = DB.F<NativeContactsEntity>().Eq(p => p.user_id, userId);
            var update =
                DB.PF<NativeContactsEntity, CoworkerBindingSE>(
                    x => x.contacts,
                    i => i.user_id == contactId
                );

            var res = await DB.UpdateAsync(filter, update);

            var modified = res.ModifiedCount > 1;

            return modified;
        }

        public async Task<string> AddNewCustomContactAsync(NewContactResultDO req)
        {
            var nc = new ContactSE()
            {
                stared = false,
                id = ObjectId.GenerateNewId(),
                firstName = req.FirstName,
                lastName = req.LastName
            };

            var filter = DB.F<CustomContactsEntity>().Eq(p => p.user_id, req.UserId);
            var update = DB.U<CustomContactsEntity>().Push(p => p.contacts, nc);
            UpdateResult result = await DB.UpdateAsync(filter, update);

            return nc.id.ToString();
        }
    }
}
