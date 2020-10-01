using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


namespace L02.webapi.Controllers 
{
    [ApiController]
    [Route("[controller]")]

    public class StudentsController : ControllerBase {
        StudentsRepo students = new StudentsRepo();

        [HttpGet]

        public List<Students> GetStudents()
        {
            return students.myStudents;
        }

        [HttpPut("{id}")] 

        public Students UpdateStudent(int id, [FromBody] Students student)
        {
            foreach (Students itr in students.myStudents) 
            {
                if (itr.id == id) 
                {
                    itr.name = student.name;
                    itr.faculty = student.faculty;
                    return itr;
                }
            }
            return null;
        }

        [HttpPost]

        public List<Students> InsertStudent([FromBody] Students student)
        {
            students.myStudents.Add(student);
            return students.myStudents;
        }
    
        [HttpDelete("{id}")]

        public List<Students> DeleteStudent(int id)
        {
            foreach (Students itr in students.myStudents)
            {
                if (id == itr.id) 
                {
                    students.myStudents.Remove(itr);
                    return students.myStudents;
                }
            }
            return null;
        }
    }
}