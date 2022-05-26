using ApiForTest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ApiForTest.Controllers
{
    [Route("api/v1/[action]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {

        private readonly DataBase _dataBase;

        public PersonsController(DataBase dataBase)
        {
            _dataBase = dataBase;

            if (!_dataBase.Database.CanConnect())
            {
                throw new Exception("DataBase wasn't found.");
            }
        }

        [HttpGet, ActionName("persons")]
        public IActionResult GetAllPersons()
        {
            try
            {
                if (_dataBase.IsEmpty())
                {
                    return Ok("DataBase is empty.");
                }

                return Ok(_dataBase.Persons.ToArray());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}"), ActionName("person")]
        public IActionResult GetPerson(long id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest("Id can't be lower then 1.");
                }

                Person personData = _dataBase.Persons.Find(id);

                if (personData == null)
                {
                    return NotFound($"Database didn't have person with id {id}");
                }

                return Ok(personData);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost, ActionName("person")]
        public IActionResult AddPerson([FromBody] Person person)
        {
            try
            {
                string problem = Checks.CheckPersonForProblems(person);

                if (problem != null)
                {
                    return BadRequest(problem);
                }

                _dataBase.Add(person);
                _dataBase.SaveChanges();

                return Ok(person);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}"), ActionName("person")]
        public IActionResult ChangePerson(long id, [FromBody] Person newPersonData)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest("Id can't be lower then 1");
                }

                Person personData = _dataBase.Persons.Find(id);

                if (personData == null)
                {
                    return NotFound($"Database didn't have person with id {id}");
                }

                string problem = Checks.CheckPersonForProblems(newPersonData);

                if (problem != null)
                {
                    return BadRequest(problem);
                }

                string changes = Checks.TakeDataChanges(personData, newPersonData);

                if (changes == null)
                {
                    return BadRequest("Data is equal with old data.");
                }

                personData.Name = newPersonData.Name;
                personData.DisplayName = newPersonData.DisplayName;
                personData.Skills = newPersonData.Skills;

                _dataBase.SaveChanges();

                return Ok(changes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}"), ActionName("person")]
        public IActionResult DeletePerson(long id)
        {
            try
            {
                if (id < 1)
                {
                    BadRequest("Id can't be lower then 1");
                }

                Person personData = _dataBase.Persons.Find(id);

                if (personData == null)
                {
                    return NotFound($"Database didn't have person with id {id}");
                }

                _dataBase.Persons.Remove(personData);

                _dataBase.SaveChanges();

                return Ok("Deleted");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        } 
    }
}
