using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmritTulya.EntityLayer
{
    public class AccessTokenModel
    {
        public string Username { get; set; }
        public string token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public DateTime expires_in { get; set; }

    }
}
