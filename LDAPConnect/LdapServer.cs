using System;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Net;
using System.Security.Permissions;

namespace LDAPConnect
{
    public class LdapServer
    {

        public LdapServer(string hostName, string baseDn, string filter)
        {
            this.HostName = hostName;
            this.BaseDn = baseDn;
            this.Filter = filter;
        }

        public string HostName { get; private set; }
        public string BaseDn { get; set; }
        public string Filter { get; set; }

        [SecurityPermission(SecurityAction.Demand)]
        public LdapUser Authenticate(string userName, string password)
        {
            using (var ldap = new LdapConnection(new LdapDirectoryIdentifier(this.HostName)))
            {
                ldap.SessionOptions.ProtocolVersion = 3;

                ldap.AuthType = AuthType.Anonymous;
                ldap.Bind();
                var dn = GetDn(ldap, userName);

                if (dn != null)
                {
                    try
                    {
                        ldap.AuthType = AuthType.Basic;
                        ldap.Bind(new NetworkCredential(dn, password));
                        return GetUser(ldap, dn);
                    }
                    catch (DirectoryOperationException ex1)
                    { //Invalid user.
                        Debug.WriteLine(ex1.Message);
                    }
                    catch (LdapException ex2)
                    { //Invalid password.
                        Debug.WriteLine(ex2.Message);
                    }
                }
            }
            return null;
        }

        private String GetDn(LdapConnection ldap, String userName)
        {
            var request = new SearchRequest(this.BaseDn, string.Format(CultureInfo.InvariantCulture, this.Filter, userName), SearchScope.Subtree);
            var response = (SearchResponse)ldap.SendRequest(request);
            if (response.Entries.Count == 1)
            {
                return response.Entries[0].DistinguishedName;
            }
            return null;
        }

        private LdapUser GetUser(LdapConnection ldap, String dn)
        {
            var request = new SearchRequest(dn, "(objectclass=*)", SearchScope.Base);
            var response = (SearchResponse)ldap.SendRequest(request);
            if (response.Entries.Count == 1)
            {
                return new LdapUser(response.Entries[0]);
            }
            return null;
        }

    }
}