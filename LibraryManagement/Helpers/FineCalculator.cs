namespace LibraryManagement.Helpers
{
    public class FineCalculator
    {
        public const int FinePerDay = 50; // apni marzi ka rate rakh lo

        public static int Calculate(DateTime dueDate, string status, int savedFineAmount)
        {
            // Agar book already return ho chuki hai, to jo fine save hui thi wahi final hai
            if (status == "Returned")
                return savedFineAmount;

            // Abhi tak issued hai — aaj ki date ke hisab se live fine nikaalo
            int daysLate = (DateTime.Now.Date - dueDate.Date).Days;

            return daysLate > 0 ? daysLate * FinePerDay : 0;
        }
    }
}
