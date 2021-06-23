using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
// Sample Code from https://www.medo64.com/2012/10/ldap-authentication-from-c/
namespace LDAPConnect {
    class App
    {
        static void Main(string[] args)
        {
            var server = "10.1.10.4";
            var baseDn = "dc=nfenterprises,dc=com";
            var filter = "uid={0}";

            var userName = "chitchcock";
            var password = "serv1ce!";

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