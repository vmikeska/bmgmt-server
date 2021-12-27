using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace builder_mgmt_server.Entities
{
    public class AccountEntity : EntityBase
    {
        public ObjectId user_id { get; set; }
        public string password { get; set; }
        public string mail { get; set; }
        public DateTime created { get; set; }
        //public bool EmailSent { get; set; }
        //public bool EmailConfirmed { get; set; }
    }

    public class UserEntity : EntityBase
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string desc { get; set; }

        public string phone { get; set; }

        public string mail { get; set; }

        public string website { get; set; }

        public LocationSE location { get; set; }

        public List<TagSE> fields { get; set; }
    }

    public class LocationSE
    {
        public string text { get; set; }
        public List<double> coords { get; set; }
    }

    public class UserSkillsBindingEntity : TagBindingBaseEntity { }
    public class UserSkillsTagEntity : TagBaseEntity { }


    public class TagBindingBaseEntity : EntityBase
    {
        public ObjectId entity_id { get; set; }

        public ObjectId tag_id { get; set; }
    }

    public class TagBaseEntity : EntityBase
    {
        public string name { get; set; }
    }

    public class SearchableEntity : EntityBase
    {
        public string name { get; set; }
        public string desc { get; set; }
        public ObjectId owner_id { get; set; }
        public bool finished { get; set; }
    }

    public class ProjectEntity : SearchableEntity
    {
        public LocationSE location { get; set; }
    }

    public class TaskEntity : SearchableEntity
    {
        
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public TaskTypeEnum type { get; set; }
        
        public int week { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int mid { get; set; }
        public int wid { get; set; }
        public int manHours { get; set; }
        public int manDays { get; set; }

        public LocationSE location { get; set; }

    }

    public class TaskParticipantEntity : TopicParticipantEntity { }

    public class ProjectParticipantEntity : TopicParticipantEntity { }


    public class TopicParticipantEntity : EntityBase
    {
        public ObjectId topic_id { get; set; }

        public ObjectId user_id { get; set; }

        public TopicParticipantEnum role { get; set; }

    }

    public class TaskChatMessagesEntity : BaseChatMessagesEntity { }
    public class ProjectChatMessagesEntity : BaseChatMessagesEntity { }

    public class BaseChatMessagesEntity : EntityBase
    {
        public ObjectId topic_id { get; set; }

        public List<ChatMessageSE> messages { get; set; }
    }

    public class ChatMessageSE
    {
        public ObjectId id { get; set; }
        public DateTime posted { get; set; }
        public string text { get; set; }
        public ObjectId author_id { get; set; }
    }

    public class ProjectsTaskEntity : EntityBase
    {
        public ObjectId task_id { get; set; }

        public ObjectId proj_id { get; set; }
    }




    public class NativeContactsEntity : EntityBase
    {
        public ObjectId user_id { get; set; }
        public List<CoworkerBindingSE> contacts { get; set; }

        //public List<ObjectId> Friends { get; set; }

        //public List<ObjectId> Proposed { get; set; }

        //public List<ObjectId> AwaitingConfirmation { get; set; }

        //public List<ObjectId> Blocked { get; set; }

    }



    public class CustomContactsEntity : EntityBase
    {
        public ObjectId user_id { get; set; }
        public List<ContactSE> contacts { get; set; }

    }

    public class CoworkerBindingSE
    {
        public ObjectId id { get; set; }

        public ObjectId user_id { get; set; }

        public bool stared { get; set; }

        // some custom stuff, possibly tags, etc

    }

    public class ContactSE
    {
        public ObjectId id { get; set; }

        public bool stared { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

    }

    public class TagSE
    {
        public ObjectId id { get; set; }

        public ObjectId cat_id { get; set; }

        public string name { get; set; }
    }
}
