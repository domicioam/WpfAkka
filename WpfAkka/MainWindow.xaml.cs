using Akka.Actor;
using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAkka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IActorRef actor;

        public MainWindow()
        {
            InitializeComponent();

            // make the actor run in the UI thread to demonstrate the issue
            var config = ConfigurationFactory.ParseString(
                @"
                    akka {
                      actor {
                        deployment {
                          /asyncDemoActor {
                            dispatcher = akka.actor.synchronized-dispatcher
                          }
                        }
                      }
                    }
                ");
            
            var actorSystem = ActorSystem.Create("ActorSystem", config);
            actor = actorSystem.ActorOf(Props.Create(() => new AsyncDemoActor(textBlock)), "asyncDemoActor");

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            actor.Tell(new AsyncDemoActor.WithGetAwaiter());
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            actor.Tell(new AsyncDemoActor.WithPipeTo());
        }
    }
}
