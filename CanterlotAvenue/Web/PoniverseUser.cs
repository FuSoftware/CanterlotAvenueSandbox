using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanterlotAvenue.Web
{
    class PoniverseUser
    {
        public int Id { get; }
        public string Username { get; }
        public string DisplayName { get; }
        public string Slug { get; }
        public string Email { get; }
        public string Permissions { get; } //Technically may be an object or something
        public bool Activated { get; }
        public string ActivatedAt { get; }
        public string CreatedAt { get; }
        public string UpdatedAt { get; }
        public string EmailHash { get; }
        public string UsernameHash { get; }

        string LoginData { get; } //Json data used when login


        public PoniverseUser(string json)
        {
            dynamic oJson = JsonConvert.DeserializeObject(json);
            this.Id = oJson.id;
            this.Username = oJson.username;
            this.DisplayName = oJson.display_name;
            this.Slug = oJson.slug;
            this.Email = oJson.email;
            this.Permissions = oJson.permissions;
            this.Activated = oJson.activated;
            this.ActivatedAt = oJson.activated_at;
            this.CreatedAt = oJson.created_at;
            this.UpdatedAt = oJson.updated_at;
            this.EmailHash = oJson.email_hash;
            this.UsernameHash = oJson.username_hash;

            this.LoginData = json;
        }
    }
}
