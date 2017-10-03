using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Knowzy.Xamarin.Model
{
    // full list here: https://developer.microsoft.com/en-us/graph/docs/api-reference/beta/resources/projectrome_historyitem
    public class HistoryItem
    {
        //public enum StatusCode { active, updated, deleted, ignored }

        //// server
        //public StatusCode Status { get; set; }

        //// server
        //public DateTimeOffset CreatedDateTime { get; set; }

        //// server
        //public DateTimeOffset LastModifiedDateTime { get; set; }

        //// required
        //public string Id { get; set; }

        // required
        public DateTimeOffset StartedDateTime { get; set; }

        // optional
        public DateTimeOffset LastActiveDateTime { get; set; }

        public string UserTimezone { get; set; }

    }
}
