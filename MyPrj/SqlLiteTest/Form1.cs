using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace SqlLiteTest
{
    public partial class Form1 : Form
    {

        //数据库连接
        SQLiteConnection m_dbConnection;

        public Form1()
        {
            InitializeComponent();

            createNewDatabase();
            connectToDatabase();
            createTable();
            fillTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printHighscores();
        }

        void createNewDatabase()
        {

            if (!File.Exists("MyDatabase.db"))
            {
                SQLiteConnection.CreateFile("MyDatabase.db");
            }
        }

        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }

        //在指定数据库中创建一个table
        void createTable()
        {
            try
            {
                string sql = "create table highscores (name varchar(20), score int)";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = ex.Message;
            }

        }


        //插入一些数据
        void fillTable()
        {
            string sql = "insert into highscores (name, score) values ('Me', 3000)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into highscores (name, score) values ('Myself', 6000)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into highscores (name, score) values ('And I', 9001)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        //使用sql查询语句，并显示结果
        void printHighscores()
        {
            StringBuilder sb1 = new StringBuilder();
            string sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            { sb1.Append("Name: " + reader["name"] + "\tScore: " + reader["score"]); }

            this.richTextBox1.Text = sb1.ToString();
        }

    }
}
