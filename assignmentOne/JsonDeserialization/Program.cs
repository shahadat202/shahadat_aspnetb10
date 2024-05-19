using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using JsonDeserialization;
using System.Text.Json;
class Program
{
    static void Main(string[] args)
    {
        string jsonString = @"
        {
            'Title': 'Asp.Net',
            'Fees': 30000,
            'Teacher': {
                'Name': 'JalalUddin',
                'Email': 'jalaluddin@gmail.com',
                'PresentAddress': {
                    'Street': '5/A',
                    'City': 'Dhaka',
                    'Country': 'Bangladesh'
                },
                'PermanentAddress': {
                    'Street': '2/Z',
                    'City': 'Rajshahi',
                    'Country': 'Bangladesh'
                },
                'Phones': [
                    { 'Number': '01714288861', 'Extension': '123', 'CountryCode': '+880' },
                    { 'Number': '01782072565', 'Extension': '456', 'CountryCode': '+880' }
                ]
            },
            'Topics': [
                {
                    'Title': 'C#',
                    'Description': 'Intermediate Course',
                    'Sessions': [
                        { 'DurationHour': 60, 'LearningObjective': 'OOP' }
                    ]
                },
                {
                    'Title': 'Asp.Net',
                    'Description': 'Professional Course',
                    'Sessions': [
                        { 'DurationHour': 120, 'LearningObjective': 'Software Development' }
                    ]
                }
            ],
            'Tests': [
                {
                    'StartDate': '2024-05-08',
                    'EndDate': '2024-05-20',
                    'TestFees': [100, 200]
                },
                {
                    'StartDate': '2024-06-10',
                    'EndDate': '2024-06-20',
                    'TestFees': [300, 400]
                }
            ]
        }"
        ;

        var course = JsonFormatter.Deserialize<Course>(jsonString);

        Console.WriteLine($"Course Title: {course.Title}");
        Console.WriteLine($"Course Fees: {course.Fees}");

        Console.WriteLine($"Teacher Name: {course.Teacher.Name}");
        Console.WriteLine($"Teacher Email: {course.Teacher.Email}");
        Console.WriteLine($"Teacher Present Address: {course.Teacher.PresentAddress.Street}, " +
            $"" + $"{course.Teacher.PresentAddress.City}, {course.Teacher.PresentAddress.Country}");
    }
}