using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Knowzy.Xamarin.Model
{
    // includes required and server set properties only for now
    // full list here: https://developer.microsoft.com/en-us/graph/docs/api-reference/beta/resources/projectrome_activity
    public class Activity
    {
        // server 
        public DateTimeOffset CreatedDateTime { get; set; }

        // server
        public DateTimeOffset LastModifiedDateTime { get; set; }

        // server
        public string Id { get; set; }

        // required
        public string AppActivityId { get; set; }

        // required
        public string ActivitySourceHost { get; set; }

        // required
        public string ActivationUrl { get; set; }

        // required
        public VisualInfo VisualElements { get; set; }
    }
}
