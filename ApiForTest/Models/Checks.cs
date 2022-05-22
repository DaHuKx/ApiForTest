using System;
using System.Collections.Generic;

namespace ApiForTest.Models
{
    static public class Checks
    {
        static public void CheckPersonForProblems(Person person)
        {
            if (person.Id.HasValue)
            {
                throw new Exception("Id must be null or undefined.");
            }

            if (person.HaveEmptyName())
            {
                throw new Exception("A person must have name and display name.");
            }

            if (!person.HaveUniqueSkills())
            {
                throw new Exception("A person can't have the same skills.");
            }

            foreach (var skill in person.Skills)
            {
                if ((skill.Level < 1) || (skill.Level > 10))
                {
                    throw new Exception("Skill are defined in the range 1-10.");
                }
            }
        }

        static public Person CheckPersonInDataBase(long id, BaseTest baseTest)
        {
            if (baseTest.IsEmpty())
            {
                throw new Exception("DataBase is empty.");
            }

            var person = baseTest.Persons.Find(id);

            if (person == null)
            {
                throw new Exception("DataBase didn't have person with this id.");
            }

            return person;
        }

        static public string TakeDataChanges(Person oldData, Person newData)
        {
            string result = new("");

            if (oldData.Name != newData.Name)
            {
                result += $"Name changed from '{oldData.Name}' to '{newData.Name}'\n\n";
            }

            if (oldData.DisplayName != newData.DisplayName)
            {
                result += $"Display name changed from '{oldData.DisplayName}' to '{newData.DisplayName}'\n\n";
            }

            List<string> tempSkillsList = new();

            foreach (var skill in oldData.Skills)
            {
                var temp = newData.Skills.Find(sk => sk.Name == skill.Name);

                if (temp == null)
                {
                    tempSkillsList.Add(skill.Name);
                }
            }

            if (tempSkillsList.Count != 0)
            {
                result += $"Was lost {tempSkillsList.Count} skill(s):\n";

                foreach (var lostSkill in tempSkillsList)
                {
                    result += $"{lostSkill}\n";
                }
            }

            result += "\n";

            foreach (var skill in oldData.Skills)
            {
                var temp = newData.Skills.Find(sk => ((sk.Name == skill.Name)) && (sk.Level != skill.Level));

                if (temp != null)
                {
                    result += $"Skill '{skill.Name}' was changed from '{skill.Level}' to '{temp.Level}'\n";
                }
            }

            result += "\n";
            tempSkillsList.Clear();

            foreach (var skill in newData.Skills)
            {
                var temp = oldData.Skills.Find(sk => string.Equals(sk.Name, skill.Name));

                if (temp == null)
                {
                    tempSkillsList.Add(skill.Name);
                }
            }

            if (tempSkillsList.Count != 0)
            {
                result += $"Was added {tempSkillsList.Count} new skill(s):\n";

                foreach (var newSkill in tempSkillsList)
                {
                    result += $"{newSkill}\n";
                }
            }

            return result;
        }
    }
}
