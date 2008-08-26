using System;
using System.Collections.Generic;
using System.Text;
using Atrendia.CourseManagement.Logic;
using Atrendia.CourseManagement.Logic.Entities;

namespace Testing.Tests
{
    class EndpointTests
    {

        public static void Test()
        {
            COMEndpoint endpoint = new COMEndpoint("CDMDEV");
            Console.WriteLine("Activity ID:");
            string activityId = Console.ReadLine().Trim();
            Console.WriteLine("Contact Email:");
            string email = Console.ReadLine().Trim();

            Activity activity = endpoint.GetActivityById(activityId);
            if (activity == null)
            {
                Console.WriteLine("Activity not found");
                return;
            }

            List<Contact> toAdd = new List<Contact>();
            Contact contact = endpoint.GetContactByEmail(email, null);
            if (contact == null)
            {
                Console.WriteLine("Contact not found");
            }

            toAdd.Add(endpoint.GetContactByEmail(email, null));
            endpoint.AddParticipantsToActivity(activity, toAdd);
        }

    }
}
