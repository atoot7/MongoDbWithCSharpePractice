using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Model
{
    public class ResumeModel
    {
        public ObjectId _id { get; set; }

        [Required]
        public string PublicName { get; set; }
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string FullName { get; set; }
        public string ProfileImageName { get; set; }

        [Required]
        public string Category { get; set; }
        public List<ResumeDetails> Resumes { get; set; }
        public VisibilitySettingModel Privacy { get; set; }
    }
    public class ResumeDetails
    {
        public Guid _ResumeId { get; set; }
        public string ResumeTitle { get; set; }
        public string ResumeTags { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string Objective { get; set; }
        public List<EducationInfo> EducationInfos { get; set; }
        //List<TrainingInfo> TrainingInfos { get; set; }
        public List<JobHistory> JobHistories { get; set; }
        public List<Skills> Skills { get; set; }
        public List<Language> Languages { get; set; }
        public Image Image { get; set; }
        public string SetActive { get; set; }
        public VisibilitySettingModel Privacy { get; set; }
    }

    public class EducationInfo
    {
        public Guid _EducationId { get; set; }
        public string Title { get; set; }
        public string Institute { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

    }

    public class Language
    {
        public Guid _LanguageId { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
    }


    public class JobHistory
    {
        public Guid _HistoryId { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }

    public class Skills
    {
        public Guid _SkillId { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
    }

    public class Image
    {
        public string Name;
    }
}
