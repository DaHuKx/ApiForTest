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

        static public Person CheckPersonInDataBase(long id, DataBase dataBase)
        {
            if (dataBase.IsEmpty())
            {
                return null;
            }

            var person = dataBase.Persons.Find(id);

            if (person == null)
            {
                return null;
            }

            return person;
        }

        static public string TakeDataChanges(Person oldPersonData, Person newPersonData)
        {
            if (oldPersonData.EqualsTo(newPersonData))
            {
                return null;
            }

            string result = new("");

            if (oldPersonData.Name != newPersonData.Name)
            {
                result += $"Name changed from '{oldPersonData.Name}' to '{newPersonData.Name}'\n\n";
            }
            else
            {
                result += $"Name: {oldPersonData.Name}\n\n";
            }

            if (oldPersonData.DisplayName != newPersonData.DisplayName)
            {
                result += $"Display name changed from '{oldPersonData.DisplayName}' to '{newPersonData.DisplayName}'\n\n";
            }

            List<string> lostSkillsList = new();
            List<Skill> changedSkillList = new();

            foreach (var skill in oldPersonData.Skills)
            {
                Skill tempLost = newPersonData.Skills.Find(sk => sk.Name == skill.Name);
                Skill tempChange = newPersonData.Skills.Find(sk => ((sk.Name == skill.Name)) && (sk.Level != skill.Level));

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

            foreach (var skill in newPersonData.Skills)
            {
                var temp = oldPersonData.Skills.Find(sk => string.Equals(sk.Name, skill.Name));

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

            if (result.Trim() == $"Name: {oldPersonData.Name}")
            {
                result += "Changed the order of skills.";
            }

            return result;
        }
    }
}
