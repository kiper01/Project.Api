using Newtonsoft.Json;
using System;

namespace Project.Core.Models.Dto.User
{
    public class DtoEditUser
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "newPassword")]
        public string NewPassword { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty(PropertyName = "notificationUpdateStatusDataIndicator")]
        public Boolean NotificationUpdateStatusDataIndicator { get; set; }

        [JsonProperty(PropertyName = "notificationUpdateValueDataIndicator")]
        public Boolean NotificationUpdateValueDataIndicator { get; set; }

        [JsonProperty(PropertyName = "notificationUpdateCommentDataIndicator")]
        public Boolean NotificationUpdateCommentDataIndicator { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public Boolean Deleted { get; set; }
    }
}
