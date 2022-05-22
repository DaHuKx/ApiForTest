using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiForTest.Models
{
    public class Person
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        internal string _skills { get; set; }

        [NotMapped]
        public List<Skill> Skills
        {
            get { return _skills == null ? null : JsonConvert.DeserializeObject<List<Skill>>(_skills); }
            set { _skills = JsonConvert.SerializeObject(value); }
        }

        internal bool HaveUniqueSkills()
        {
            foreach (var skill in Skills)
            {
                string tempSkill = skill.Name.ToLower().Trim();

                if (Skills.FindAll(sk => sk.Name.ToLower().Trim() == tempSkill).Count > 1)
                {
                    return false;
                }
            }

            return true;
        }

        internal bool HaveEmptyName()
        {
            return string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(DisplayName);
        }
    }
}
