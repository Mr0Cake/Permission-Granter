using PermissionGranter.Model;
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

namespace PermissionGranter
{
    /// <summary>
    /// Interaction logic for ExtensionMethodsTest.xaml
    /// </summary>
    public partial class ExtensionMethodsTest : UserControl
    {
        public ExtensionMethodsTest()
        {
            InitializeComponent();

            add.Add("test", "test");
            add.Add("pest", "pest");
            add.TryAdd("test", "test");
            add.TryAdd("zoumoetenlukken", "zoumoetenlukken");

            add2.Add("test1", "test1");
            add2.Add("testvalue", "testvalue");
            add.TryAdd("laatsteWaarde", "Laatstewaard");

            add2.Add("test", "test");

            voeg.Add("help");
            voeg.Add("schelp");

            toe.Add("hashshet");
            toe.Add("laatstewaarde");

            voeg.AddRange(toe);

            txtMethod1.Text = "add" + Environment.NewLine;
            foreach (KeyValuePair<string, string> kvp in add)
            {
                txtMethod1.Text += kvp.Key + " " + kvp.Value + Environment.NewLine;
            }

            txtMethod1.Text += "################ add2" + Environment.NewLine;
            foreach (KeyValuePair<string, string> kvp in add2)
            {
                txtMethod1.Text += kvp.Key + " " + kvp.Value + Environment.NewLine;
            }
            txtMethod2.Text += "###########voeg" + Environment.NewLine;
            foreach (string s in voeg)
            {
                txtMethod2.Text += s + Environment.NewLine;
            }
            txtMethod2.Text += "###########toe" + Environment.NewLine;
            foreach (string s in toe)
            {
                txtMethod2.Text += s + Environment.NewLine;
            }
        }

        public Dictionary<string, string> add = new Dictionary<string, string>();
        public Dictionary<string, string> add2 = new Dictionary<string, string>();
        public KeyValuePair<string, string> kvp1 = new KeyValuePair<string, string>("kvp","kvp");
        public HashSet<string> voeg = new HashSet<string>();
        public HashSet<string> toe = new HashSet<string>();

        
    }
}
