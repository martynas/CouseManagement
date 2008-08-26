using System;
using Testing.Tests;
using System.Diagnostics;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            MembershipTests mTest = new MembershipTests();
            //mTest.DeleteUser("mrt");
            //mTest.CreateNewUser("mrt", "mrt@localhost", "laikinas", "275-------ATRE-TEST");
            mTest.CreateNewUser("trainer", "trainer@localhost", "laikinas", "-SUPERUSER---------");
            Debug.WriteLine(mTest.UsersCount());
        }
    }
}
