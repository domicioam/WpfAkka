using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAkka
{
    public class AsyncDemoActor : ReceiveActor
    {
        #region Messages

        public class WithGetAwaiter { }
        public class WithPipeTo { }
        public class HelloWorld { }

        #endregion

        int counter = 1;

        public AsyncDemoActor(TextBlock textBlock)
        {
            Receive<WithGetAwaiter>(msg =>
            {
                counter = 1;

                Context.System.Scheduler.ScheduleTellRepeatedly(
                    TimeSpan.FromMilliseconds(250),
                    TimeSpan.FromMilliseconds(250),
                    Self,
                    new HelloWorld(),
                    Self);

                Task.Delay(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
            });

            Receive<WithPipeTo>(msg =>
            {
                counter = 1;

                Context.System.Scheduler.ScheduleTellRepeatedly(
                    TimeSpan.FromMilliseconds(250),
                    TimeSpan.FromMilliseconds(250),
                    Self,
                    new HelloWorld(),
                    Self);

                Task.Delay(TimeSpan.FromSeconds(5)).PipeTo(Self);
            });

            Receive<HelloWorld>(msg =>
            {
                textBlock.Text = $"Hello World {counter}";
                counter++;
            });
        }
    }
}
