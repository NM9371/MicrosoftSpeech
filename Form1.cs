using Microsoft.Speech.Recognition;
using System.Globalization;

namespace MicrosoftSpeech
{
    public partial class Form1 : Form
    {
        static bool completed;

        static Label label;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label = label1;
            recognizing_Speech();
        }

        // Handle the SpeechRecognized event.  
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null && e.Result.Text != null)
            {
                Console.WriteLine("  Recognized text =  {0}", e.Result.Text);
                label.Text = e.Result.Text;
            }
            else
            {
                Console.WriteLine("  Recognized text not available.");
            }
        }

        // Handle the RecognizeCompleted event.  
        static void recognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("  Error encountered, {0}: {1}",
                e.Error.GetType().Name, e.Error.Message);
            }
            if (e.Cancelled)
            {
                Console.WriteLine("  Operation cancelled.");
            }
            if (e.InputStreamEnded)
            {
                Console.WriteLine("  End of stream encountered.");
            }
            completed = true;
        }
        static void recognizing_Speech()
        {
            //CultureInfo culture = new CultureInfo("ru-RU");
            //SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(culture);
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new CultureInfo("ru-RU"));
            using (recognizer)
            {

                Choices words = new Choices();
                words.Add(new string[] { "велосипед", "where loe see pied", "один", "два", "раз", "купить" });
                GrammarBuilder grammarBuilder = new GrammarBuilder();
                grammarBuilder.Append(words);
                Grammar grammar = new Grammar(grammarBuilder);
                grammar.Name = "123";
                recognizer.LoadGrammar(grammar);

                recognizer.SetInputToWaveFile(@"D:\Downloads\src\GoogleSpeech\work_dir\my.wav");


                // Attach event handlers for the results of recognition.  
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
                recognizer.RecognizeCompleted +=
                  new EventHandler<RecognizeCompletedEventArgs>(recognizer_RecognizeCompleted);


                // Perform recognition on the entire file.  
                Console.WriteLine("Starting asynchronous recognition...");
                completed = false;
                recognizer.RecognizeAsync();

                // Keep the console window open.  
                //while (!completed)
                //{
                //    Console.ReadLine();
                //}
                Console.WriteLine("Done.");
            }
        }
    }
}