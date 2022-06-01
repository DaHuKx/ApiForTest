using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApiForTest.Models
{
    static public class Checks
    {
        /// <summary>
        /// Return string with changes between old person data and new person data.
        /// </summary>
        static public string TakeDataChanges(Person oldPersonData, Person newPersonData)
        {
            if (oldPersonData.EqualsTo(newPersonData))
            {
                return null;
            }

            StringBuilder changes = new();

            if (oldPersonData.Name != newPersonData.Name)
            {
                changes.Append($"Name changed from '{oldPersonData.Name}' to '{newPersonData.Name}'\n\n");
            }
            else
            {
                changes.Append($"Name: {oldPersonData.Name}\n\n");
            }

            if (oldPersonData.DisplayName != newPersonData.DisplayName)
            {
                changes.Append($"Display name changed from '{oldPersonData.DisplayName}' to '{newPersonData.DisplayName}'\n\n");
            }

            List<string> lostSkillsList = new();

            foreach (Skill oldSkill in oldPersonData.Skills)
            {
                Skill newSkill = newPersonData.GetSkillEqualTo(oldSkill);

                if (newSkill == null)
                {
                    lostSkillsList.Add(oldSkill.Name);
                }
                else if (newSkill.Level != oldSkill.Level)
                {
                    changes.Append($"Skill '{oldSkill.Name}' was changed from '{oldSkill.Level}' to '{newSkill.Level}'\n");
                }
            }

            if (lostSkillsList.Count != 0)
            {
                changes.Append($"\nWas lost {lostSkillsList.Count} skill(s):\n");

                foreach (string lostSkill in lostSkillsList)
                {
                    changes.Append($"{lostSkill}\n");
                }

                changes.Append('\n');
            }

            List<Skill> addedSkillsList = new();

            foreach (Skill newSkill in newPersonData.Skills)
            {
                Skill oldSkill = oldPersonData.GetSkillEqualTo(newSkill);

                if (oldSkill == null)
                {
                    addedSkillsList.Add(newSkill);
                }
            }

            if (addedSkillsList.Count != 0)
            {
                changes.Append($"Was added {addedSkillsList.Count} new skill(s):\n");

                foreach (Skill newSkill in addedSkillsList)
                {
                    changes.Append($"{newSkill.Name}: {newSkill.Level}\n");
                }
            }

            if (changes.ToString().Trim() == $"Name: {oldPersonData.Name}")
            {
                changes.Append("Changed the order of skills.");
            }

            return changes.ToString();
        }

        /// <summary>
        /// Check validation of person. Return null if it's correct or string with problems.
        /// </summary>
        internal static string CheckPersonValidation(Person person)
        {
            if (person.Id != null)
            {
                return "Id most be null or undefined.";
            }

            ValidationContext validationContext = new(person);
            List<ValidationResult> validationResults = new();

            if (!Validator.TryValidateObject(person, validationContext, validationResults, true))
            {
                StringBuilder errors = new();

                foreach (ValidationResult error in validationResults)
                {
                    errors.Append(error.ErrorMessage);
                }

                return errors.ToString();
            }

            return null;
        }
    }
}
