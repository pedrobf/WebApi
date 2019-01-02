using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace WebApi.Controllers
{
    // Rota padrão.
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        // Variavel private para receber o parametro.
        private readonly DatabaseContext database;

        // Construtor que recebe um parametro da classe DatabaseContext.
        public StudentController(DatabaseContext context)
        {
            database = context;
        }
        // Método Get onde lista todos Alunos.
        [HttpGet]
        public List<Student> GetAll()
        {
            return database.Students.ToList();
        }


        // Método Get onde filtra por parametro o sexo dos Alunos.
        [HttpGet("filterbygender")]
        public IActionResult GetAll([FromQuery] string gender)
        {
            if (!gender.ToLower().Equals("m") && !gender.ToLower().Equals("f"))
            {
                return BadRequest("Not found, enter with M for male or F for female");
            }

            IList<Student> students =
                database.Students
                        .Where(c => c.Gender.ToLower().Equals(gender.ToLower()) || string.IsNullOrEmpty(gender))
                        .ToList();

            return Ok(students);
        }

        // Método Get por Id para trazer um aluno específico, onde faz a verificação se o ID existe.
        [HttpGet("{id}")]
        public IActionResult GetById(long Id)
        {
            var student = database.Students.Find(Id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        // Método Post para criar um Aluno passando como parametro: nome, email, data de nascimento, sexo e turmaId.
        // Validação de dados nos atributos email, data de nascimento e sexo.
        [HttpPost]
        public IActionResult Create([FromBody] Student student)
        {
            if (student == null || ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            if (student.Birthday > DateTime.Today)
            {
                return BadRequest(new { message = "Invalid date" });
            }

            if (student.Gender != "M" && student.Gender != "F")
            {
                return BadRequest(new { message = "To inform the gender insert M for male or F for female" });
            }

            database.Students.Add(student);
            database.SaveChanges();

            return StatusCode(201, student);
        }

        // Método Put para atualizar aluno, validando entrada de dados do email e data de nascimento.
        [HttpPut("{id}")]
        public IActionResult Update(long Id, [FromBody] Student student)
        {
            var result = database.Students.Find(Id);

            if (result == null || result.Id != Id || ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            if (student.Birthday > DateTime.Today)
            {
                return BadRequest(new { message = "Invalid date" });
            }

            if (student.Gender != "M" && student.Gender != "F")
            {
                return BadRequest(new { message = "To inform the gender insert M for male or F for female" });
            }

            result.Name = student.Name;
            result.Email = student.Email;
            result.Gender = student.Gender;
            result.Birthday = student.Birthday;
            result.CourseId = student.CourseId;
            database.Students.Update(result);
            database.SaveChanges();
            return NoContent();
        }

        // Método para atualizar apenas o nome do Aluno por Id.
        [HttpPatch("{id}")]
        public IActionResult PatchUpdate(long Id, [FromBody] Student student)
        {
            var result = database.Students.Find(Id);

            if (result == null || result.Id != Id)
            {
                return BadRequest(ModelState);
            }

            result.Name = student.Name;
            database.Students.Update(result);
            database.SaveChanges();
            return NoContent();
        }

        // Método para deletar aluno passando parametro do id, onde ele verifica se este id é true.
        [HttpDelete("{id}")]
        public IActionResult Delete(long Id)
        {
            var aluno = database.Students.Find(Id);
            if (aluno == null)
            {
                return NotFound();
            }
            database.Students.Remove(aluno);
            database.SaveChanges();

            return NoContent();
        }
    }
}