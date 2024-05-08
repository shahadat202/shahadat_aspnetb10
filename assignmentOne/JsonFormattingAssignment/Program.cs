using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormattingAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Course course = new Course();
            course.Title = "Asp.Net";
            course.Fees = 30000;
            course.Teacher = new Instructor()
            {
                Name = "JalalUddin",
                Email = "jalaluddin@gmail.com",
                PresentAddress = new Address()
                {
                    Street = "5/A",
                    City = "Dhaka",
                    Country = "Bangladesh"
                },
                PermanentAddress = new Address()
                {
                    Street = "2/Z",
                    City = "Rajshahi",
                    Country = "Bangladesh"
                },
                Phones = new List<Phone>()
                {
                    new Phone() {Number = "01714288861", Extension = "123", CountryCode = "+880"},
                    new Phone() {Number = "01782072565", Extension = "456", CountryCode = "+880"}
                }
            };
            course.Topics = new List<Topic>()
            {
                new Topic() {Title = "C#", Description = "Intermeidate Course",
                    Sessions = new List<Session>()
{                       new Session() {DurationHour = 60, LearningObjective = "OOP"}
                    }
                },
                new Topic() {Title = "Asp.Net", Description = "Profession Course", 
                    Sessions = new List<Session>()
{                       new Session() {DurationHour = 120, LearningObjective = "Software Development"}
                    } 
                }
            };
            course.Tests = new List<AddmissionTest>()
            {
                new AddmissionTest()
                {
                    StartDate = new DateTime(2024,05,08),
                    EndDate = new DateTime(2024,05,20),
                    TestFees = 1000
                },
                new AddmissionTest()
                {
                    StartDate = new DateTime(2024,06,10),
                    EndDate = new DateTime(2024,06,20),
                    TestFees = 2000
                }
            };
            string JsonString = JsonFormattingAssignment.JsonFormatter.Serialize(course);
            //Console.WriteLine(JsonString);

            Console.WriteLine("Hello World!");

        }
    }
}
