/* Title:           Main Menu
 * Date:            5-14-16
 * Author:          Terry Holmes
 *
 * Description:     This is the main menu */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagesDLL;

namespace NewInventoryProject
{
    public partial class MainMenu : Form
    {
        //setting up the classes
        MessagesClass TheMessagesClass = new MessagesClass();

        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this will close the application
            TheMessagesClass.CloseTheProgram();
        }

        private void btnPerformValuation_Click(object sender, EventArgs e)
        {
            //this will show the valuation
            PerformValuation PerformValuation = new PerformValuation();
            PerformValuation.Show();
            this.Close();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox AboutBox = new AboutBox();
            AboutBox.ShowDialog();
        }

        private void btnCheckPrice_Click(object sender, EventArgs e)
        {
            //this will display the check price
            CheckPrice CheckPrice = new CheckPrice();
            CheckPrice.Show();
            this.Close();
        }

        private void btnPartsWithoutPartNumbers_Click(object sender, EventArgs e)
        {
            TheMessagesClass.UnderDevelopment();
        }

        private void btnTableMissingPartNumbers_Click(object sender, EventArgs e)
        {
            TheMessagesClass.UnderDevelopment();
        }
    }
}
