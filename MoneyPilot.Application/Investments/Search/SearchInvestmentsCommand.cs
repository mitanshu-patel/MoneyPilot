using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyPilot.Application.Investments.Search
{
    public record SearchInvestmentsCommand(Guid UserOId); // This can be extended with additional search parameters in the future, such as filters for date range, category, etc.
}
