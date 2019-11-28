/*using System.Collections.Generic;
using System.Linq;
using CIS174_TestCoreApp.Entities;
using CIS174_TestCoreApp.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CIS174_TestCoreApp
{
    public class DbService
    {
        readonly PersonContext _context;
        readonly ILogger<DbService> _log;


        public PeopleService(UserContext context, ILoggerFactory factory)
        {
            _context = context;
            _log = factory.CreateLogger<DbService>();
        }

        public ICollection<Person> GetPeople()
        {
            return _context.People
                .Where(r => !r.IsDeleted)
                .Select(x => new Person
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BirthDate = x.BirthDate,
                    City = x.City,
                    State = x.State,
                    IsDeleted = x.IsDeleted,
                    SetOfAccomplishments = x.SetOfAccomplishments,
                })
                .ToList();
        }

        public ICollection<PeopleBriefViewModel> GetPeopleBrief()
        {
            return _context.People
                .Where(r => !r.IsDeleted)
                .Select(x => new PeopleBriefViewModel
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    NumberOfAccomplishments = x.SetOfAccomplishments.Count,
                })
                .ToList();
        }



        public int CreatePerson(CreatePersonCommand cmd)
        {
            var person = cmd.ToPerson();
            _context.Add(person);
            _context.SaveChanges();
            return person.Id;
        }

        public void CreateAccessLog(PersonLog log)
        {
            // commented out for "Change the Web API logs that we created to log ONLY to the file"
            //_context.PeopleLog.Add(log);
            //_context.SaveChanges();
            _log.LogInformation("accessLog:{accessLog}", JsonConvert.SerializeObject(log));
        }

        public void CreateExceptionLog(ErrorLog log)
        {
            // commented out for "Change the Web API logs that we created to log ONLY to the file"
            // _context.ErrorLog.Add(log);
            //_context.SaveChanges();
            _log.LogError("errorLog:{errorLog}", JsonConvert.SerializeObject(log));
        }

        public PersonDetailViewModel GetPersonDetails(int id)
        {
            return _context.People
                .Where(x => x.Id == id)
                .Where(x => !x.IsDeleted)
                .Select(x => new PersonDetailViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BirthDate = x.BirthDate,
                    State = x.State,
                    City = x.City,
                    SetOfAccomplishments = x.SetOfAccomplishments
                    .Select(item => new PersonDetailViewModel.Item
                    {
                        Name = item.Name,
                        DateOfAccomplishment = item.DateOfAccomplishment,
                    })
                })
                .SingleOrDefault();
        }

        public Person GetPerson(int personId)
        {
            return _context.People
                .Where(x => x.Id == personId)
                .SingleOrDefault();
        }

        public UpdatePersonCommand GetPersonForUpdate(int PersonId)
        {
            return _context.People
                .Where(x => x.Id == PersonId)
                .Where(x => !x.IsDeleted)
                .Select(x => new UpdatePersonCommand
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BirthDate = x.BirthDate,
                    City = x.City,
                    State = x.State,
                    SetOfAccomplishments = x.SetOfAccomplishments,
                })
                .SingleOrDefault();
        }

        public void UpdatePerson(UpdatePersonCommand cmd)
        {
            var person = _context.People.Find(cmd.Id);
            if (person == null)
            {
                _log.LogWarning("Person #{id} doesn't exist", cmd.Id);
                return;
            }
            if (person.IsDeleted)
            {
                _log.LogWarning("Unable to update person #{id} because they are deleted", cmd.Id);
                return;
            }

            cmd.UpdatePerson(person);
            _context.SaveChanges();
        }

        public void DeletePerson(int personId)
        {
            var person = _context.People.Find(personId);
            if (person == null)
            {
                _log.LogWarning("Unable to delete person #{id} because they doesn't exist", personId);
                return;
            }
            if (person.IsDeleted)
            {
                _log.LogWarning("Unable to delete person #{id} because they are already deleted", personId);
                return;
            }
            person.IsDeleted = true;
            _context.SaveChanges();
        }

        public bool DoesPersonExist(int id)
        {
            return _context.People
                .Where(x => !x.IsDeleted)
                .Where(x => x.Id == id)
                .Any();
        }

        public bool DoesPersonNameMatch(int id, string name)
        {
            return _context.People
                .Where(x => !x.IsDeleted)
                .Where(x => x.Id == id)
                .Where(x => (x.FirstName + " " + x.LastName) == name)
                .Any();
        }

    }
}
*/