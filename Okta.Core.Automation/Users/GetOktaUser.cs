using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Okta.Core;

namespace Okta.Core.Automation
{
    [Cmdlet(VerbsCommon.Get, "OktaUser")]
    public class GetOktaUser : OktaCmdlet
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 0,
            HelpMessage = "Id to retrieve"
        )]
        public string Id { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Search query string"
        )]
        public string Query { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = "Filter"
        )]
        public string Filter { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            Position = 3,
            HelpMessage = "Maximum number of users to retrieve"
            )]
        public int Limit { get; set; }

        [Parameter(
    Mandatory = false,
    ValueFromPipelineByPropertyName = true,
    Position = 4,
    HelpMessage = "Url (typically the next page url)"
)]
        public string Url { get; set; }

        protected override void ProcessRecord()
        {
            var usersClient = Client.GetUsersClient();
            if (!string.IsNullOrEmpty(Id))
            {
                var user = usersClient.Get(Id);
                WriteObject(user);
            }
            else
            {
                if (Limit > 0)
                {
                    Core.PagedResults<Models.User> res = null;
                    if (!string.IsNullOrEmpty(Url))
                    {
                        res = usersClient.GetList(new Uri(Url));
                    }
                    else
                    {
                        res = usersClient.GetList(pageSize: (Limit > 0) ? Limit : 200, filter: new FilterBuilder(Filter), searchType: SearchType.Filter, query: Query);
                    }
                    WriteObject(res, true);
                }
                else
                {
                    var users = usersClient.GetFilteredEnumerator(pageSize: (Limit > 0) ? Limit : 200, query: Query, filter: new FilterBuilder(Filter));
                    WriteObject(users);
                }

            }
        }
    }
}
