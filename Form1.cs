using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace findhash
{
    public partial class Form1 : Form
    {
        // Create labels and textboxes for the second and third columns
        private Label[] labelsColumn1 = new Label[3];
        private TextBox[] textboxesColumn2 = new TextBox[3];
        private Label[] labelsColumn3 = new Label[3];

        Size size = new(100, 30);

        // Create the encrypt button
        private Button encryptButton;

        // Create the FlowLayoutPanel for displaying submissions
        private FlowLayoutPanel submissionPanel;

        private ComboBox decryptComboBox; // Dropdown list for selecting submissions to decrypt

        private Label decryptedTextLabel; // Label to display decrypted text

        private Aes aesAlg; // AES encryption algorithm

        public Form1()
        {
            InitializeComponent();

            // Add headings for the first, second, and third columns
            Label userHeadingLabel = new Label();
            userHeadingLabel.Text = "USER:";
            userHeadingLabel.Location = new System.Drawing.Point(20, 0);

            Label dataHeadingLabel = new Label();
            dataHeadingLabel.Text = "DATA:";
            dataHeadingLabel.Location = new System.Drawing.Point(200, 0);

            Label sha256HeadingLabel = new Label();
            sha256HeadingLabel.Text = "SHA256:";
            sha256HeadingLabel.Location = new System.Drawing.Point(400, 0);

            // Initialize the labels and textboxes for the second column
            for (int i = 0; i < 3; i++)
            {
                labelsColumn1[i] = new Label();
                if (i == 0)
                {
                    labelsColumn1[i].Text = "Name";
                }
                if (i == 1)
                {
                    labelsColumn1[i].Text = "Email";
                }
                if (i == 2)
                {
                    labelsColumn1[i].Text = "Number";
                }
                labelsColumn1[i].Location = new System.Drawing.Point(20, 20 + (i * 30));
                labelsColumn1[i].Width = 150; // Adjusted width

                textboxesColumn2[i] = new TextBox();
                textboxesColumn2[i].Location = new System.Drawing.Point(200, 20 + (i * 30)); // Adjusted location
                textboxesColumn2[i].Size = new System.Drawing.Size(200, 20); // Set the width to 200 (adjust as needed)
                textboxesColumn2[i].LostFocus += new EventHandler(TextBox_LostFocus);
                textboxesColumn2[i].Width = 180; // Adjusted width

                labelsColumn3[i] = new Label();
                labelsColumn3[i].Location = new System.Drawing.Point(400, 20 + (i * 30)); // Adjusted location
                labelsColumn3[i].Width = 600; // Adjusted width
            }

            // Initialize the encrypt button

            encryptButton = new Button();
            encryptButton.Text = "ENCRYPT";
            encryptButton.Location = new System.Drawing.Point(20, 110);
            encryptButton.Click += new EventHandler(EncryptButton_Click);
            encryptButton.Size = size;

            // Initialize the FlowLayoutPanel for submissions
            submissionPanel = new FlowLayoutPanel();
            submissionPanel.FlowDirection = FlowDirection.TopDown;
            submissionPanel.Location = new System.Drawing.Point(20, 180); // Adjusted location
            submissionPanel.AutoSize = true;

            // Add controls to the form
            Controls.Add(userHeadingLabel);
            Controls.Add(dataHeadingLabel);
            Controls.Add(sha256HeadingLabel);

            for (int i = 0; i < 3; i++)
            {
                Controls.Add(labelsColumn1[i]);
                Controls.Add(textboxesColumn2[i]);
                Controls.Add(labelsColumn3[i]);
            }
            Controls.Add(encryptButton);
            Controls.Add(submissionPanel);

            // Initialize the decrypt button
            Button decryptButton = new Button();
            decryptButton.Text = "Decrypt";
            decryptButton.Location = new System.Drawing.Point(200, 110);
            decryptButton.Click += new EventHandler(DecryptButton_Click);
            decryptButton.Size = size;

            // Initialize the dropdown list for selecting submissions to decrypt
            decryptComboBox = new ComboBox();
            decryptComboBox.Location = new System.Drawing.Point(320, 110);
            decryptComboBox.DropDownStyle = ComboBoxStyle.DropDownList; // Make it a dropdown list
            decryptComboBox.Width = 60; // Adjusted width

            // Initialize the label "AES256" to display next to the dropdown
            Label aesLabel = new Label();
            aesLabel.Text = "AES256:";
            aesLabel.Location = new System.Drawing.Point(400, 110); // Adjusted location

            // Initialize the label to display decrypted text
            decryptedTextLabel = new Label();
            decryptedTextLabel.Location = new System.Drawing.Point(500, 110); // Adjusted location
            decryptedTextLabel.AutoSize = true;

            // Add the "Decrypt" button and the dropdown list to the form
            Controls.Add(decryptButton);
            Controls.Add(decryptComboBox);
            Controls.Add(aesLabel);
            Controls.Add(decryptedTextLabel);

            // Initialize AES encryption algorithm
            aesAlg = Aes.Create();
            aesAlg.GenerateKey();
            aesAlg.GenerateIV();
        }

        private int encryptCounter = 0; // Counter for tracking submissions
        private List<string> encryptedSubmissions = new List<string>(); // List to store encrypted submissions


        private void EncryptButton_Click(object sender, EventArgs e)
        {
            // Concatenate text from the second column
            string concatenatedText = "";
            for (int i = 0; i < 3; i++)
            {
                concatenatedText += textboxesColumn2[i].Text;
            }

            // Increment the submission counter
            encryptCounter++;

            // Create a label to display the concatenated text
            Label submissionLabel = new Label();
            var EncryptedText = EncryptAES256Hash(concatenatedText);
            submissionLabel.Text = encryptCounter + ". " + EncryptedText;
            submissionLabel.AutoSize = true;

            // Add the label to the FlowLayoutPanel for submissions
            submissionPanel.Controls.Add(submissionLabel);

            // Store the encrypted text in the list
            encryptedSubmissions.Add(EncryptedText);

            // Update the dropdown list with submission numbers
            decryptComboBox.Items.Add(encryptCounter);
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Find the index of the textbox in the second column
            int index = Array.IndexOf(textboxesColumn2, textBox);

            // Get the text from the second column
            string inputText = textBox.Text;

            // Encrypt the text using SHA-256
            string hashedText = GetSHA256Hash(inputText);

            // Update the label in the third column with the hashed text
            labelsColumn3[index].Text = hashedText;
        }

        private static string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Convert to hexadecimal
                }

                return builder.ToString();
            }
        }

        private string EncryptAES256Hash(string input)
        {
            try
            {
                // Convert the input text to bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Create an encryptor
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV); // Using the IV from aesAlg

                // Encrypt the bytes
                byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

                // Convert the encrypted bytes to a Base64-encoded string
                string encryptedText = Convert.ToBase64String(encryptedBytes);

                return encryptedText;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encryption failed: " + ex.Message);
                return "Encryption Error";
            }
        }

        private string DecryptAES256Hash(string hashedText)
        {
            try
            {
                // Convert the hashed text to bytes
                byte[] encryptedBytes = Convert.FromBase64String(hashedText);

                // Create a decryptor
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV); // Using the IV from aesAlg

                // Decrypt the bytes
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                // Convert the decrypted bytes to a string
                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                return decryptedText;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Decryption failed: " + ex.Message);
                return "Decryption Error";
            }
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            int selectedSubmissionIndex = decryptComboBox.SelectedIndex;
            if (selectedSubmissionIndex >= 0 && selectedSubmissionIndex < encryptedSubmissions.Count)
            {
                // Retrieve the encrypted text based on the selected submission
                string hashedText = encryptedSubmissions[selectedSubmissionIndex];

                // Perform decryption here, and update the label with the decrypted text
                string decryptedText = DecryptAES256Hash(hashedText);

                decryptedTextLabel.Text = decryptedText; // Update the label with decrypted text
            }
        }

    }
}
