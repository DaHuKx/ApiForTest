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
                string validation = Checks.CheckPersonValidation(person);

                if (validation != null)
                {
                    return BadRequest(validation);
                }

                _dataBase.AddAsync(person);
                _dataBase.SaveChangesAsync();

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
                Person personData = _dataBase.Persons.Find(id);

                if (personData == null)
                {
                    return NotFound($"Database didn't have person with id {id}");
                }

                string validation = Checks.CheckPersonValidation(newPersonData);

                if (validation != null)
                {
                    return BadRequest(validation);
                }

                string changes = Checks.TakeDataChanges(personData, newPersonData);

                if (changes == null)
                {
                    return BadRequest("Data is equal with old data.");
                }

                personData.ChangeDataOn(newPersonData);

                _dataBase.SaveChangesAsync();

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
                Person personData = _dataBase.Persons.Find(id);

                if (personData == null)
                {
                    return NotFound($"Database didn't have person with id {id}");
                }

                _dataBase.Persons.Remove(personData);
                _dataBase.SaveChangesAsync();

                return Ok("Deleted.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
