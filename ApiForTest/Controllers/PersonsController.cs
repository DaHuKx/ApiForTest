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

        private readonly BaseTest _baseTest;

        public PersonsController(BaseTest baseTest)
        {
            _baseTest = baseTest;

            if (!_baseTest.Database.CanConnect())
            {
                throw new Exception("DataBase wasn't found.");
            }
        }

        [HttpGet]
        public IActionResult GetAllPersons()
        {
            try
            {
                if (_baseTest.IsEmpty())
                {
                    return Ok("DataBase is empty.");
                }

                return Ok(_baseTest.Persons.ToArray());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetPerson(long id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest("Id can't be lower then 1.");
                }

                try
                {
                    return Ok(Checks.CheckPersonInDataBase(id, _baseTest));
                }
                catch (Exception e)
                {
                    return NotFound(e.Message);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public IActionResult AddPerson([FromBody] Person person)
        {
            try
            {
                string tempString = Checks.CheckPersonForProblems(person);

                if (tempString != null)
                {
                    return BadRequest(tempString);
                }

                _baseTest.Add(person);
                _baseTest.SaveChanges();

                return Ok(person);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult ChangePerson(long id, [FromBody] Person newPerson)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest("Id can't be lower then 1");
                }

                Person person;

                try
                {
                    person = Checks.CheckPersonInDataBase(id, _baseTest);
                }
                catch (Exception e)
                {
                    return NotFound(e.Message);
                }

                string tempString = Checks.CheckPersonForProblems(newPerson);

                if (tempString != null)
                {
                    return BadRequest(tempString);
                }

                string changes = Checks.TakeDataChanges(person, newPerson);

                if (changes == null)
                {
                    return BadRequest("Data is equal with old data.");
                }

                person.Name = newPerson.Name;
                person.DisplayName = newPerson.DisplayName;
                person.Skills = newPerson.Skills;

                _baseTest.SaveChanges();

                return Ok(changes);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson(long id)
        {
            try
            {
                if (id < 1)
                {
                    BadRequest("Id can't be lower then 1");
                }

                Person person;

                try
                {
                    person = Checks.CheckPersonInDataBase(id, _baseTest);
                }
                catch (Exception e)
                {
                    return NotFound(e.Message);
                }

                _baseTest.Persons.Remove(person);

                _baseTest.SaveChanges();

                return Ok("Deleted");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        } 
    }
}
