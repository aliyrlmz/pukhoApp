using Newtonsoft.Json;
using pukhoApp.Models;
using pukhoApp.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using pukhoApp.Helpers;

namespace pukhoApp.Dashboard
{
    public partial class userDashboard : MetroSet_UI.Forms.MetroSetForm
    {
        private person loginPerson = new person();
        pukhoappTestDBEntities db = new pukhoappTestDBEntities();
        public userDashboard(person loginPerson)
        {
            this.loginPerson = loginPerson;
            InitializeComponent();
            CountryApi(loginPerson);
            FillFormElements(loginPerson);
            AliApiTest();
            listviewproducts();
            listviewshoppingcart();
        }
        private void FillFormElements(person loginPerson)
        {

            if (loginPerson != null && loginPerson.ID != 0)
            {
                txtUsername.Text = string.Format("{0} {1}", loginPerson.first_name.Trim(), loginPerson.last_name.Trim());
                txtUserusername.Text = loginPerson.username;
                txtUserpass.Text = loginPerson.password;
                txtUsermail.Text = loginPerson.email;
                //burada sıkıntı var. cmbCountry default selected item = loginperson.country olması lazım.
                txtUserweb.Text = loginPerson.website;
                txtUserphone.Text = loginPerson.phone;
                txtUserskype.Text = loginPerson.skype;
                txtUsermeslek.Text = loginPerson.meslek;
                lblWelcome.Text = "Hoşgeldiniz " + loginPerson.first_name + ' ' +loginPerson.last_name;
                cmbCountry.SelectedItem = loginPerson.country;
            }
            else
            {
                this.Close();
                MessageBox.Show("Bir hata var, giriş yapmadınız");
            }
        }

        private async void AliApiTest()
        {
            string URL = "https://finans.truncgil.com/today.json";
            string urlParameters = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {

                var stream = await response.Content.ReadAsStreamAsync();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var jsonSerializer = new JsonSerializer();
                        var data = jsonSerializer.Deserialize<Root>(jsonTextReader);
                        //var data = jsonSerializer.Deserialize<List<dynamic>>(jsonTextReader);
                        //var bindingSource1 = new BindingSource();
                        //bindingSource1.DataSource = data;
                        //comboBox1.DataSource = bindingSource1.DataSource;
                        //comboBox1.DisplayMember = "Name";
                        lblEursatis.Text  = data.EUR.Satış;
                        lblEuralis.Text  = data.EUR.Alış;
                        lblUsdsatis.Text = data.USD.Satış;
                        lblUsdalis.Text = data.USD.Alış;
                        lblGramaltinalis.Text = data.gramAltin.Alış;
                        lblGramaltinsatis.Text = data.gramAltin.Satış;

                        //bi kaçıyorum. bunu sonraya bırakayım. sen yaz kanka neler yapmam gerekiyorsa görev şeysine
                        //<3<3<3


                    }
                }
            }
        }

        public class Root
        {
            public EUR EUR { get; set; }
            public USD USD { get; set; }
            public string name { get; set; }

            [JsonProperty(PropertyName = "gram-altin")]
            public gramAltin gramAltin { get; set; }

        }
        public class EUR
        {
            public string Alış { get; set; }
            public string Satış { get; set; }
            public string Değişim { get; set; }
        }
        public class USD
        {
            public string Alış { get; set; }
            public string Satış { get; set; }
            public string Değişim { get; set; }
        }

        public class gramAltin
        {
            public string Alış { get; set; }
            public string Satış { get; set; }
            public string Değişim { get; set; }
        }
        private void btnUserupdate_Click(object sender, EventArgs e)
        {
            string labelAd = "";
            string labelSoyad = "";
            string[] dizi;
            dizi = txtUsername.Text.Split(' ');
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
            if (labelAd[labelAd.Length - 1] == ' ')
            {
                labelAd = labelAd.Remove(labelAd.Length - 1);
            }
            pukhoappTestDBEntities db = new pukhoappTestDBEntities();
            person kayitPerson = db.person.FirstOrDefault(r => r.ID == loginPerson.ID);
            //db açıp giriş yaptığımız loginperson idisi ile aynı bi sonuç buluyorum

            if (kayitPerson == null)
            //boşsa return
            {
                MessageBox.Show("Giriş yapılan ID Bulunamadı");
                return;
            }
            if (lblUsernametip.Text == VariableConstants.Available
                && lblUserpasstip.Text == VariableConstants.Available
                && lblUsermailtip.Text == VariableConstants.Available
                && lblUserphonetip.Text == VariableConstants.Available
                && (LblUserusernametip.Text == VariableConstants.usernameAvaible
                || LblUserusernametip.Text == VariableConstants.Available)
               )
            // tipler kullanılabilir ise (sağda test butonu ile aktif ediyorum simdilik)
            {
                kayitPerson.first_name = labelAd;
                kayitPerson.last_name = labelSoyad.Trim();
                kayitPerson.password = txtUserpass.Text.Trim();
                kayitPerson.email = txtUsermail.Text.Trim();
                kayitPerson.country = cmbCountry.Text.Trim();//kayıt yapıyo 
                kayitPerson.email = txtUsermail.Text.Trim();
                kayitPerson.phone = txtUserphone.Text.Trim();
                kayitPerson.skype = txtUserskype.Text.Trim();
                kayitPerson.website = txtUserweb.Text.Trim();
                kayitPerson.meslek = txtUsermeslek.Text.Trim();
                //kaydediyoruz
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show("Succesfull");

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

        private void txtUserphone_Leave(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtUserphone.Text, "^[0-9]+$") && txtUserphone.Text.Length == 11)
            {
                lblUserphonetip.Text = VariableConstants.Available;
            }
            else
            {
                lblUserphonetip.Text = VariableConstants.PhoneTip;
            }

        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length >= 3)
            {
                lblUsernametip.Text = VariableConstants.Available;
            }

        }

        private void txtUserpass_Leave(object sender, EventArgs e)
        {
            var regexItem = new Regex(@"\W|_");

            if (txtUserpass.Text.Length >= 6  && regexItem.IsMatch(txtUserpass.Text))
            {
                lblUserpasstip.Text = VariableConstants.Available;
            }
            else
            {
                lblUserpasstip.Text = VariableConstants.PasswordTip;
            }
        }

        private void txtUsermail_Leave(object sender, EventArgs e)
        {
            
            if (GeneralHelper.EmailValidator(txtUsermail.Text))
            {
                lblUsermailtip.Text = VariableConstants.Available;
            }
            else
            {
                lblUsermailtip.Text = VariableConstants.MailTip;
            }
        }
        //kanka direk textbox silcez saten.... comb
        private void btnDovizhesapla_Click(object sender, EventArgs e)
        {
            if (Regex.IsMatch(txtDovizhesapla.Text, "^[0-9]*$"))
            {
                float euralis;
                float eursatis;
                euralis = float.Parse(lblEuralis.Text);
                eursatis = float.Parse(lblEursatis.Text);
                float tlalis;
                tlalis= float.Parse(txtDovizhesapla.Text);
                float tlsatis;
                tlsatis= float.Parse(txtDovizhesapla.Text);
                float usdalis;
                float usdsatis;
                usdalis = float.Parse(lblUsdalis.Text);
                usdsatis = float.Parse(lblUsdsatis.Text);
                float gramalis;
                float gramsatis;
                ////////
                gramalis = float.Parse(lblGramaltinalis.Text);
                gramsatis = float.Parse(lblGramaltinsatis.Text);

                lblEurtlalis.Text = (tlalis/euralis).ToString();
                lblEurtlsatis.Text = (tlsatis*eursatis).ToString();
                lblUsdtlalis.Text  = (tlalis/usdalis).ToString();
                lblUsdtlsatis.Text  = (tlsatis*usdsatis).ToString();
                lblGramaltintlalis.Text  = (tlalis/gramalis).ToString();
                lblGramaltintlsatis.Text  = (tlsatis*gramsatis).ToString();
            }
            else
            {
                txtDovizhesapla.Text = "Lütfen Bu alana sayısal değer giriniz";
            }

        }

        private async void CountryApi(person loginPerson)
        {
            string URL = "https://restcountries.com/v2/";
            string urlParameters = "all?fields=name";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {

                var stream = await response.Content.ReadAsStreamAsync();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var jsonSerializer = new JsonSerializer();
                        //var data = jsonSerializer.Deserialize<Root>(jsonTextReader);
                        var data = jsonSerializer.Deserialize<List<dynamic>>(jsonTextReader);
                        var bindingSource1 = new BindingSource();
                        bindingSource1.DataSource = data;
                        cmbCountry.DataSource = bindingSource1.DataSource;
                        cmbCountry.DisplayMember = "Name";
                        cmbCountry.ValueMember = "Name";
                    }
                }
            }
        }

        private void txtUserusername_Leave(object sender, EventArgs e)
        {
            if (Models.Constants.Helpers.checkussernameExists(txtUserusername.Text))
            {
                LblUserusernametip.Text = VariableConstants.usernameAvaible;
            }

            else
            {
                if (loginPerson.username == txtUserusername.Text)
                {
                    LblUserusernametip.Text = VariableConstants.usernameAvaible;
                }
                else
                {
                    LblUserusernametip.Text = VariableConstants.userExists;
                }
            }

        }
        private void listviewproducts()
        {
            var itemlist = (from p in db.Products select p).ToList();
            foreach (var item in itemlist)
            {
                ListViewItem lvi = new ListViewItem();
                lbProducts.Items.Add(item.Name);
            }
        }
        public void listviewshoppingcart()
        {
            var itemlist = (from p in db.Carts select p).ToList();
            foreach (var item in itemlist)
            {
                if (item.UserID == loginPerson.ID)
                {

                    lbCart.Items.Add(item.Products.Name);
                }
            }
        }

        private void lbProducts_SelectedIndexChanged(object sender)
        {
            if (Application.OpenForms.OfType<adminProductdetails>().Count() == 0)
            {
                string selectedproduct = lbProducts.SelectedItem.ToString();
                adminProductdetails newform = new adminProductdetails(selectedproduct, loginPerson);
                newform.Show();
            }

            else
            {
                List<Form> activeforms = new List<Form>();
                foreach (Form f in Application.OpenForms)
                    if (f.Name == "adminProductdetails")
                        activeforms.Add(f);
                foreach (Form f in activeforms)
                    f.Close();
                string selectedproduct = lbProducts.SelectedItem.ToString();
                adminProductdetails newform = new adminProductdetails(selectedproduct, loginPerson);
                newform.Show();
            }
        }

        private void lbCart_SelectedIndexChanged(object sender)
        {
            if (Application.OpenForms.OfType<adminProductdetails>().Count() == 0)
            {
                string selectedproduct = lbCart.SelectedItem.ToString();
                adminProductdetails newform = new adminProductdetails(selectedproduct, loginPerson);
                newform.Show();
            }

            else
            {
                List<Form> activeforms = new List<Form>();
                foreach (Form f in Application.OpenForms)
                    if (f.Name == "adminProductdetails")
                        activeforms.Add(f);
                foreach (Form f in activeforms)
                    f.Close();
                string selectedproduct = lbCart.SelectedItem.ToString();
                adminProductdetails newform = new adminProductdetails(selectedproduct, loginPerson);
                newform.Show();
            }

        }
    }
}
