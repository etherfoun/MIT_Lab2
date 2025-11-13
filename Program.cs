public class Program
{

    // Test Email Sender and Receiver
    //static void Main(string[] args)
    //{
    //    //EmailSender.SendEmail("Lab2", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus sodales massa ipsum, " +
    //    //                              "id interdum libero dapibus sit amet. Praesent pellentesque eget urna ac interdum. Etiam vulputate consectetur mi, " +
    //    //                              "non ultricies mi scelerisque vitae. Aliquam eleifend nisi ut orci elementum, lobortis ullamcorper nisi pulvinar. " +
    //    //                              "Quisque molestie pharetra vehicula. Sed ut arcu auctor, egestas.");

    //    Console.WriteLine("--- Testing Email Receiver ---");

    //    EmailReceiver.ReceiveWithImap();

    //    EmailReceiver.ReceiveWithPop3();

    //    Console.WriteLine("--- Test finished ---");
    //    Console.ReadLine();
    //}

    // -------------------------------------------------------------------------------------------- //

    // Test Speed of Synchronous vs Asynchronous Email Sending
    public static async Task Main(string[] args)
    {
        Console.WriteLine("--- Running Speed Test ---");

        await SpeedTest.RunSpeedTest();

        Console.WriteLine("--- Test finished ---");
        Console.ReadLine();
    }
}