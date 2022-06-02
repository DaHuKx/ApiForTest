using Newtonsoft.Json;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiForTest.Models
{
    public class Person
    {
        public long? Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string DisplayName { get; set; }

        internal string _skills { get; set; }

        [NotMapped]
        public ImmutableArray<Skill> Skills
        {
            get
            {
                if (_skills == null)
                {
                    return new ImmutableArray<Skill>();
                }

                return JsonConvert.DeserializeObject<ImmutableArray<Skill>>(_skills);
            }

            set { _skills = JsonConvert.SerializeObject(value); }
        }

        /// <summary>
        /// Checks if a person has unique skills.
        /// </summary>
        public bool HaveUniqueSkills()
        {
            for (int firstIndex = 0; firstIndex < Skills.Length; firstIndex++)
            {
                for (int secondIndex = 0; secondIndex < Skills.Length; secondIndex++)
                {
                    if ((Skills[firstIndex].Name.Trim().ToLower() == Skills[secondIndex].Name.Trim().ToLower()) && (firstIndex != secondIndex))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if a person has empty name or display name.
        /// </summary>
        public bool HaveEmptyName()
        {
            return string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(DisplayName);
        }

        /// <summary>
        /// Checks if person are the same.
        /// </summary>
        public bool EqualsTo(Person person)
        {
            if ((person.Name.Trim().ToLower() != Name.Trim().ToLower()) || (person.DisplayName.Trim().ToLower() != DisplayName.Trim().ToLower()))
            {
                return false;
            }

            if (person.Skills.Length != Skills.Length)
            {
                return false;
            }

            for (int index = 0; index < Skills.Length; index++)
            {
                if ((person.Skills[index].Name.Trim().ToLower() != Skills[index].Name.Trim().ToLower()) || (person.Skills[index].Level != Skills[index].Level))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Change data on data from input person.
        /// </summary>
        internal void ChangeDataOn(Person person)
        {
            if ((person == null) || (EqualsTo(person)))
            {
                return;
            }

            Name = person.Name;
            DisplayName = person.DisplayName;
            Skills = person.Skills;
        }

        /// <summary>
        /// Finds a skill with the same name with input.
        /// </summary>
        /// <returns>Found skill or null.</returns>
        public Skill GetSkillEqualTo(Skill skill)
        {
            for (int index = 0; index < Skills.Length; index++)
            {
                if (Skills[index].Name == skill.Name)
                {
                    return Skills[index];
                }
            }

            return null;
        }
    }
}
