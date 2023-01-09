using pukhoApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pukhoApp.Dashboard
{
    public partial class adminDashboard : MetroSet_UI.Forms.MetroSetForm
    {
        public adminDashboard(person loginPerson)
        {
            LoginPerson=loginPerson;
            InitializeComponent();
            personBindingSource.DataSource = db.person.ToList();
            categoriesBindingSource.DataSource = db.Categories.ToList();
            listview();
            lblPerson.Text = loginPerson.username;
        }
        pukhoappTestDBEntities db = new pukhoappTestDBEntities();
        public person LoginPerson { get; set; }

        private void btnAdminupdate_Click(object sender, EventArgs e)
        {
            person kayitPerson = db.person.FirstOrDefault(r => r.username == cmbadminUsers.Text.ToString());
            if (rbRoleuser.Checked)
            {
                kayitPerson.roleid = 2;
                string labelAd = "";
                string labelSoyad = "";
                string[] dizi;
                dizi = txtAdminusername.Text.Split(' ');
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

                kayitPerson.first_name = labelAd;
                kayitPerson.last_name = labelSoyad.Trim();
                kayitPerson.username = txtAdminuserusername.Text.Trim();
                kayitPerson.password = txtAdminuserpass.Text.Trim();
                kayitPerson.email = txtAdminusermail.Text.Trim();
                kayitPerson.phone = txtAdminuserphone.Text.Trim();
                kayitPerson.skype = txtAdminuserskype.Text.Trim();
                kayitPerson.website = txtAdminuserwebsite.Text.Trim();
                kayitPerson.meslek = txtAdminusermeslek.Text.Trim();
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
            if (rbRoleadmin.Checked)
            {
                kayitPerson.roleid = 1;
                string labelAd = "";
                string labelSoyad = "";
                string[] dizi;
                dizi = txtAdminusername.Text.Split(' ');
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

                kayitPerson.first_name = labelAd;
                kayitPerson.last_name = labelSoyad.Trim();
                kayitPerson.username = txtAdminuserusername.Text.Trim();
                kayitPerson.password = txtAdminuserpass.Text.Trim();
                kayitPerson.email = txtAdminusermail.Text.Trim();
                kayitPerson.phone = txtAdminuserphone.Text.Trim();
                kayitPerson.skype = txtAdminuserskype.Text.Trim();
                kayitPerson.website = txtAdminuserwebsite.Text.Trim();
                kayitPerson.meslek = txtAdminusermeslek.Text.Trim();
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
        private void listview()
        {
            //yok o listviewdi bu 
            var itemlist = (from p in db.Products select p).ToList();
            foreach (var item in itemlist)
            {
                ListViewItem lvi = new ListViewItem();
                //lvi.Name= itemlist.Name;
                //And so on...
                lstAdminproducts.Items.Add(item.Name);
                //detaili nasıl açıcaz ? new form (item details oky )
            }
        }
        private void cmbadminUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            person kayitPerson = db.person.FirstOrDefault(r => r.username == cmbadminUsers.Text.ToString());
            if ( kayitPerson != null )
            { 
            txtAdminusername.Text = kayitPerson.first_name + " " + kayitPerson.last_name;
            txtAdminuserusername.Text = kayitPerson.username;
            txtAdminuserpass.Text = kayitPerson.password;
            txtAdminusermail.Text = kayitPerson.email;
            txtAdminuserwebsite.Text = kayitPerson.website;
            txtAdminuserphone.Text = kayitPerson.phone;
            txtAdminuserskype.Text = kayitPerson.skype;
            txtAdminusermeslek.Text = kayitPerson.meslek;
            if (kayitPerson.roleid == 1)
            {
                rbRoleadmin.Checked = true;
                rbRoleuser.Checked = false;
            }
            else
            {
                rbRoleadmin.Checked = false;
                rbRoleuser.Checked = true;
            }
            }
            else
            {

            }
        }

        private void btnAdminaddcategory_Click(object sender, EventArgs e)
        {
            pukhoappTestDBEntities db = new pukhoappTestDBEntities();
            Categories newcat = new Categories();
            newcat.Catname = txtAdminaddcategoryname.Text;
            newcat.Catdesc = txtAdminaddcategoryname.Text;
            db.Categories.Add(newcat);
            if (db.SaveChanges() > 0 /*&& newUserKontrol == null*/)
            {

                MessageBox.Show("Added Category");
                lstAdminproducts.Clear();
                listview();

            }
            else
            {
                MessageBox.Show("Not successful");
            }
        }

        private void btnAdminproductdetails_Click(object sender, EventArgs e)
        {
            pukhoappTestDBEntities db = new pukhoappTestDBEntities();
            Products newproduct = new Products();
            newproduct.Name = txtAdminaddproductname.Text;
            newproduct.Procatid = 1;
            newproduct.Createdate = DateTime.Now;
            newproduct.Createdby = LoginPerson.ID;
            db.Products.Add(newproduct);
            if (db.SaveChanges() > 0 /*&& newUserKontrol == null*/)
            {

                MessageBox.Show("Added Product");
                lstAdminproducts.Clear();
                listview();
                //seçtiremedik
                lstAdminproducts.SelectedValue = txtAdminaddproductname.Text;

            }
            else
            {
                MessageBox.Show("Not successful");
            }
        }

        private void lstAdminproducts_SelectedIndexChanged(object sender)
        {
            if ( Application.OpenForms.OfType<adminProductdetails>().Count() == 0)
            {
                string selectedproduct = lstAdminproducts.SelectedItem.ToString();
                adminProductdetails newform = new adminProductdetails(selectedproduct,LoginPerson);
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
                string selectedproduct = lstAdminproducts.SelectedItem.ToString();
                person loginPerson = LoginPerson;
                adminProductdetails newform = new adminProductdetails(selectedproduct, LoginPerson);
                newform.Show();
            }
        }

        public static implicit operator adminDashboard(adminProductdetails v)
        {
            throw new NotImplementedException();
        }

        private void btnDeleteproducts_Click(object sender, EventArgs e)
        {
            var deletelist = db.Products.Where(c => c.Name==lstAdminproducts.SelectedItem);
            db.Products.RemoveRange(deletelist);
            db.SaveChanges();
            lstAdminproducts.Clear();
            listview();
            List<Form> activeforms = new List<Form>();
            foreach (Form f in Application.OpenForms)
                if (f.Name == "adminProductdetails")
                    activeforms.Add(f);
            foreach (Form f in activeforms)
                f.Close();
        }
    }
}
