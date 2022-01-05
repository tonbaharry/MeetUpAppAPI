using System;

namespace MeetUpAppAPI.Helper
{
    public static class DateTimeHelper
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if(dob.Date > today.AddYears(-age))
            {
                age = age - 1;
            }
            return age;
        }
    }
}