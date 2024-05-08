using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormattingAssignment
{
    public class Course
    {
        public string Title { get; set; }
        public double Fees { get; set; }
        public Instructor Teacher { get; set; }
        public List<Topic> Topics { get; set; }
        public List<AddmissionTest> Tests { get; set; }

    }
    public class AddmissionTest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TestFees { get; set; }
    }
    public class Topic
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Session> Sessions { get; set; }
    }
    public class Session
    {
        public int DurationHour { get; set; }
        public string LearningObjective { get; set; }
    }

    public class Instructor
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Address PresentAddress { get; set; }
        public Address PermanentAddress { get; set; }
        public List<Phone> Phones { get; set; }
    }
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
    public class Phone
    {
        public string Number { get; set; }
        public string Extension { get; set; }
        public string CountryCode { get; set; }
    }
}
