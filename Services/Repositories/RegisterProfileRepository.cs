using Models;
using Services.Model;
using Services.Specializations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.Repositories
{
    public class RegisterProfileRepository : IRegisterProfileRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public RegisterProfileModel Get(string id)
        {
            var profile = from r in context.RegisterProfiles
                          where r.Id.ToString().Equals(id)
                          select new RegisterProfileModel()
                          {
                              Id = r.Id,
                              Name = r.Name,
                              DailyWorkPercentage = (float)r.DailyWorkPercentage,
                              ConceptPercentage = (float)r.ConceptPercentage,
                              AssistancePercentage = (float)r.AssistancePercentage,
                              TrimesterId = r.Trimesters.Id,
                              SchoolYearId = r.SchoolYears.Id
                          };

            return profile.FirstOrDefault();
        }

        public IEnumerable<RegisterProfileModel> GetProfiles()
        {
            var profiles = from r in context.RegisterProfiles
                           select new RegisterProfileModel()
                           {
                               Id = r.Id,
                               Name = r.Name,
                               DailyWorkPercentage = (float)r.DailyWorkPercentage,
                               ConceptPercentage = (float)r.ConceptPercentage,
                               AssistancePercentage = (float)r.AssistancePercentage,
                               TrimesterId = r.Trimesters.Id,
                               SchoolYearId = r.SchoolYears.Id
                           };

            return profiles;
        }

        public bool Save(RegisterProfileModel profileModel)
        {
            RegisterProfiles dbProfiles = new RegisterProfiles();

            int result;

            try
            {
                //Add
                if (profileModel.Id.Equals(0))
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        dbProfiles.Name = profileModel.Name;
                        dbProfiles.Users = context.Users.Single(p => p.Id.Equals(profileModel.UserId));
                        dbProfiles.AssistancePercentage = profileModel.AssistancePercentage;
                        dbProfiles.DailyWorkPercentage = profileModel.DailyWorkPercentage;
                        dbProfiles.ConceptPercentage = profileModel.ConceptPercentage;
                        dbProfiles.Trimesters = context.Trimesters.Single(p => p.Id.Equals(profileModel.TrimesterId));
                        dbProfiles.SchoolYears = context.SchoolYears.Single(p => p.Id.Equals(profileModel.SchoolYearId));

                        context.RegisterProfiles.Add(dbProfiles);
                        result = context.SaveChanges();

                        int registerProfileId = dbProfiles.Id;

                        if (!registerProfileId.Equals(0))
                        {
                            foreach (ExamModel examModel in profileModel.Exams)
                            {
                                Exams exam = new Exams();
                                exam.Percentage = examModel.Percentage;
                                exam.Score = examModel.Score;
                                exam.RegisterProfiles = dbProfiles;

                                context.Exams.Add(exam);
                            }

                            foreach (ExtraclassWorkModel extraclassModel in profileModel.ExtraclassWorks)
                            {
                                ExtraclassWorks extraclass = new ExtraclassWorks();
                                extraclass.Percentage = extraclassModel.Percentage;
                                extraclass.RegisterProfiles = dbProfiles;

                                context.ExtraclassWorks.Add(extraclass);
                            }

                            result = context.SaveChanges();
                        }

                        transactionScope.Complete();
                    }

                }
                else
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        // get the record
                        dbProfiles = context.RegisterProfiles.Single(p => p.Id.Equals(profileModel.Id));

                        // set new values
                        dbProfiles.Name = profileModel.Name;
                        dbProfiles.Users = context.Users.Single(p => p.Id.Equals(profileModel.UserId));
                        dbProfiles.AssistancePercentage = profileModel.AssistancePercentage;
                        dbProfiles.DailyWorkPercentage = profileModel.DailyWorkPercentage;
                        dbProfiles.ConceptPercentage = profileModel.ConceptPercentage;
                        dbProfiles.Trimesters = context.Trimesters.Single(p => p.Id.Equals(profileModel.TrimesterId));
                        dbProfiles.SchoolYears = context.SchoolYears.Single(p => p.Id.Equals(profileModel.SchoolYearId));
                        
                        foreach (ExamModel examModel in profileModel.Exams)
                        {
                            Exams exam = context.Exams.FirstOrDefault(p => p.Id.Equals(examModel.Id));

                            if (exam != null)
                            {
                                exam.Percentage = examModel.Percentage;
                                exam.Score = examModel.Score;
                            }
                            else
                            {
                                exam = new Exams();
                                exam.Percentage = examModel.Percentage;
                                exam.Score = examModel.Score;
                                exam.RegisterProfiles = dbProfiles;

                                context.Exams.Add(exam);
                            }
                        }

                        foreach (ExtraclassWorkModel extraclassModel in profileModel.ExtraclassWorks)
                        {

                            ExtraclassWorks extraclass = context.ExtraclassWorks.FirstOrDefault(p => p.Id.Equals(extraclassModel.Id));

                            if (extraclass != null)
                            {
                                extraclass.Percentage = extraclassModel.Percentage;

                            }
                            else
                            {
                                extraclass = new ExtraclassWorks();
                                extraclass.Percentage = extraclassModel.Percentage;
                                extraclass.RegisterProfiles = dbProfiles;

                                context.ExtraclassWorks.Add(extraclass);
                            }
                        }

                        // save them back to the database
                        result = context.SaveChanges();

                        transactionScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
