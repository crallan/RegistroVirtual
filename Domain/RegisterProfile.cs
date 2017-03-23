using Models;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RegisterProfile
    {
        //Methods
        public IEnumerable<RegisterProfileModel> GetProfiles()
        {
            return new RegisterProfileRepository().GetProfiles();
        }

        public bool Save(RegisterProfileModel profile)
        {
            return new RegisterProfileRepository().Save(profile);
        }

        public RegisterProfileModel Get(string id)
        {
            return new RegisterProfileRepository().Get(id);
        }

        public RegisterProfileModel GetProfile(int schoolYear, int year, int trimester, int subject)
        {
            return new RegisterProfileRepository().GetProfile(schoolYear, year, trimester, subject);
        }
    }
}
