using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;

namespace LDAPConnect {
    class App
    {
        static void Main(string[] args)
        {
            var server = "192.168.56.100";
            var baseDn = "dc=localdomain";
            var filter = "uid={0}";

            var userName = "jdoe";
            var password = "test";

            var ldap = new LdapServer(server, baseDn, filter);
            var user = ldap.Authenticate(userName, password);
            if (user != null)
            {
                Console.WriteLine("UID ......: " + user.Uid);
                Console.WriteLine("Name .....: " + user.Name);
                Console.WriteLine("First name: " + user.FirstName);
                Console.WriteLine("Last name : " + user.LastName);
            }
            else
            {
                Console.WriteLine("Authorization failed.");
            }

            Console.ReadKey();
        }
    }
}