using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        internal bool EqualsTo(Person person)
        {
            if ((person.Name != Name) || (person.DisplayName != DisplayName))
            {
                return false;
            }

            if (person.Skills.Count != Skills.Count)
            {
                return false;
            }

            for (int i = 0; i < Skills.Count; i++)
            {
                if ((person.Skills[i].Name != Skills[i].Name) || (person.Skills[i].Level != Skills[i].Level))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
