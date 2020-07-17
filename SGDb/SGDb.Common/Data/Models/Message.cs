using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SGDb.Common.Data.Models
{
    public class Message
    {
        public string serializedData;

        public Message(object data)
        {
            this.GuidId = Guid.NewGuid().ToString();
            this.Data = data;
        }
        
        private Message()
        {
        }
        
        public int Id { get; private set; }

        public string GuidId { get; set; }

        public Type Type { get; private set; }

        public bool Published { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public void MarkAsPublished() => this.Published = true;
        
        [NotMapped]
        public object Data
        {
            get => JsonConvert.DeserializeObject(this.serializedData, this.Type,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
            set
            {
                this.Type = value.GetType();
                this.serializedData = JsonConvert.SerializeObject(value);
            }
        }
    }
}