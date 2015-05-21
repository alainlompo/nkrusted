using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excelta.NKrusted.Core.Generators;

namespace EWDal.UserForms
{
    public partial class BasicGenerator : Form
    {
        public BasicGenerator()
        {
            InitializeComponent();
        }

        private void cbTrustedConnection_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTrustedConnection.Checked)
            {
                txtUserID.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtUserID.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();
            if (fBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = fBD.SelectedPath;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            DatabaseMapper mapper = new DatabaseMapper();
            string securityPart = (cbTrustedConnection.Checked)? "integrated security = true":"user id ="+txtUserID.Text + "; password="+txtPassword.Text;
            string connectionString = "Server="+txtServer.Text + "; initial catalog ="+ txtDatabase.Text+ ";" + securityPart;

            mapper.GenerateClassFiles(txtPath.Text, connectionString, txtDatabase.Text);
 
            

        }
    }
}
