using System;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Data.SQLite;
namespace appgui;
public class Form1 : Form
{
    string sql;
    SQLiteCommand command;
    public Button button;
    bool key_valid = false;
    public Button keybutton;
    public TextBox ipBox;
    public TextBox licenseKeyBox;
    private static string dbCommand = "Data Source=DemoDB.db;Version=3;New=False;Compress=True;";
    private static SQLiteConnection dbConnection = new SQLiteConnection(dbCommand);
    private static SQLiteCommand Command = new SQLiteCommand("", dbConnection);
    public Form1()
    {
        Size = new Size(280, 200);

        Label ip_label = new Label();
        ip_label.Text = "Key";
        ip_label.Location = new Point(18, 20);
        ip_label.AutoSize = true;
        ip_label.Font = new Font("Calibri", 10);
        ip_label.Padding = new Padding(6);
        this.Controls.Add(ip_label);
        ipBox = new TextBox();
        ipBox.Location = new Point(70, 23);
        ipBox.Size = new Size(170, 90);
        this.Controls.Add(ipBox);

        keybutton = new Button();
        keybutton.Size = new Size(60, 20);
        keybutton.Location = new Point(120, 60);
        keybutton.Text = "Check Key";
        this.Controls.Add(keybutton);
        keybutton.Click += new EventHandler(key_check);

        Label license = new Label();
        license.Text = "Data";
        license.Location = new Point(18, 90);
        license.AutoSize = true;
        license.Font = new Font("Calibri", 10);
        license.Padding = new Padding(6);
        this.Controls.Add(license);
        licenseKeyBox = new TextBox();
        licenseKeyBox.Location = new Point(80, 90);
        licenseKeyBox.Size = new Size(170, 90);
        this.Controls.Add(licenseKeyBox);
        button = new Button();
        button.Size = new Size(60, 20);
        button.Location = new Point(120, 120);
        button.Text = "Add Data";
        this.Controls.Add(button);
        button.Click += new EventHandler(license_click);

    }

    private void key_check(object sender, EventArgs e)
    {
        if (ipBox.Text == "")
        {
            MessageBox.Show("Please enter a key");
            return;
        }
        string key = ipBox.Text;
        //MessageBox.Show(ipBox.Text);
        if (ipBox.Text == "1234567890")
        {
            key_valid = true;
            enCrypt();
            MessageBox.Show("Key is valid");
        }
        else
        {
            key_valid = false;
            MessageBox.Show("Invalid Key");
        }
    }
    private void license_click(object sender, EventArgs e)
    {
        if (ipBox.Text == "")
        {
            MessageBox.Show("Please enter a key");
            return;
        }
        else if (key_valid == false)
        {
            MessageBox.Show("Enter a valid key");
            return;
        }
        MessageBox.Show("Starting SQL");
        deCrypt();
        openConnection();
        /*string input = licenseKeyBox.Text;
        string[] values = input.Split(' ');
        string FirstName = values[0];
        string LastName = values[1];
        MessageBox.Show(FirstName + " " + LastName);
        using (var transaction = dbConnection.BeginTransaction())
        {
            var insertCmd = dbConnection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Person VALUES('" + FirstName + "','" + LastName + "')";
            insertCmd.ExecuteNonQuery();
            transaction.Commit();
        }*/
        var selectCmd = dbConnection.CreateCommand();
        var selectCmd1 = dbConnection.CreateCommand();
        selectCmd.CommandText = "SELECT DataType FROM Person";
        selectCmd1.CommandText = "SELECT Data FROM Person";
        string msg1;
        using (var reader = selectCmd.ExecuteReader())
        {
            using (var reader1 = selectCmd1.ExecuteReader())
            {
                while (reader.Read() && reader1.Read())
                {
                    var message = reader.GetString(0);
                    var message1 = reader1.GetString(0);
                    msg1 = message + " " + message1;
                    MessageBox.Show(msg1);
                }
            }
            enCrypt();
        }
        //var reader = selectCmd.ExecuteReader(); 
        //reader.Read();
        closeConnection();
        return;
    }

    private void enCrypt()
    {
        sql = "PRAGMA lic='77523-009-0000007-72328';";
        command = new SQLiteCommand(sql, dbConnection);
        command.ExecuteNonQuery();

        sql = "PRAGMA rekey='abc123';";
        command = new SQLiteCommand(sql, dbConnection);
        command.ExecuteNonQuery();
    }
    private void deCrypt()
    {
        sql = "PRAGMA rekey='abc123';";
        command = new SQLiteCommand(sql, dbConnection);
        command.ExecuteNonQuery();

        sql = "PRAGMA lic='77523-009-0000007-72328';";
        command = new SQLiteCommand(sql, dbConnection);
        command.ExecuteNonQuery();
        //now you have all access to encrypted data.db
        sql = "PRAGMA lic='';";
        command = new SQLiteCommand(sql, dbConnection);
        command.ExecuteNonQuery();
    }
    private void openConnection()
    {
        MessageBox.Show("Opening Connection");
        if (dbConnection.State == System.Data.ConnectionState.Closed)
        {
            dbConnection.Open();
            //MessageBox.Show("Connection opened to:" + dbConnection.State.ToString());
        }
    }
    private void closeConnection()
    {
        if (dbConnection.State == System.Data.ConnectionState.Open)
        {
            dbConnection.Close();
            //MessageBox.Show("Connection closed to:" + dbConnection.State.ToString());
        }
    }
}
static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}