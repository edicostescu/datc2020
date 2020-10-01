using System;
using System.Collections.Generic;


namespace L02.webapi 
{
    public class StudentsRepo {

        private static readonly Random rnd = new Random();
        private static readonly string[] Names = new[]
        {
            "John", "Walter", "Michael", "Josh", "Dani"
        };

        private static readonly string[] Faculties = new[]
        {
            "Harvard", "Oxford", "MIT", "Cambridge", "Princeton"
        };

        public List<Students> myStudents = new List<Students>() 
        {
            new Students(1, Names[rnd.Next(Names.Length)], Faculties[rnd.Next(Faculties.Length)]),
            new Students(2, Names[rnd.Next(Names.Length)], Faculties[rnd.Next(Faculties.Length)]),
            new Students(3, Names[rnd.Next(Names.Length)], Faculties[rnd.Next(Faculties.Length)]),
            new Students(4, Names[rnd.Next(Names.Length)], Faculties[rnd.Next(Faculties.Length)]),
            new Students(5, Names[rnd.Next(Names.Length)], Faculties[rnd.Next(Faculties.Length)])
        };
    }
}