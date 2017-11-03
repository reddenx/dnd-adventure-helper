using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models
{
    public class CharacterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CharacterDto(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}