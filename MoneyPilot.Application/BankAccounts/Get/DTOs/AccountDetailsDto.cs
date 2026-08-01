namespace MoneyPilot.Application.BankAccounts.Get.DTOs
{
    public record AccountDetailsDto
    {
        public int Id { get; init; }

        public string HolderName { get; init; }

        public decimal Balance { get; init; }

        public long AccountNumber { get; init; }
    }
}
