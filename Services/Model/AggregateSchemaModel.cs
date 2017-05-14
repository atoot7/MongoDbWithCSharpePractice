using System;
using System.Collections.Generic;

namespace Services.Model
{
    public class AggregateSchemaModel
    {
        class BasicInfo
        {
            string FirstaNme { get; set; }
            string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Phone { get; set; }
            public string Website { get; set; }
            List<EducationInfo> EducationInfos { get; set; }
            //List<TrainingInfo> TrainingInfos { get; set; }
            List<JobHistory> JobHistories { get; set; }
            List<Skills> Skills { get; set; }
            List<Language> Languages { get; set; }
            Image Image { get; set; }
        }

        class EducationInfo
        {
            public string Title { get; set; }
            public string Institute { get; set; }
            public string Location { get; set; }
            public bool IsActive { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Description { get; set; }

        }

        class Language
        {
            public string Name { get; set; }
            public string Level { get; set; }
        }


        class JobHistory
        {
            public string Title { get; set; }
            public string Company { get; set; }
            public string Location { get; set; }
            public bool IsActive { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Description { get; set; }
        }

        class Skills
        {
            public string Name { get; set; }
            public string Level { get; set; }
        }

        class Image
        {
            public string Name;
        }
    }
}
