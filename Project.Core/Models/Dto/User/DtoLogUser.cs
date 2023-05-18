using Newtonsoft.Json;
using System;

namespace Project.Core.Models.Dto.User
{
    public class DtoLogUser
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Login")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "PasswordHash")]
        public string PasswordHash { get; set; }

        [JsonProperty(PropertyName = "LastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "FirstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "MiddleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "Phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "Deleted")]
        public Boolean Deleted { get; set; }

        [JsonProperty(PropertyName = "OrganizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty(PropertyName = "notificationUpdateStatusDataIndicator")]
        public Boolean NotificationUpdateStatusDataIndicator { get; set; }

        [JsonProperty(PropertyName = "notificationUpdateValueDataIndicator")]
        public Boolean NotificationUpdateValueDataIndicator { get; set; }

        [JsonProperty(PropertyName = "notificationUpdateCommentDataIndicator")]
        public Boolean NotificationUpdateCommentDataIndicator { get; set; }
    }
}
