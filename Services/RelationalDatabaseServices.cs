using DataLayer;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RelationalDatabaseServices
    {
        public bool SaveSQLCV(ResumeDetails model)
        {
            try
            {
                using (var ent = new ResumePortalDbEntities())
                {
                    //assuming logged in user for testing purpose
                    var assumedUserId = 1;
                    var name = model.FullName.Split(null);


                    //saving cv


                    var cv = new DataLayer.ResumeDetail()
                    {
                        FirstName = name.FirstOrDefault(),
                        LastName = name.LastOrDefault(),
                        DateOfBirth = model.DateOfBirth,
                        Phone = model.Phone,
                        Website = model.Website,
                        UserId = assumedUserId
                    };
                    ent.ResumeDetails.Add(cv);
                    ent.SaveChanges();



                    //get resume id
                    var resumeId = cv.ResumeId;
                    //saving skills

                    var skillList = model.Skills;
                    if (skillList != null && skillList.Any())
                    {
                        foreach (var skill in skillList)
                        {
                            var skilldata = new DataLayer.Skill()
                            {
                                ResumeId = resumeId,
                                LevelId = int.Parse(skill.Level),
                                Name = skill.Name,
                            };
                            ent.Skills.Add(skilldata);
                            ent.SaveChanges();
                        }
                    }

                    //saving languages
                    var languageList = model.Languages;
                    if (languageList != null && languageList.Any())
                    {
                        foreach (var language in languageList)
                        {
                            var languageData = new DataLayer.Language()
                            {
                                ResumeId = resumeId,
                                LevelId = int.Parse(language.Level),
                                Name = language.Name,
                            };
                            ent.Languages.Add(languageData);
                            ent.SaveChanges();
                        }
                    }

                    //saving job history
                    var jobHistoryList = model.JobHistories;
                    if (jobHistoryList != null && jobHistoryList.Any())
                    {
                        foreach (var job in jobHistoryList)
                        {
                            var jobData = new DataLayer.JobHistory()
                            {
                                ResumeId = resumeId,
                                Company = job.Company,
                                Description = job.Description,
                                IsActive = job.IsActive,
                                Location = job.Location,
                                StartDate = job.StartDate,
                                EndDate = job.EndDate,
                                Title = job.Title
                            };
                            ent.JobHistories.Add(jobData);
                            ent.SaveChanges();
                        }
                    }

                    //saving job history
                    var educationList = model.EducationInfos;
                    if (educationList != null && educationList.Any())
                    {
                        foreach (var edu in educationList)
                        {
                            var eduData = new DataLayer.Education()
                            {
                                ResumeId = resumeId,
                                Institute = edu.Institute,
                                Description = edu.Description,
                                IsActive = edu.IsActive,
                                Location = edu.Location,
                                StartDate = edu.StartDate,
                                EndDate = edu.EndDate,
                                Title = edu.Title
                            };
                            ent.Educations.Add(eduData);
                            ent.SaveChanges();
                        }
                    }
                    // saving Image info
                    var imageInfo = new DataLayer.Image()
                    {
                        Name = model.Image.Name,
                        ResumeId = resumeId
                    };
                    ent.Images.Add(imageInfo);
                    ent.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
