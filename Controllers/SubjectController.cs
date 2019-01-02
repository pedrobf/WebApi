using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    // Rota padrão.
    [Route("api/[controller]")]
    public class SubjectController : Controller
    {
        // Variavel private para receber o parametro.
        private readonly DatabaseContext database;

        // Construtor que recebe um parametro da classe DatabaseContext.
        public SubjectController(DatabaseContext context)
        {
            database = context;
        }

        // Método Get onde lista todas as materias.
        [HttpGet]
        public List<Subject> GetAll()
        {
            return database.Subjects.ToList();
        }

        // Método Get onde lista a materia por Id.
        [HttpGet("{id}", Name = "GetMateria")]
        public IActionResult GetById(long Id)
        {
            var subject = database.Subjects.Find(Id);
            if (subject == null)
                return NotFound();

            return Ok(subject);
        }

        // Método Post para criar uma materia, passando por parametro nome da materia e Id a qual turma pertence.
        // Validação do nome da materia, fazendo consulta para nao poder cadastrar duas materias com o mesmo nome.
        [HttpPost]
        public IActionResult Create([FromBody] Subject subject)
        {
            if (subject == null)
            {
                return BadRequest();
            }

            var stringName = database.Subjects.SingleOrDefault(s => s.Name == subject.Name);
            if (stringName != null)
            {
                return BadRequest(new { message = "This subject is already registered" });
            }

            database.Subjects.Add(subject);
            database.SaveChanges();
            return StatusCode(201, subject);
        }

        // Método Put para atualizar materia por Id, podendo atualizar nome e id que a turma pertence.
        [HttpPut("{id}")]
        public IActionResult Update(long Id, [FromBody] Subject subject)
        {
            var result = database.Subjects.Find(Id);
            if (result == null || result.Id != Id)
            {
                return BadRequest(ModelState);
            }

            result.Name = subject.Name;
            result.CourseId = subject.CourseId;

            database.Subjects.Update(result);
            database.SaveChanges();
            return NoContent();
        }

        // Método Delete por Id.
        [HttpDelete("{id}")]
        public IActionResult Delete(long Id)
        {
            var subject = database.Subjects.Find(Id);
            if (subject == null)
            {
                return NotFound();
            }
            database.Subjects.Remove(subject);
            database.SaveChanges();
            return NoContent();
        }
    }
}