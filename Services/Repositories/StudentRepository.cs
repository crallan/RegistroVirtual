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
            throw new NotImplementedException();
        }

        public bool GetListByClass(int classId)
        {
            throw new NotImplementedException();
        }

        public bool Import(ImportModel importModel)
        {
            bool result = false;

            try
            {
                if (!string.IsNullOrEmpty(importModel.FilePath))
                {
                    FileStream stream = File.Open(importModel.FilePath, FileMode.Open, FileAccess.Read);

                    //Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    //The result of each spreadsheet will be created in the result.Tables
                    DataSet importFile = excelReader.AsDataSet();

                    //Free resources (IExcelDataReader is IDisposable)
                    excelReader.Close();

                    result = true;
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
            student.Id = 2;
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
