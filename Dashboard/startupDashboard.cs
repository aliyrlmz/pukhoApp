using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using pukhoApp.Models.Constants;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using pukhoApp.Models;
using pukhoApp.Helpers;

namespace pukhoApp
{
    public partial class startupDashboard : MetroSet_UI.Forms.MetroSetForm
    {
        public startupDashboard()
        {
            InitializeComponent();
        }

        private void startupDashboard_Load(object sender, EventArgs e)
        {
            this.StyleManager = styleManager1;
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            MetroSet_UI.Forms.MetroSetMessageBox.Show(this, "Bu proje henüz bağış kabul etmemektedir.", "Teşekkürler!");
        }

        private void swLightordark_SwitchedChanged(object sender)
        {
            if (this.styleManager1.Style == MetroSet_UI.Enums.Style.Light)
            {
                ChangeScreenColorToDark();
            }
            else
            {
                ChangeScreenColorToLight();
            }
        }

        private void ChangeScreenColorToDark()
        {
            this.styleManager1.Style = MetroSet_UI.Enums.Style.Dark;
            pukhoappLogo.Visible = false;
            pukhoappLogoDark2.Visible = true;
            pukhoNightIcon.Visible = false;
            pukhoSunIcon.Visible = true;
            this.Refresh();
        }

        private void ChangeScreenColorToLight()
        {
            this.styleManager1.Style = MetroSet_UI.Enums.Style.Light;
            pukhoappLogoDark2.Visible = false;
            pukhoappLogo.Visible = true;
            pukhoNightIcon.Visible = true;
            pukhoSunIcon.Visible = false;
            this.Refresh();

        }
        public int counter = 0;
        private void btnRecent_Click(object sender, EventArgs e)
        {
            try
            {
                if (counter == 0)
                {

                    lblRecent.Text = getRecentUpdates(5);
                    counter += 5;


                }
                else
                {
                    counter += 5;
                    lblRecent.Text = getRecentUpdates(counter);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Büyük Sıkıntı Var /r{0}", ex.Message);
            }
        }
        private string getRecentUpdates(int count)
        {
            pukhoappTestDBEntities db = new Models.pukhoappTestDBEntities();
            string recentupdatelist = String.Empty;
            List<Models.recent> RecentNewsList = db.recent.OrderByDescending(u => u.ID).Take(count).ToList();
            foreach (Models.recent item in RecentNewsList)
            {
                recentupdatelist += String.Format("{0} - {1} - {2} {3}", item.ID, item.RecentUpdate, item.CreateTime, Environment.NewLine);
            }
            return recentupdatelist;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            pukhoappTestDBEntities dbsonuc = new pukhoappTestDBEntities();
            person loginPerson = new person();
            if (txtLoginusername.Text == "" || txtLoginpass.Text == "")
            {
                MessageBox.Show("Boş geçme");
                return;
            }
            try
            {

                loginPerson = dbsonuc.person.FirstOrDefault(r => r.username == txtLoginusername.Text && r.password == txtLoginpass.Text);
                if (loginPerson != null && loginPerson.ID != 0)
                {
                    if (loginPerson.Roles.RoleID == 1)
                    {
                        person loggedPerson = loginPerson;
                        Dashboard.adminDashboard newform = new Dashboard.adminDashboard(loginPerson);
                        newform.Show();
                        this.Hide();
                    }
                    else
                    {
                        person loggedPerson = loginPerson;
                        Dashboard.userDashboard newform = new Dashboard.userDashboard(loginPerson);
                        newform.Show();
                        this.Hide();

                    }

                }
                else
                {
                    MessageBox.Show("Yanlış");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong /r{0}", ex.Message);
            }

        }

        private void cbxRegisterhidepass_CheckedChanged(object sender)
        {
            if (cbxRegisterhidepass.Checked)
            {
                txtRegisterpass.UseSystemPasswordChar = true;
                txtRegisterpassagain.UseSystemPasswordChar = true;
            }
            else
            {
                txtRegisterpass.UseSystemPasswordChar = false;
                txtRegisterpassagain.UseSystemPasswordChar = false;
            }
        }

        private void txtRegistername_Leave(object sender, EventArgs e)
        {
            if (txtRegistername.Text.Length >= 3)
            {
                lblRegisternametip.Text = VariableConstants.Available;
            }
            else
            {
                lblRegisternametip.Text = VariableConstants.NameSurnameTip;
            }
        }

        private void txtRegisterusername_Leave(object sender, EventArgs e)
        {
            if (Models.Constants.Helpers.checkussernameExists(txtRegisterusername.Text))
            {
                lblRegisterusernametip.Text = VariableConstants.usernameAvaible;
            }

            else
            {
                lblRegisterusernametip.Text = VariableConstants.userExists;
            }
            //if (txtRegisterusername.Text.Length >= 3)
            //{
            //    lblRegisterusernametip.Text = VariableConstants.Available;
            //}
            //else
            //{
            //    lblRegisterusernametip.Text = VariableConstants.UserNameTip;
            //}
        }
        private static bool isValid(String str)
        {
            Regex reg = new Regex("^[a-zA-Z]+$");
            return Regex.IsMatch(str, @"^[a-zA-Z]+$");
        }
        private void txtRegisterpass_Leave(object sender, EventArgs e)
        {
            var regexItem = new Regex(@"\W|_");

            if (txtRegisterpass.Text.Length >= 3 && regexItem.IsMatch(txtRegisterpass.Text))
            {
                lblRegisterpasstip.Text = VariableConstants.Available;
            }
            else
            {
                lblRegisterpasstip.Text = VariableConstants.PasswordTip;
            }
        }

        private void txtRegisterpassagain_Leave(object sender, EventArgs e)
        {

            if (txtRegisterpassagain.Text == txtRegisterpass.Text)
            {
                lblRegisterpassagaintip.Text = VariableConstants.Available;
            }
            else
            {
                lblRegisterpassagaintip.Text = VariableConstants.PasswordAgainTip;
            }
        }

        private void txtRegistermail_Leave(object sender, EventArgs e)
        {
           
            if (GeneralHelper.EmailValidator(txtRegistermail.Text))
            {
                lblRegistermailtip.Text = VariableConstants.Available;
            }
            else
            {
                lblRegistermailtip.Text = VariableConstants.MailTip;
            }
        }

        private void btnRegistercheckall_Click(object sender, EventArgs e)
        {
            //pukhoTabcontrol.TabPages.Remove(tabLogin);
            //pukhoTabcontrol.TabPages.Remove(tabRegister);
            //pukhoTabcontrol.TabPages.Remove(tabAbout);
            //pukhoTabcontrol.TabPages.Remove(tabRecent);
        }

        private void btnRegistergeneratepass_Click(object sender, EventArgs e)
        {
            string allowedChars = "";

            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";

            allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";

            char[] sep = { ',' };

            string[] arr = allowedChars.Split(sep);

            string passwordString = "";

            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < Convert.ToInt32(18); i++)

            {

                temp = arr[rand.Next(0, arr.Length)];

                passwordString += temp;

            }

            txtRegisterpass.Text = passwordString;
            txtRegisterpassagain.Text = passwordString;
            string sifre = txtRegisterpass.Text;
            Clipboard.SetText(sifre);
            MessageBox.Show(" Şifreniz oluşturuldu ve kopyalandı.Şifreniz : " + sifre);

        }


        private void database_kayit()
        {
            string labelAd = "";
            string labelSoyad = "";
            string[] dizi;
            dizi = txtRegistername.Text.Split(' ');
            for (int i = 0; i < dizi.Length; i++)
            {
                if (i == dizi.Length - 1)
                {
                    labelSoyad += dizi[i];
                }
                else
                {

                    labelAd += dizi[i] + " ";
                }
            }
            if (labelAd[labelAd.Length-1] == ' ')
            {
                labelAd = labelAd.Remove(labelAd.Length-1);
            }
            string message = "";

            pukhoappTestDBEntities db = new pukhoappTestDBEntities();
            person newUser = new person();
            person newUserKontrol = new person();
            // 
            newUser.first_name = labelAd;
            newUser.last_name = labelSoyad.Trim();
            newUser.username = txtRegisterusername.Text.Trim();
            newUser.password = txtRegisterpass.Text.Trim();
            newUser.email = txtRegistermail.Text.Trim();
            newUserKontrol = db.person.FirstOrDefault(r => r.username == txtRegisterusername.Text);
            if (newUserKontrol != null)
            {
                MessageBox.Show("Kullanıcı adı mevcut");
                return;
            }
            if ((lblRegisterusernametip.Text == VariableConstants.usernameAvaible) && lblRegisternametip.Text == VariableConstants.Available
                && lblRegistermailtip.Text == VariableConstants.Available
                && lblRegisterpasstip.Text == VariableConstants.Available && lblRegisterpassagaintip.Text == VariableConstants.Available)
            {

                db.person.Add(newUser);
                if (db.SaveChanges() > 0 /*&& newUserKontrol == null*/)
                {

                    message = newUser.first_name + " " + newUser.last_name + " kaydın başarıyla oluşturuldu";
                    MessageBox.Show("Succesfull");
                    txtLoginusername.Text = txtRegisterusername.Text;
                    txtLoginpass.Text = txtRegisterpass.Text;
                    pukhoTabcontrol.SelectTab(tabLogin);

                }
                else
                {
                    MessageBox.Show("Not successful");

                }
            }
            else
            {

            }


        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            database_kayit();
        }

        private void cbxLoginhidepass_CheckedChanged(object sender)
        {
            if (cbxLoginhidepass.Checked)
            {
                txtLoginpass.UseSystemPasswordChar = true;
            }
            else
            {
                txtLoginpass.UseSystemPasswordChar = false;
            }
        }

        private void txtLoginusername_Leave(object sender, EventArgs e)
        {
            if (Models.Constants.Helpers.checkussernameExists(txtLoginusername.Text))
            {
                lblUsernametip.Text = VariableConstants.usernotExists;
            }
            else
            {
                lblUsernametip.Text = VariableConstants.userExists;

            }

        }
    }
}

