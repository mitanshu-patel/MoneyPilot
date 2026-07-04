using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyPilot.Shared.Common
{
    public class OpenAIOptions
    {
        public string Url { get; set; }

        public string Model { get; set; }

        public string ApiKey { get; set; }

        public string DeploymentName { get; set; }
    }
}
