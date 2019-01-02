using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    // Rota padrão.    
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        // Variavel private para receber o parametro.
        private readonly DatabaseContext database;

        // Construtor que recebe um parametro da classe DatabaseContext.
        public CourseController(DatabaseContext context)
        {
            database = context;
        }

        // Método Get onde lista todas as turmas, trazendo os alunos que estão vinculados a cada turma.
        [HttpGet]
        public IList<Course> GetAll()
        {
            var courses = database.Courses.Include(c => c.Students).ToList();
            return courses;
        }

        // Método Get por Id para trazer uma turma específica, onde faz a verificação se o ID existe.
        [HttpGet("{id}", Name = "GetTurma")]
        public IActionResult GetById(long Id)
        {
            var course = database.Courses.Find(Id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // Método Post para Criar uma Turma passando como parametro apenas seu nome.
        // Validacao do noma da turma, para nao poder cadastrar uma nova turma com um nome ja registrado.
        [HttpPost]
        public IActionResult Create([FromBody] Course course)
        {
            if (course == null)
            {
                return BadRequest(ModelState);
            }

            var stringName = database.Courses.SingleOrDefault(t => t.Name == course.Name);
            if (stringName != null)
            {
                return BadRequest(new { message = "This group is already registered" });
            }

            database.Courses.Add(course);
            database.SaveChanges();
            return StatusCode(201, course);
        }

        // Método Put para atualizar o nome da Turma, deve informar o Id.
        [HttpPut("{id}")]
        public IActionResult Update(long Id, [FromBody] Course course)
        {
            var result = database.Courses.Find(Id);
            if (result == null || result.Id != Id)
            {
                return BadRequest(ModelState);
            }

            result.Name = course.Name;
            database.Courses.Update(result);
            database.SaveChanges();
            return NoContent();
        }

        // Método Delete, deve informar o Id.
        [HttpDelete("{id}")]
        public IActionResult Delete(long Id)
        {
            var course = database.Courses.Find(Id);
            if (course == null)
            {
                return NotFound();
            }
            database.Courses.Remove(course);
            database.SaveChanges();
            return NoContent();
        }

    }
}