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

                        //Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                        if (extension.Equals(".xls"))
                        {
                            //Reading from a binary Excel file ('97-2003 format; *.xls)
                            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }

                        excelReader.IsFirstRowAsColumnNames = true;

                        //The result of each spreadsheet will be created in the result.Tables
                        importFile = excelReader.AsDataSet();

                        //Loop over each sheet of the document

                        foreach (DataTable sheet in importFile.Tables)
                        {
                            //Create the class for these students
                            ClassRepository classRepository = new ClassRepository();
                            Classes dbClass = new Classes();
                            dbClass.Name = sheet.Rows[0]["SECCION"].ToString();
                            dbClass.Institution = context.Institution.Single(p => p.Id.Equals(1));

                            context.Classes.Add(dbClass);
                            int resultClass = context.SaveChanges();
                            string classId = dbClass.Id.ToString();

                            if (!classId.Equals(0))
                            {
                                Classes @class = context.Classes.Single(p => p.Id.ToString().Equals(classId));

                                foreach (DataRow row in sheet.Rows)
                                {
                                    Students student = new Students();
                                    student.FirstName = row["NOMBRE"].ToString().ToUpper();
                                    student.LastName = row["PRIMER APELLIDO"].ToString().ToUpper() + " " + row["SEGUNDO APELLIDO"].ToString().ToUpper();
                                    student.Classes = @class;

                                    context.Students.Add(student);
                                }

                                int resultStudents = context.SaveChanges();

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
            catch (Exception) {
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
