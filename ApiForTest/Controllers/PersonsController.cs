using ApiForTest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
                try
                {
                    Checks.CheckPersonForProblems(person);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
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

                try
                {
                    Checks.CheckPersonForProblems(newPerson);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                string Changes = Checks.TakeDataChanges(person, newPerson);

                person.Name = newPerson.Name;
                person.DisplayName = newPerson.DisplayName;
                person.Skills = newPerson.Skills;

                _baseTest.SaveChanges();

                return Ok(Changes);
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
