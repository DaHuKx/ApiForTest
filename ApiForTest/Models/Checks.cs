using System;
using System.Collections.Generic;

namespace ApiForTest.Models
{
    static public class Checks
    {
        static public string CheckPersonForProblems(Person person)
        {
            if (person.Id.HasValue)
            {
                return "Id must be null or undefined.";
            }

            if (person.HaveEmptyName())
            {
                return "A person must have name and display name.";
            }

            if (!person.HaveUniqueSkills())
            {
                return "A person can't have the same skills.";
            }

            foreach (var skill in person.Skills)
            {
                if ((skill.Level < 1) || (skill.Level > 10))
                {
                    return "Skills are defined in the range 1-10.";
                }
            }

            return null;
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
            if (oldData.EqualsTo(newData))
            {
                return null;
            }

            string result = new("");

            if (oldData.Name != newData.Name)
            {
                result += $"Name changed from '{oldData.Name}' to '{newData.Name}'\n\n";
            }
            else
            {
                result += $"Name: {oldData.Name}\n\n";
            }

            if (oldData.DisplayName != newData.DisplayName)
            {
                result += $"Display name changed from '{oldData.DisplayName}' to '{newData.DisplayName}'\n\n";
            }

            List<string> lostSkillsList = new();
            List<Skill> changedSkillList = new();

            foreach (var skill in oldData.Skills)
            {
                var tempLost = newData.Skills.Find(sk => sk.Name == skill.Name);
                var tempChange = newData.Skills.Find(sk => ((sk.Name == skill.Name)) && (sk.Level != skill.Level));

                if (tempLost == null)
                {
                    lostSkillsList.Add(skill.Name);
                }

                if (tempChange != null)
                {
                    result += $"Skill '{skill.Name}' was changed from '{skill.Level}' to '{tempChange.Level}'\n";
                }
            }

            if (lostSkillsList.Count != 0)
            {
                result += $"\nWas lost {lostSkillsList.Count} skill(s):\n";

                foreach (var lostSkill in lostSkillsList)
                {
                    result += $"{lostSkill}\n";
                }

                result += "\n";
            }

            List<Skill> newSkillsList = new();

            foreach (var skill in newData.Skills)
            {
                var temp = oldData.Skills.Find(sk => string.Equals(sk.Name, skill.Name));

                if (temp == null)
                {
                    newSkillsList.Add(skill);
                }
            }

            if (newSkillsList.Count != 0)
            {
                result += $"Was added {newSkillsList.Count} new skill(s):\n";

                foreach (var newSkill in newSkillsList)
                {
                    result += $"{newSkill.Name}: {newSkill.Level}\n";
                }
            }

            if (result.Trim() == $"Name: {oldData.Name}")
            {
                result += "Changed the order of skills.";
            }

            return result;
        }
    }
}
