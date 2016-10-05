using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces.Models;

namespace HIS.WebApi.Auth.Data.Models
{
    public class Role: IRole<string>
    {
        public string Id { get; }
        public string Name { get; set; }
    }
}
