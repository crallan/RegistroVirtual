using Excel;
using Models;
using Services.Model;
using Services.Specializations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        RegistroVirtualEntities context = new RegistroVirtualEntities();

        public StudentModel Get(string id)
        {
            var student = from s in context.Students
                          where s.Id.ToString().Equals(id)
                          select new StudentModel()
                           {
                               Id = s.Id,
                               CardId = s.CardId,
                               FirstName = s.FirstName,
                               LastName = s.LastName,
                               ClassId = s.Classes.Id
                           };

            return student.FirstOrDefault();
        }

        public StudentModel GetStudentByCardId(string cardId)
        {
            var student = from s in context.Students
                          where s.CardId.Equals(cardId)
                          select new StudentModel()
                          {
                              Id = s.Id,
                              CardId = s.CardId,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              ClassId = s.Classes.Id
                          };

            return student.FirstOrDefault();
        }

        public IEnumerable<StudentModel> GetList()
        {
            var students = from s in context.Students
                          select new StudentModel()
                          {
                              Id = s.Id,
                              CardId = s.CardId,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              ClassId = s.Classes.Id
                          };

            return students;
        }

        public IEnumerable<StudentModel> GetListByClass(int classId)
        {
            var students = from s in context.Students
                           where s.Classes.Id.Equals(classId)
                           select new StudentModel()
                           {
                               Id = s.Id,
                               CardId = s.CardId,
                               FirstName = s.FirstName,
                               LastName = s.LastName,
                               ClassId = s.Classes.Id
                           };

            return students;
        }

        public bool Import(ImportModel importModel)
        {
            bool result = false;

            try
            {
                if (!string.IsNullOrEmpty(importModel.FilePath))
                {
                    FileStream stream = File.Open(importModel.FilePath, FileMode.Open, FileAccess.Read);
                    string extension = Path.GetExtension(importModel.FilePath);

                    if (extension.Equals(".xls") || extension.Equals(".xlsx"))
                    {
                        DataSet importFile = new DataSet();

                        IExcelDataReader excelReader = null;

                        if (extension.Equals(".xls"))
                        {
                            //Reading from a binary Excel file ('97-2003 format; *.xls)
                            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else
                        {
                            //Reading from a OpenXml Excel file (2007 format; *.xlsx)
                            excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }

                        excelReader.IsFirstRowAsColumnNames = true;

                        //The result of each spreadsheet will be created in the result.Tables
                        importFile = excelReader.AsDataSet();

                        //Loop over each sheet of the document

                        foreach (DataTable sheet in importFile.Tables)
                        {
                            //Create the class for these 
                            string className = sheet.Rows.OfType<DataRow>().Where(r => !string.IsNullOrEmpty(r["SECCION"].ToString())).FirstOrDefault()["SECCION"].ToString();
                            int currentYear = DateTime.Now.Year;

                            if (!string.IsNullOrEmpty(className))
                            {
                                ClassRepository classRepository = new ClassRepository();

                                Classes dbClass = context.Classes.Where( c=> c.Name == className && c.YearCreated == currentYear).FirstOrDefault();

                                if (dbClass == null || dbClass.Id.Equals(0)) {
                                    dbClass = new Classes();

                                    dbClass.Name = className;
                                    dbClass.Institution = context.Institution.Single(p => p.Id.ToString().Equals(importModel.InstitutionId));

                                    string schoolYear = className.Split('-').Count() > 0 ? className.Split('-').First() : "1";
                                    dbClass.SchoolYears = context.SchoolYears.Single(s => s.Year.ToString().Equals(schoolYear));
                                    dbClass.YearCreated = currentYear;

                                    RegisterProfiles profile = context.RegisterProfiles.Where(x => x.SchoolYears.Id == dbClass.SchoolYears.Id && x.YearCreated == currentYear).FirstOrDefault();

                                    if (profile != null && profile.Id > 0)
                                    {
                                        dbClass.RegisterProfiles = profile;
                                        dbClass.RegisterProfiles1 = profile;
                                        dbClass.RegisterProfiles2 = profile;
                                    }
                                    
                                    context.Classes.Add(dbClass);
                                    int resultClass = context.SaveChanges();
                                }

                                string classId = dbClass.Id.ToString();

                                if (!classId.Equals(0))
                                {
                                    Classes @class = context.Classes.Single(p => p.Id.ToString().Equals(classId));

                                    foreach (DataRow row in sheet.Rows)
                                    {
                                        string studentName = row["NOMBRE"].ToString().ToUpper();
                                        string studentCard = row["CEDULA"].ToString().ToUpper();
                                        string studentLastName1 = row["PRIMER APELLIDO"].ToString().ToUpper();
                                        string studentLastName2 = row["SEGUNDO APELLIDO"].ToString().ToUpper();

                                        if (!string.IsNullOrEmpty(studentCard) && !string.IsNullOrEmpty(studentName))
                                        {
                                            Students student = context.Students.Where(c => c.CardId.Equals(studentCard)).FirstOrDefault();

                                            if (student == null || student.Id.Equals(0))
                                            {
                                                student = new Students();
                                                student.FirstName = studentName;
                                                student.CardId = studentCard;
                                                student.LastName = studentLastName1 + " " + studentLastName2;
                                                student.Classes = @class;
                                                context.Students.Add(student);
                                            }
                                            else {
                                                student.Classes = @class;
                                            }
                                        }
                                    }

                                    int resultStudents = context.SaveChanges();

                                }
                            }
                        }

                        //Free resources (IExcelDataReader is IDisposable)
                        excelReader.Close();

                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex) {
                result = false;
            }

            return result;
        }

        public bool Save(StudentModel student)
        {
            Students dbStudent = new Students();

            int result;

            try
            {

                //Add
                if (student.Id.Equals(0))
                {
                    dbStudent.FirstName = student.FirstName;
                    dbStudent.LastName = student.LastName;
                    dbStudent.CardId = student.CardId;
                    dbStudent.Classes = context.Classes.Single(p => p.Id.Equals(student.ClassId));

                    context.Students.Add(dbStudent);
                    result = context.SaveChanges();
                }
                else
                {
                    // get the record
                    dbStudent = context.Students.Single(p => p.Id.Equals(student.Id));

                    // set new values
                    dbStudent.FirstName = student.FirstName;
                    dbStudent.LastName = student.LastName;
                    dbStudent.CardId = student.CardId;
                    dbStudent.Classes = context.Classes.Single(p => p.Id.Equals(student.ClassId));

                    // save them back to the database
                    result = context.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
